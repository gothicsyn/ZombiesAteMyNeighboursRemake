using UnityEngine;
using System;

[Serializable]
public struct Ammo
{
	public WeaponType weaponType;
	public float ammoCount;

	public Ammo(WeaponType type, float ammo)
	{
		weaponType = type;
		ammoCount = ammo;
	}
}