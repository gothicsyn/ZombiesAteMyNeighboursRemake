// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using System;
using System.Collections.Generic;

[AddComponentMenu("Entities/Player")]
public class PlayerEntity : Entity
{
	public PlayerState playerState;

	[Header("Weapons Setup")]
	public GameObject leftGun;
	public GameObject rightGun;
	public GameObject mainGun;

	[Header("Combat Settings")]
	public bool inCombat = false;
	public float combatTime = 5.0f;
	public float combatTimer = 0.0f;
	
	public List<float> shotDelayTimers;
	[HideInInspector]
	public GameObject selectedWeapon;
	
	private float shotDelayTimer = 0.0f;
	private float shotDelay = 0.0f;

	[Header("Key Binds")]
	public string shootKeyBinding;
	public string chargeKeyBinding;
	public string useDampenersKeyBinding;
	public string useSpecialKeyBinding;
	public string useItemKeyBinding;
	public string nextItemKeyBinding;
	public string previousItemKeyBinding;
	public string nextWeaponKeyBinding;
	public string previousWeaponKeyBinding;

	
	new public void OnEnable()
	{
		base.OnEnable();
		playerState.entity = this;
		shotDelayTimers = new List<float>();
		foreach (WeaponType type in (WeaponType[]) Enum.GetValues(typeof(WeaponType))) {
			shotDelayTimers.Insert((int)type, 0.0f);
		}
	}

	public void ShootProjectile()
	{
		float ammoCount;

		GameObject instPoint;

		if (shotDelayTimers[(int)playerState.getSelectedWeapon()] <= 0.0f) {
			switch (playerState.getSelectedWeapon()) {
			case WeaponType.MAIN_GUN:
				ammoCount = playerState.getAmmo(WeaponType.MAIN_GUN);
				instPoint = mainGun;
				break;
			case WeaponType.LASER_SHOT:
				ammoCount = playerState.getAmmo(WeaponType.LASER_SHOT);
				instPoint = leftGun;
				break;
			case WeaponType.TRACKING_SHOT:
				ammoCount = playerState.getAmmo(WeaponType.TRACKING_SHOT);
				instPoint = rightGun;
				break;
			case WeaponType.DRONE:
				ammoCount = playerState.getAmmo(WeaponType.DRONE);
				instPoint = mainGun;
				break;
			case WeaponType.SHOCKWAVE:
				ammoCount = playerState.getAmmo(WeaponType.SHOCKWAVE);
				instPoint = mainGun;
				break;
			default:
				ammoCount = 1;
				instPoint = mainGun;
				break;
			}

			if (ammoCount > 0 || playerState.getSelectedWeapon() == WeaponType.MAIN_GUN) {

				GameObject firedProjectile = PoolController.instance.getPooledObject(playerState.getSelectedWeapon().ToString());
				firedProjectile.SetActive(true);
				firedProjectile.transform.position = instPoint.transform.position;
				firedProjectile.transform.rotation = instPoint.transform.parent.transform.parent.transform.rotation;
				
				// Get the projectiles controller
				ProjectileController projCont = firedProjectile.GetComponent<ProjectileController>();
				float speed = projCont.projectileSpeed;
				
				// Apply force to move the projectile
				Rigidbody rigidBody = firedProjectile.GetComponent<Rigidbody>();
				rigidBody.AddRelativeForce(Vector3.right * speed);
				rigidBody.AddRelativeForce(rigidBody.GetRelativePointVelocity(transform.position), ForceMode.Impulse);

				// FIXME remove the ammo
				//RemovePlayerAmmo(ammoCount);
				DI_Events.EventCenter<WeaponType, PlayerState>.invoke("OnFire", playerState.getSelectedWeapon(), playerState);
				shotDelayTimers[(int)playerState.getSelectedWeapon()] = projCont.shotDelay;

				// Laser fires two shots.
				if (playerState.getSelectedWeapon() == WeaponType.LASER_SHOT) {
					instPoint = rightGun;
					firedProjectile = PoolController.instance.getPooledObject(playerState.getSelectedWeapon().ToString());
					firedProjectile.SetActive(true);
					firedProjectile.transform.position = instPoint.transform.position;
					firedProjectile.transform.rotation = instPoint.transform.parent.transform.parent.transform.rotation;
					
					// Get the projectiles controller
					projCont = firedProjectile.GetComponent<ProjectileController>();
					speed = projCont.projectileSpeed;

                    if (hasQuadDamage)
                    {
                        projCont.damage *= 4;
                    }

					// Apply force to move the projectile
					rigidBody = firedProjectile.GetComponent<Rigidbody>();
					rigidBody.AddRelativeForce(Vector3.right * speed);
					rigidBody.AddRelativeForce(rigidBody.GetRelativePointVelocity(transform.position), ForceMode.Impulse);
					
					// FIXME remove the ammo
					//RemovePlayerAmmo(ammoCount);
					DI_Events.EventCenter<WeaponType, PlayerState>.invoke("OnFire", playerState.getSelectedWeapon(), playerState);
				}
			}
			
			// Update the player
			combatTimer = combatTime;
			if (inCombat == false) {
				inCombat = true;
				DI_Events.EventCenter.invoke("OnEnterCombat");
			}
		}
	}

	public void LateUpdate()
	{
		if (isLocalPlayer) {
			if (GameStateController.instance.gameState == GameStates.PLAYING) {
				if(Input.GetButton(shootKeyBinding)){
					if (shotDelayTimer == 0.0f) {
						ShootProjectile();
					}
				}

				if (Input.GetButtonDown(nextWeaponKeyBinding)) {
					playerState.selectNextWeapon();
				}

				if (Input.GetButtonDown(previousWeaponKeyBinding)) {
					playerState.selectPreviousWeapon();
				}

				if (Input.GetButtonDown(nextItemKeyBinding)) {
					playerState.selectNextItem();
				}
				
				if (Input.GetButtonDown(previousItemKeyBinding)) {
					playerState.selectPreviousItem();
				}

				for (int index = 0; index < shotDelayTimers.Count; ++index) {
					if (shotDelayTimers[index] > 0.0f) {
						shotDelayTimers[index] -= Time.deltaTime;
					}
					if (shotDelayTimers[index] < 0.0f) {
						shotDelayTimers[index] = 0.0f;
					}
				}
				
				combatTimer = Mathf.Clamp(combatTimer - Time.deltaTime, 0, combatTime);
				
				if (combatTimer <= 0.0f) {
					combatTimer = 0.0f;
					inCombat = false;
					DI_Events.EventCenter.invoke("OnExitCombat");
				}
			}
		}
	}
}