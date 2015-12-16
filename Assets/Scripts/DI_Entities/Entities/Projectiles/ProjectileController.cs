// // Devils Inc Studios
// // Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
// //
// // TODO: Include a description of the file here.
// //

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ProjectileController : NetworkBehaviour {

    List<Entity> enemyList = new List<Entity>();

	[Header("Projectile Settings")]
	public float spawnTimer = 5;
	public float projectileSpeed = 0.0f;
    public bool hasSplashDamage = false;
    public float splashRadius = 30.0f;
    public float splashDamageDivision = 4.0f;
    private float splashDamage = 0.0f;
	public bool isEnemyProjectile;
	public WeaponType weaponType;

	[Header("SFX Settings")]
	public AudioClip hitSound;
	public float hitSoundVolume = 0.25f;
	public AudioClip weaponSFX;

	[Header("Damage Settings")]
	public float shotDelay = 0.0f;
	public float damage = 0.0f;
	[HideInInspector]
	public float originalPointReduction = 0.0f;

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
	public int enemiesHit = 0;

	[HideInInspector]
	public Entity owner;

	[HideInInspector]
	public bool firstSpawn = true;

    private GameObject target;

	public void OnEnable()
	{
		if (firstSpawn) {
			originalPointReduction = damage;
		}

		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
       
        if (hasSplashDamage)
            splashDamage = damage / splashDamageDivision;

		enemiesHit = 0;
		owner = GameStateController.instance.playerState.entity;

		DI_Events.EventCenter<AudioClip, float>.invoke("OnPlayEffect", weaponSFX, 0.25f);
		StartCoroutine(expire());
	}

    void Update()
    {
       
        if (weaponType == WeaponType.TRACKING_SHOT)
        {
            if (enemyList.Count != 0)
            {
                float distance;
                distance = Vector3.Distance(transform.position, enemyList[0].transform.position);

                for (int i = 1; i < enemyList.Count; i++)
                {
                    if (Vector3.Distance(transform.position, enemyList[i].transform.position) < distance)
                        distance = Vector3.Distance(transform.position, enemyList[i].transform.position);
                }

                transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * projectileSpeed);
            }
        }
    }

	public IEnumerator expire()
	{
		yield return new WaitForSeconds(spawnTimer);
		this.gameObject.SetActive(false);
	}

	public void OnCollisionEnter (Collision other)
	{
		if (isEnemyProjectile && other.gameObject.tag == "Player" && weaponType.ToString().Contains("ENEMY_")) {
            if(hasSplashDamage)
            {
                if (enemyList.Count != 0)
                {
                    for (int i = 0; i < enemyList.Count; i++)
                    {
                        if (Vector3.Distance(transform.position, enemyList[i].transform.position) <= splashRadius && enemyList[i] != other.gameObject.GetComponent<Entity>())
                        {
                            DI_Events.EventCenter<Entity, Entity, float, WeaponType>.invoke("OnHit", enemyList[i], owner, (float)splashDamage, weaponType);
                        }
                    }
                }
            }

			DI_Events.EventCenter<AudioClip, float>.invoke("OnPlayEffect", hitSound, 1.0f);
			DI_Events.EventCenter<Entity, Entity, float, WeaponType>.invoke("OnHit", other.gameObject.GetComponent<Entity>(), owner, (float)damage, weaponType);
			this.gameObject.SetActive(false);
		}
		else if (!isEnemyProjectile && other.gameObject.tag == "Enemy") {

            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (hasSplashDamage)
            {
                if (enemyList.Count != 0)
                {
                    for (int i = 0; i < enemyList.Count; i++)
                    {
                        if (Vector3.Distance(transform.position, enemyList[i].transform.position) <= splashRadius && enemyList[i].gameObject.GetComponent<Enemy>() != enemy)
                        {
                            DI_Events.EventCenter<Entity, Entity, float, WeaponType>.invoke("OnHit", enemyList[i].gameObject.GetComponent<Enemy>(), owner, (float)splashDamage, weaponType);
                        }
                    }
                }
            }
			DI_Events.EventCenter<Entity, Entity, float, WeaponType>.invoke("OnHit", enemy, owner, damage, weaponType);
			DI_Events.EventCenter<AudioClip, float, Vector3>.invoke("OnPlayEffectAtPoint", hitSound, hitSoundVolume, other.transform.position);

			float statusRoll = UnityEngine.Random.Range(0, 100);

			if (canBurn && burnChance >= statusRoll) {
				//Debug.Log("Projectile Controller: Burn!");
				DI_Events.EventCenter<Entity, Entity, float, float>.invoke("OnBurn", enemy, owner, burnDuration, burnDamage);
			}

			if (canFreeze && freezeChance >= statusRoll) {
				//Debug.Log("Projectile Controller: Freeze!");
				DI_Events.EventCenter<Entity, Entity, float>.invoke("OnFreeze", enemy, owner, freezeDuration);
			}

			if (canChill && chillChance >= statusRoll) {
				//Debug.Log("Projectile Controller: Chill!");
				DI_Events.EventCenter<Entity, Entity, float, float>.invoke("OnChill", enemy, owner, chillDuration, chillSlow);
			}

			if (canPoison && poisonChance >= statusRoll) {
				//Debug.Log("Projectile Controller: Poison!");
				DI_Events.EventCenter<Entity, Entity, float, float>.invoke("OnPoison", enemy, owner, poisonDuration, poisonDamage);
			}

			this.gameObject.SetActive(false);
		}
	}

    public void OnTriggerEnter(Collider other)
    {
        if (isEnemyProjectile && other.gameObject.tag == "Player")
        {
            enemyList.Add(other.gameObject.GetComponent<Entity>());
        }
        else if (!isEnemyProjectile && other.gameObject.tag == "Enemy")
        {
            enemyList.Add(other.gameObject.GetComponent<Entity>());
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (isEnemyProjectile && other.gameObject.tag == "Player")
        {
            enemyList.Remove(other.gameObject.GetComponent<Entity>());
        }
        else if (!isEnemyProjectile && other.gameObject.tag == "Enemy")
        {
            enemyList.Remove(other.gameObject.GetComponent<Entity>());
        }
    }
}
