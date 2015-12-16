// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
// TODO: Include a description of the file here.
//

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System;

public class BuildManager : EditorWindow
{
	private string zipTool = PlayerPrefs.GetString("Zip Tool Location", "D:\\Program Files (x86)\\7-Zip\\7z.exe");
	private string zipArgs = PlayerPrefs.GetString("Zip Args", "a -mx9 -tzip");
	private string buildsFolder = PlayerPrefs.GetString("Builds Folder", "Builds");
	private string copyToFolder = PlayerPrefs.GetString("Builds Copy To Folder", "");
	private float progressbarProgress;
	private string progressbarMessage;
	private string progressbarTitle;
	private bool showBuildSettings = false;
	private bool showZipToolEditor = false;
	private bool showArgsEditor = false;
	private bool showBuildFolderEditor = false;
	private bool showCopyToFolderEditor = false;
	private string tempArgs;
	private string tempZipTool;
	private string tempBuildFolder;
	private string tempCopyToFolder;
	private bool isBuildingGame = false;
	private string buildLog;
	private Vector2 logScroll = Vector2.zero;
	private int startTime;
	private bool buildX86 = true;
	private bool buildX64 = true;
	private bool buildMac = true;
	private bool buildWebGL = true;

	public void OnGUI()
	{
		EditorGUILayout.SelectableLabel(BuildInfo.buildDisplayName);
		EditorGUILayout.Separator();

		showBuildSettings = EditorGUILayout.Foldout(showBuildSettings, "Build Settings");
		if (showBuildSettings) {
			EditorGUILayout.BeginVertical();
			EditorGUILayout.SelectableLabel("Zip Tool: " + zipTool, GUILayout.MaxWidth(1000));
			if (GUILayout.Button("Modify")) {
				tempZipTool = EditorUtility.OpenFilePanel("Zip Tool Location", "", "");
				showZipToolEditor = true;
			}
			if (showZipToolEditor) {
				EditorGUILayout.SelectableLabel("Selected Zip Tool: " + tempZipTool, GUILayout.MaxWidth(1000));
				if (GUILayout.Button("Save")) {
					showZipToolEditor = false;
					zipTool = tempZipTool;
					PlayerPrefs.SetString("Zip Tool Location", zipTool);
				}
				if (GUILayout.Button("Discard")) {
					showZipToolEditor = false;
				}
			}

			EditorGUILayout.SelectableLabel("Zip Arguments: " + zipArgs, GUILayout.MaxWidth(1000));
			if (!showArgsEditor) {
				if (GUILayout.Button("Modify")) {
					showArgsEditor = true;
					tempArgs = zipArgs;
				}
			}
			if (showArgsEditor) {
				tempArgs = EditorGUILayout.TextField("Zip Arguments: ", tempArgs, GUILayout.MaxWidth(1000));
				if (GUILayout.Button("Save")) {
					showArgsEditor = false;
					zipArgs = tempArgs;
					PlayerPrefs.SetString("Zip Args", zipArgs);
				}
				if (GUILayout.Button("Discard")) {
					showArgsEditor = false;
				}
			}

			EditorGUILayout.SelectableLabel("Builds Folder: " + buildsFolder, GUILayout.MaxWidth(1000));
			if (!showBuildFolderEditor) {
				if (GUILayout.Button("Modify")) {
					showBuildFolderEditor = true;
					tempBuildFolder = EditorUtility.OpenFolderPanel("Build Folder Location", "", "");
				}
			}
			if (showBuildFolderEditor) {
				EditorGUILayout.SelectableLabel("Selected Build Folder Location: " + tempBuildFolder, GUILayout.MaxWidth(1000));
				if (GUILayout.Button("Save")) {
					showBuildFolderEditor = false;
					buildsFolder = tempBuildFolder;
					PlayerPrefs.SetString("Builds Folder", buildsFolder);
				}
				if (GUILayout.Button("Discard")) {
					showBuildFolderEditor = false;
				}
			}

			EditorGUILayout.SelectableLabel("Copy Builds To Folder: " + copyToFolder, GUILayout.MaxWidth(1000));
			if (!showCopyToFolderEditor) {
				if (GUILayout.Button("Modify")) {
					showCopyToFolderEditor = true;
					tempCopyToFolder = EditorUtility.OpenFolderPanel("Copy Builds To Folder", "", "");
				}
			}
			if (showCopyToFolderEditor) {
				EditorGUILayout.SelectableLabel("Selected Build Folder Copy To Location: " + tempCopyToFolder, GUILayout.MaxWidth(1000));
				if (GUILayout.Button("Save")) {
					showCopyToFolderEditor = false;
					copyToFolder = tempCopyToFolder;
					PlayerPrefs.SetString("Builds Copy To Folder", copyToFolder);
				}
				if (GUILayout.Button("Discard")) {
					showCopyToFolderEditor = false;
				}
			}

			EditorGUILayout.EndVertical();
		}

		EditorGUILayout.Separator();

		EditorGUILayout.BeginHorizontal();
			buildX86 = GUILayout.Toggle(buildX86, "Build Windows x86");
			buildX64 = GUILayout.Toggle(buildX64, "Build Windows x64");
			buildMac = GUILayout.Toggle(buildMac, "Build Mac");
			buildWebGL = GUILayout.Toggle(buildWebGL, "Build WebGL");
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Separator();

		if (GUILayout.Button("Build Game")) {
			isBuildingGame = true;
			BuildGame();
		}

		if (isBuildingGame) {
			EditorUtility.DisplayProgressBar(progressbarTitle, progressbarMessage, progressbarProgress);
		}

		EditorGUILayout.LabelField("Build Log:");
		logScroll = EditorGUILayout.BeginScrollView(logScroll, false, true);
		EditorGUILayout.TextArea(buildLog, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
		EditorGUILayout.EndScrollView();
	}

	[MenuItem ("Devils Inc Studios/Build Tools/Build Manager")]
	public static void showWindow()
	{
		EditorWindow buildManager = EditorWindow.GetWindow(typeof(BuildManager));
		buildManager.maxSize = new Vector2(1000, 400);
		buildManager.minSize = new Vector2(1000, 400);
	}

	public string[] getScenes()
	{
		List<string> scenes = new List<string>();
		foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
			if (scene.enabled) {
				scenes.Add(scene.path);
			}
		}
		return scenes.ToArray();
	}

	public void updateProgressBar(float updatedProgress, string updatedMessage)
	{
		progressbarProgress = updatedProgress;
		progressbarMessage = updatedMessage;
		EditorUtility.DisplayProgressBar(progressbarTitle, progressbarMessage, progressbarProgress);
	}

	public void logMessage(string logMessage)
	{
		buildLog += System.Environment.NewLine + logMessage;
	}


	public void startBuild(string[] levels, string location, BuildTarget target, BuildOptions options)
	{
		int buildStartTime = (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
		logMessage("Started " + target.ToString() + " Build at: " + startTime);
		BuildPipeline.BuildPlayer(levels, location, target, options);
		int buildTime = (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
		logMessage("Finished " + target.ToString() + " Build in : " + (buildTime - buildStartTime) + " seconds");
		logMessage("Build Saved as: " + location);
	}

	public void compressBuild(BuildTarget target)
	{
		Process zipProcess = new System.Diagnostics.Process();
		switch (target) {
			case BuildTarget.StandaloneWindows:
				zipProcess.StartInfo.Arguments = zipArgs
					+ " \"" + copyToFolder + "\\" + BuildInfo.buildName + ".zip\""
						+ " \"" + buildsFolder + "\\" + BuildInfo.buildName + ".exe\""
						+ " \"" + buildsFolder + "\\" + BuildInfo.buildName + "_Data\"";
				break;
			case BuildTarget.StandaloneWindows64:
				zipProcess.StartInfo.Arguments = zipArgs
					+ " \"" + copyToFolder + "\\" + BuildInfo.buildName + "_x64.zip\""
				+ " \"" + buildsFolder + "\\" + BuildInfo.buildName + "_x64.exe\""
					+ " \"" + buildsFolder + "\\" + BuildInfo.buildName + "_x64_Data\"";
				break;
			case BuildTarget.StandaloneOSXUniversal:
				zipProcess.StartInfo.Arguments = zipArgs
					+ " \"" + copyToFolder + "\\" + BuildInfo.buildName + "_Mac.app.zip\""
					+ " \"" + buildsFolder + "\\" + BuildInfo.buildName + "_Mac.app\"";
				break;
			case BuildTarget.WebGL:
				zipProcess.StartInfo.Arguments = zipArgs
					+ " \"" + copyToFolder + "\\" + BuildInfo.buildName + "_WebGL.zip\""
					+ " \"" + buildsFolder + "\\" + BuildInfo.buildName + "_WebGL\"";
				break;
		}
		zipProcess.StartInfo.CreateNoWindow = false;
		zipProcess.StartInfo.FileName = "\"" + zipTool + "\"";
		zipProcess.StartInfo.RedirectStandardOutput = false;
		zipProcess.StartInfo.RedirectStandardError = false;
		zipProcess.StartInfo.UseShellExecute = false;
		logMessage("Compressing using zip args: " + zipProcess.StartInfo.Arguments);
		zipProcess.Start();
		zipProcess.WaitForExit();
		int zipStartTime = (int) zipProcess.StartTime.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
		int zipExitTime = (int) zipProcess.ExitTime.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
		logMessage("Finished Compression of " + target.ToString() + " Build in: " + (zipExitTime - zipStartTime) + " seconds");
	}

	public void BuildGame ()
	{
		if (!BuildPipeline.isBuildingPlayer) {
			progressbarTitle = "Building Game";
			buildLog = "Starting Build";
			startTime = (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;

			// Build X86 Version
			if (buildX86) {
				updateProgressBar(0f, "Building x86 Build");
				startBuild(getScenes(), buildsFolder + "/" + BuildInfo.buildName + ".exe", BuildTarget.StandaloneWindows, BuildOptions.None);
			}
			// Build X64 Version
			if (buildX64) {
				updateProgressBar(12.5f, "Building x64 Build");
				startBuild(getScenes(), buildsFolder + "/" + BuildInfo.buildName + "_x64.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
			}

			// Build Mac Version
			if (buildMac) {
				updateProgressBar(25f, "Building Mac Build");
				startBuild(getScenes(), buildsFolder + "/" + BuildInfo.buildName + "_Mac.app", BuildTarget.StandaloneOSXUniversal, BuildOptions.None);
			}

			// Build WebGL Version
			if (buildWebGL) {
				updateProgressBar(37.5f, "Building WebGL Build");
				startBuild(getScenes(), buildsFolder + "/" + BuildInfo.buildName + "_WebGL", BuildTarget.WebGL, BuildOptions.None);
			}

			// Build x86 Version
			if (buildX86) {
				updateProgressBar(50f, "Compressing x86 Build");
				compressBuild(BuildTarget.StandaloneWindows);
			}

			// Build x64 Version
			if (buildX64) {
				updateProgressBar(62.5f, "Compressing x64 Build");
				compressBuild(BuildTarget.StandaloneWindows64);
			}

			// Build Mac Version
			if (buildMac) {
				updateProgressBar(75f, "Compressing Mac Build");
				compressBuild(BuildTarget.StandaloneOSXUniversal);
			}

			// Build WebGL Version
			if (buildWebGL) {
				updateProgressBar(87.5f, "Compressing WebGL Build");
				compressBuild(BuildTarget.WebGL);
			}

			PlayerPrefs.SetFloat("Build Number", BuildInfo.buildNumber + 1);
			BuildInfo.refreshInfo();
			logMessage("Finished building in: " + ((int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds - startTime) + " seconds");
			EditorUtility.ClearProgressBar();
			isBuildingGame = false;
		}
	}
}