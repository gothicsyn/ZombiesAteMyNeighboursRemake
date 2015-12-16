// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

#region Includes
using UnityEngine;
using System;
#endregion

[AddComponentMenu("Game State/Debug/Debug Controller")]
public class DebugController : MonoBehaviour
{
	public static DebugController instance;
	public static DebugLevels debugLevel = DebugLevels.NONE;

	public void Awake()
	{
		// Assign the self reference.
		instance = this;
	}

	public void setDebugLevel(DebugLevels level)
	{
		debugLevel = level;
	}

	public void disableDebug()
	{
		debugLevel = DebugLevels.NONE;
	}
}
