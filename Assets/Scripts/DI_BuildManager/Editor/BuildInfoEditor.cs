// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
// TODO: Include a description of the file here.
//

using UnityEditor;
using UnityEngine;
using SardonicMe.Perlib;

public class BuildInfoEditor : EditorWindow
{
	public string displayName = BuildInfo.buildInfoFile.GetValue<string>("Build Display Name", "");
	public string buildName = BuildInfo.buildInfoFile.GetValue<string>("Build Name", "");
	public float majorRevNumber = BuildInfo.buildInfoFile.GetValue<float>("Build Major Rev", 0f);
	public float minorRevNumber = BuildInfo.buildInfoFile.GetValue<float>("Build Minor Rev", 0f);
	public float buildNumber = BuildInfo.buildInfoFile.GetValue<float>("Build Number", 0f);
	[MenuItem ("Devils Inc Studios/Build Tools/Build Info Manager")]
	public static void showWindow()
	{
		EditorWindow buildInfoEditor = EditorWindow.GetWindow(typeof(BuildInfoEditor));
		buildInfoEditor.maxSize = new Vector2(600, 200);
		buildInfoEditor.minSize = new Vector2(600, 200);
	}

	public void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		displayName = EditorGUILayout.TextField("Build Display Name", displayName);
		buildName = EditorGUILayout.TextField("Build File Name", buildName);
		majorRevNumber = EditorGUILayout.FloatField("Major Rev Number", majorRevNumber);
		minorRevNumber = EditorGUILayout.FloatField("Minor Rev Number", minorRevNumber);
		buildNumber = EditorGUILayout.FloatField("Build Number", buildNumber);
		if (GUILayout.Button("Save")) {
			BuildInfo.buildInfoFile.SetValue<string>("Build Display Name", displayName);
			BuildInfo.buildInfoFile.SetValue<string>("Build Name", buildName);
			BuildInfo.buildInfoFile.SetValue<float>("Build Major Rev", majorRevNumber);
			BuildInfo.buildInfoFile.SetValue<float>("Build Minor Rev", minorRevNumber);
			BuildInfo.buildInfoFile.SetValue<float>("Build Number", buildNumber);
			BuildInfo.refreshInfo();
		}
		EditorGUILayout.EndVertical();
	}
}