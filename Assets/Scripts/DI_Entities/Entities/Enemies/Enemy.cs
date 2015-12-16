// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
//using UnityEditor;

[AddComponentMenu("Entities/Enemy")]
public class Enemy : Entity
{
	[Header("Enemy Settings")]
	public Enemies type;
	public bool ranged = false;
	[HideInInspector]
	public bool isAttacking = false;
	[HideInInspector]
	public bool shielded = false;
	[HideInInspector]
	public float shieldTimer = 0.0f;

	[Header("Damage Settings")]
	public bool canAttack = true;
	public float damage = 10.0f;
	public float attackSpeed = 1.0f;
	public GameObject firingPoint;

	[Header("Loot Settings")]
	public float points = 0.0f;
	public float itemDropChance = 25.0f;
	public ItemDropChanceTable itemDrops;

	[Header("GUI Settings")]
	public GameObject statusBar;
	public GameObject burningIcon;
	public GameObject chilledIcon;
	public GameObject frozenIcon;
	public GameObject poisonedIcon;

	[HideInInspector]
	public WaypointAI waypointAgent;
	[HideInInspector]
	public bool isBurningImmuneOriginal;
	[HideInInspector]
	public bool isChilledImmuneOriginal;
	[HideInInspector]
	public bool isFrozenImmuneOriginal;
	[HideInInspector]
	public bool isPoisonImmuneOriginal;
	[HideInInspector]
	public PrefabLoader prefabLoader;

	[Header("Weapon Settings")]
	public WeaponType selectedWeapon;

	#region Public Methods
	new public void OnEnable()
	{
		if (canMove) {
			waypointAgent = this.GetComponent<WaypointAI>();
		}
		// If this isn't the first spawn we need to clean up some stuff.
		if (isDead) {
			isDead = false;
			isChilled = false;
			isPoisoned = false;
			isFrozen = false;
			isBurning = false;
			burnTimeRemaining = 0.0f;
			burnDamage = 0.0f;
			chilledTimeRemaining = 0.0f;
			chilledSpeedReduction = 0.0f;
			poisonDamage = 0.0f;
			poisonedTimeRemaining = 0.0f;
			frozenTimeRemaining = 0.0f;
			isBurningImmune = isBurningImmuneOriginal;
			isFrozenImmune = isFrozenImmuneOriginal;
			isChilledImmune = isChilledImmuneOriginal;
			isPoisonImmune = isPoisonImmuneOriginal;
			shieldTimer = 0.0f;
			shielded = false;
			this.GetComponent<CharacterController>().enabled = true;
			setEffectColor(originalColor);
		}
		else {
			originalMovementSpeed = movementSpeed;
			originalMaxHealth = maxHealth;
			isBurningImmuneOriginal = isBurningImmune;
			isChilledImmuneOriginal = isChilledImmune;
			isFrozenImmuneOriginal = isFrozenImmune;
			isPoisonImmuneOriginal = isPoisonImmune;
			prefabLoader = GameStateController.instance.gameObject.GetComponent<PrefabLoader>();
			originalColor = new List<Color>();
			renderers = new List<Renderer>();
			foreach (Renderer childRenderer in this.gameObject.transform.GetComponentsInChildren<Renderer>()) {
				if (childRenderer.GetComponent<Renderer>().material != null) {
					if (childRenderer.GetComponent<Renderer>().material.HasProperty("_Color")) {
						originalColor.Add(childRenderer.GetComponent<Renderer>().material.color);
						renderers.Add(childRenderer);
					}
				}
			}
			if (hasAnimations) {
				entityAnimator = GetComponentInChildren<Animator>();
				if (entityAnimator == null) {
					hasAnimations = false;
				}
			}
		}


		// The base class will remove the events
		DI_Events.EventCenter<Entity, Entity, float, WeaponType>.addListener("OnHit", handleOnHit);
		DI_Events.EventCenter<Entity, Entity, float, float>.addListener("OnChill", base.handleOnChill);
		DI_Events.EventCenter<Entity, Entity, float, float>.addListener("OnBurn", base.handleOnBurn);
		DI_Events.EventCenter<Entity, Entity, float>.addListener("OnFreeze", base.handleOnFreeze);
		DI_Events.EventCenter<Entity, Entity, float, float>.addListener("OnPoison", base.handleOnPoison);

		//DI_Events.EventCenter<Entity, Entity, SlowEffect>.addListener("OnSlow", base.handleOnSlow);
		health = maxHealth;
		if (!isImmortal && healthBar != null) {
			healthBar.fillAmount = health / maxHealth;
			healthValue.text = (healthBar.fillAmount * 100) + "%";
			if (health == maxHealth) {
				healthBar.transform.parent.gameObject.SetActive(false);
			}
			else {
				healthBar.transform.parent.gameObject.SetActive(true);
			}
		}
	}

	new public void OnDisable()
	{
		DI_Events.EventCenter<Entity, Entity, float, float>.removeListener("OnChill", base.handleOnChill);
		DI_Events.EventCenter<Entity, Entity, float, float>.removeListener("OnBurn", base.handleOnBurn);
		DI_Events.EventCenter<Entity, Entity, float>.removeListener("OnFreeze", base.handleOnFreeze);
		DI_Events.EventCenter<Entity, Entity, float, float>.removeListener("OnPoison", base.handleOnPoison);
		DI_Events.EventCenter<Entity, Entity, float, WeaponType>.removeListener("OnHit", handleOnHit);
	}

	new public void Update()
	{
		if (!isDead) {
			if (shieldTimer > 0.0f) {
				shieldTimer -= Time.deltaTime;
				isBurningImmune = true;
				isChilledImmune = true;
				isFrozenImmune = true;
				isPoisonImmune = true;
			}
			if (shielded && shieldTimer <= 0.0f) {
				shielded = false;
				isBurningImmune = isBurningImmuneOriginal;
				isChilledImmune = isChilledImmuneOriginal;
				isFrozenImmune = isFrozenImmuneOriginal;
				isPoisonImmune = isPoisonImmuneOriginal;
			}

			if (isBurning || isChilled || isFrozen || isPoisoned) {
				if (shielded) {
					burnTimeRemaining = 0.0f;
					chilledTimeRemaining = 0.0f;
					frozenTimeRemaining = 0.0f;
					poisonedTimeRemaining = 0.0f;
				}

				statusBar.SetActive(true);
				if (isBurning) {
					burningIcon.SetActive(true);
				}
				else {
					burningIcon.SetActive(false);
				}

				if (isFrozen) {
					frozenIcon.SetActive(true);
				}
				else {
					frozenIcon.SetActive(false);
				}

				if (isChilled) {
					chilledIcon.SetActive(true);
				}
				else {
					chilledIcon.SetActive(false);
				}

				if (isPoisoned) {
					poisonedIcon.SetActive(true);
				}
				else {
					poisonedIcon.SetActive(false);
				}
			}
			else {
				statusBar.SetActive(false);
			}

			if (health <= 0.0f && !isDead) {
				OnDeath(GameStateController.instance.playerState.entity);
			}
		}
		CancelInvoke();
		base.Update();
	}

	new public void OnDeath(Entity attacker)
	{
		DI_Events.EventCenter<PlayerState>.invoke("OnUpdateHudRequest", GameStateController.instance.playerState);
		statusBar.SetActive(false);

		// TODO powerups rolls
		float dropRoll = UnityEngine.Random.Range(0, 100);
		#if DEBUG
		Debug.Log("Ammo Roll: " + dropRoll);
		#endif
		if (itemDropChance >= dropRoll) {
			#if DEBUG
				Debug.Log("Ammo Roll: SUCCESS");
			#endif
			//spawnRandomPowerup();
		}
		else {
			#if DEBUG
				Debug.Log("Ammo Roll: FAIL");
			#endif
		}

		if (this.GetComponent<CharacterController>() != null) {
			this.GetComponent<CharacterController>().enabled = false;
		}
		StopAllCoroutines();
		base.OnDeath(attacker);
	}

	public void spawnRandomPowerup()
	{
		int weightRoll = UnityEngine.Random.Range(0, 100);

		foreach (ItemDropChance data in itemDrops.drops) {
			if (weightRoll > data.rangeMin && weightRoll <= data.rangeMax) {
				#if DEBUG
					Debug.Log("selectRandomPowerUp - Rolled a " + weightRoll + " this is translated to: " + data.type.ToString());
				#endif
				GameObject powerUp = prefabLoader.getPrefab(data.type.ToString());
				if (powerUp != null) {
					GameObject _spawnedPowerUp = PoolController.instance.getPooledObject(powerUp.name);
					_spawnedPowerUp.SetActive(true);
					_spawnedPowerUp.transform.position = transform.position;
					_spawnedPowerUp.transform.rotation = transform.rotation;
				}
			}
		}
	}

	/// <summary>
	/// Handles the on hit.
	/// </summary>
	/// <param name="target">Target.</param>
	/// <param name="attacker">Attacker.</param>
	/// <param name="damage">Damage.</param>
	new public void handleOnHit(Entity target, Entity attacker, float damage, WeaponType weaponType)
	{
		if (target == this) {
			if (!isDead) {
				if (!isImmortal) {
					//Shieldman's shield cuts off 90% damage for everyone except shieldmen.
					if (shielded) {
						// If its a single target ammo type allow it to deal damage.
						switch (weaponType) {
							case WeaponType.LASER_SHOT:
							case WeaponType.TRACKING_SHOT:
							case WeaponType.DRONE:
								damage *= 0.10f;
							break;

							// Main gun does full damage
							// As does shockwaves.
							case WeaponType.MAIN_GUN:
							case WeaponType.SHOCKWAVE:
							break;

							// AOE effects do 0 damage
							default:
								damage = 0.0f;
							break;
						}
					}
					health -= damage;
					healthBar.fillAmount = health / maxHealth;
					healthValue.text = (healthBar.fillAmount * 100) + "%";
					healthBar.transform.parent.gameObject.SetActive(true);

					if (health <= 0) {
						this.OnDeath(attacker);
					}
					else {
						DI_Events.EventCenter<float, Enemies, WeaponType>.invoke("OnDamage", damage, this.type, weaponType);
					}
				}
			}
		}
	}

	public void gainShield()
	{
		shielded = true;
		shieldTimer = 0.15f;
	}

	public IEnumerator Attack(Entity target) {
		isAttacking = true;
		DI_Events.EventCenter<Entity, Entity, float, WeaponType>.invoke("OnHit", target, this, damage, selectedWeapon);
		yield return new WaitForSeconds(attackSpeed);
		isAttacking = false;
	}

	public void startAttack(WeaponType firedWeapon)
	{
		if (!isAttacking) {
			StartCoroutine(Attack(firedWeapon));
		}
	}

	public IEnumerator Attack(WeaponType firedWeapon) {
		isAttacking = true;

		Debug.Log("Attack starting with: " + firedWeapon.ToString());
		GameObject firedProjectile = PoolController.instance.getPooledObject(firedWeapon.ToString());
		Debug.Log("Requesting Pooled object: " + firedProjectile.name);
		firedProjectile.transform.position = firingPoint.transform.position;
		firedProjectile.transform.rotation = firingPoint.transform.parent.transform.parent.transform.rotation;
		firedProjectile.SetActive(true);

		// Get the projectiles controller
		ProjectileController projCont = firedProjectile.GetComponent<ProjectileController>();
		float speed = projCont.projectileSpeed;
		
		// Apply force to move the projectile
		Rigidbody rigidBody = firedProjectile.GetComponent<Rigidbody>();
		rigidBody.AddRelativeForce(Vector3.forward * speed);
		rigidBody.AddRelativeForce(rigidBody.GetRelativePointVelocity(transform.position), ForceMode.Impulse);

		// Wait for the attack delay before returning
		yield return new WaitForSeconds(attackSpeed);
		isAttacking = false;
	}

	#endregion
}