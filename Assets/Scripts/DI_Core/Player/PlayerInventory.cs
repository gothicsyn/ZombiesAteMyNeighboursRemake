// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
// TODO: Include a description of the file here.
//

using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct PlayerInventory
{
	public List<Ammo> playerAmmo;
	public List<Ammo> maxAmmo;
	public WeaponType selectedWeapon;
}