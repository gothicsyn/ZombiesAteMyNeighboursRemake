// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
// TODO: Include a description of the file here.
//

using UnityEngine;
using System.Collections.Generic;

public class Asteroid : Enemy
{
	public List<Material> availableMaterials;
	public string explosionPrefab;

	new public void OnEnable()
	{
//		Debug.Log("I'm Alive");
		GetComponent<MeshRenderer>().material = availableMaterials[UnityEngine.Random.Range(0, availableMaterials.Count)];
		this.GetComponent<MeshRenderer>().enabled = true;

		// The base class will remove the events
		DI_Events.EventCenter<Entity, Entity, float, WeaponType>.addListener("OnHit", handleOnHit);
		DI_Events.EventCenter<Entity, Entity, float, float>.addListener("OnChill", base.handleOnChill);
		DI_Events.EventCenter<Entity, Entity, float, float>.addListener("OnBurn", base.handleOnBurn);
		DI_Events.EventCenter<Entity, Entity, float>.addListener("OnFreeze", base.handleOnFreeze);
		DI_Events.EventCenter<Entity, Entity, float, float>.addListener("OnPoison", base.handleOnPoison);

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
			if (health <= 0.0f && !isDead) {
//				Debug.Log("OnDeath");
				OnDeath(GameStateController.instance.playerState.entity);
			}
		}
	}

	new public void OnDeath(Entity attacker)
	{
		this.GetComponent<MeshRenderer>().enabled = false;
		GameObject explosion = PoolController.instance.getPooledObject(explosionPrefab);
		explosion.transform.position = this.transform.position;
		explosion.SetActive(true);
		base.OnDeath(attacker);
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
}