// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

#region Includes
using UnityEngine;
using UnityEngine.UI;
using System;
#endregion

[Serializable]
public struct HudElements
{
	#region Public variables
	public Text score;
	public Text wave;
	public Text enemies;
	
	public Text slot1Ammo;
	public Text slot2Ammo;
	public Text slot3Ammo;
	public Text slot4Ammo;
	public Text slot5Ammo;
	public Text slot6Ammo;
	public Text slot7Ammo;

	public UnityEngine.UI.Image slot1;
	public UnityEngine.UI.Image slot2;
	public UnityEngine.UI.Image slot3;
	public UnityEngine.UI.Image slot4;
	public UnityEngine.UI.Image slot5;
	public UnityEngine.UI.Image slot6;
	public UnityEngine.UI.Image slot7;
	#endregion
}