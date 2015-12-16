// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.Networking;

[RequireComponent(typeof(SphereCollider))]
[AddComponentMenu("Entities/Projectiles/AOE Damage")]
public class AOEDamage : NetworkBehaviour
{
	[HideInInspector]
	public List<Enemy> enemyToDamage = new List<Enemy>();

	[Header("Damage Settings")]
	public float damage;
	[HideInInspector]
	public float originalDamage;

	[Header("Weapon Settings")]
	public float explosionDelay = 0.0f;
	public Entity owner;
	public bool canTargetAir = false;
	public WeaponType type;

	[Header("SFX Settings")]
	public AudioClip weaponSFX;

	[Header("Status Settings: Burn")]
	public bool canBurn = false;
	public float burnChance = 0.0f;
	public float burnDamage = 0.0f;
	public float burnDuration = 0.0f;
	
	[Header("Status Settings: Freeze")]
	public bool canFreeze = false;
	public float freezeChance = 0.0f;
	public float freezeDuration = 0.0f;
	
	[Header("Status Settings: Chill")]
	public bool canChill = false;
	public float chillChance = 0.0f;
	public float chillSlow = 0.0f;
	public float chillDuration = 0.0f;
	
	[Header("Status Settings: Poison")]
	public bool canPoison = false;
	public float poisonChance = 0.0f;
	public float poisonDamage = 0.0f;
	public float poisonDuration = 0.0f;

	[HideInInspector]
	public bool firstSpawn = true;
	public void OnEnable()
	{
		if (firstSpawn) {
			firstSpawn = false;
			originalDamage = damage;
		}
		enemyToDamage.Clear ();
		owner = GameObject.Find("Player One").GetComponent<PlayerEntity>();
		StartCoroutine("explode");
		DI_Events.EventCenter<AudioClip, float>.invoke("OnPlayEffect", weaponSFX, 0.25f);
	}

	public void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Enemy" && col.name != "Hit Box") {
			Enemy enemy = col.GetComponent<Enemy>();
			if (!enemyToDamage.Contains(enemy)) {
				enemyToDamage.Add(enemy);
			}
		}
	}
	
	public void OnTriggerExit(Collider col)
	{
		if (col.tag == "Enemy" && col.name != "Hit Box") {
			Enemy enemy = col.GetComponent<Enemy>();
			if (enemyToDamage.Contains(enemy)) {
				enemyToDamage.Add(enemy);
			}
		}
	}

	public IEnumerator explode()
	{
		yield return new WaitForSeconds(explosionDelay);
		GameObject explosion = PoolController.instance.getPooledObject("Explosion");
		explosion.transform.position = this.transform.position;
		explosion.transform.localEulerAngles = Vector3.zero;
		explosion.SetActive(true);
		for(int index = 0; index < enemyToDamage.Count; index++){
			if (enemyToDamage.Count > index) {
				if (!enemyToDamage[index].isDead) {
					DI_Events.EventCenter<Entity, Entity, float, WeaponType>.invoke("OnHit", enemyToDamage[index], owner, damage, type);
					float statusRoll = UnityEngine.Random.Range(0, 100);
					
					if (canBurn && burnChance >= statusRoll) {
						//Debug.Log("AOE Damage: Burn!");
						DI_Events.EventCenter<Entity, Entity, float, float>.invoke("OnBurn", enemyToDamage[index], owner, burnDuration, burnDamage);
					}
					
					if (canFreeze && freezeChance >= statusRoll) {
						//Debug.Log("AOE Damage: Freeze!");
						DI_Events.EventCenter<Entity, Entity, float>.invoke("OnFreeze", enemyToDamage[index], owner, freezeDuration);
					}
					
					if (canChill && chillChance >= statusRoll) {
						//Debug.Log("AOE Damage: Chill!");
						DI_Events.EventCenter<Entity, Entity, float, float>.invoke("OnChill", enemyToDamage[index], owner, chillDuration, chillSlow);
					}
					
					if (canPoison && poisonChance >= statusRoll) {
						//Debug.Log("AOE Damage: Poison!");
						DI_Events.EventCenter<Entity, Entity, float, float>.invoke("OnPoison", enemyToDamage[index], owner, poisonDuration, poisonDamage);
					}
				}
			}
		}
		//explosion.transform.parent = null;
		this.gameObject.SetActive(false);
	}
}