// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
// TODO: Include a description of the file here.
//

using UnityEngine;
using SardonicMe.Perlib;

public static class BuildInfo
{
	public static string buildInfoFileName = "build.info";
	public static Perlib buildInfoFile = new Perlib(buildInfoFileName);
	public static float majorRevNumber = buildInfoFile.GetValue<float>("Build Major Rev", 0f);
	public static float minorRevNumber = buildInfoFile.GetValue<float>("Build Minor Rev", 0f);
	public static float buildNumber = buildInfoFile.GetValue<float>("Build Number", 0f);
	public static string buildId = majorRevNumber + "." + minorRevNumber + "b" + buildNumber;
	public static string buildDisplayName = buildInfoFile.GetValue<string>("Build Display Name", "") + ": " + buildId;
	public static string buildName = buildInfoFile.GetValue<string>("Build Name", "") + "_" + buildId;

	public static void refreshInfo()
	{
		majorRevNumber = buildInfoFile.GetValue<float>("Build Major Rev", 0f);
		minorRevNumber = buildInfoFile.GetValue<float>("Build Minor Rev", 0f);
		buildNumber = buildInfoFile.GetValue<float>("Build Number", 0f);
		buildId = majorRevNumber + "." + minorRevNumber + "b" + buildNumber;
		buildDisplayName = buildInfoFile.GetValue<string>("Build Display Name", "") + ": " + buildId;
		buildName = buildInfoFile.GetValue<string>("Build Name", "") + "_" + buildId;
	}
}