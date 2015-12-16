// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
// TODO: This script needs to load the status of the cheats from the save file (which doesn't exist yet)
//

#region Includes
using UnityEngine;
using System;
using System.Collections.Generic;
#endregion

[AddComponentMenu("Game State/Cheat/Cheat Controller")]
public class CheatController : MonoBehaviour
{
	public static CheatController instance;
	public static Dictionary<Cheat, bool> cheatStatus = new Dictionary<Cheat, bool>();
	
	public void Awake()
	{
		// Assign the self reference.
		instance = this;
		
		// Populate the initial list contents with all cheats disabled.
		string[] cheats = Enum.GetNames(typeof(Cheat));
		for (int index = 0; index < cheats.Length; ++index) {
			cheatStatus.Add((Cheat)Enum.Parse(typeof(Cheat), cheats[index]), false);
		}
	}
	
	public void enableCheat(Cheat cheat)
	{
		cheatStatus[cheat] = true;
		PlayerPrefs.SetInt("Used Cheats", 1);
	}
	
	public void disableCheat(Cheat cheat)
	{
		cheatStatus[cheat] = false;
	}
	
	public bool cheatEnabled(Cheat cheat)
	{
		return cheatStatus[cheat];
	}
}
