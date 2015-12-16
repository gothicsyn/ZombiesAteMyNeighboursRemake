// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

#region Includes
using UnityEngine;
//using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
#endregion

public class Entity : NetworkBehaviour
{
	#region Public Variables
	[Header("Movement Settings")]
	public bool canMove = true;
	public float movementSpeed = 1.0f;
	[HideInInspector]
	public bool isSlowed = false;

	[Header("Health Settings")]
	public bool isImmortal = false;
    public float immortalTime = 0.0f;
	public float maxHealth = 100.0f;
	[HideInInspector]
	public float originalMaxHealth = 0.0f;
	public float health = 100.0f;
	[HideInInspector]
	public bool isDead = false;
    [Header("Shield Settings")]
    public bool hasShield = false;
    public float shield = 0.0f;
    public float maxShield = 100.0f;
    [HideInInspector]
    public float baseShield = 0.0f;
   	[Header("Status Settings: Burning")]
	public bool isBurningImmune = false;
	public bool isBurning = false;
	public float burnTimeRemaining = 0.0f;
	public float burnDamage = 0.0f;
	public Color burningColor = Color.red;
	[Header("Status Settings: Frozen")]
	public bool isFrozenImmune = false;
	public bool isFrozen = false;
	public float frozenTimeRemaining = 0.0f;
	public Color frozenColor = Color.blue;
	[Header("Status Settings: Chilled")]
	public bool isChilledImmune = false;
	public bool isChilled = false;
	public Color chilledColor = Color.blue;
	public float chilledSpeedReduction = 0.0f;
	public float chilledTimeRemaining = 0.0f;
	[Header("Status Settings: Poisoned")]
	public bool isPoisonImmune = false;
	public bool isPoisoned = false;
	public float poisonDamage = 0.0f;
	public float poisonedTimeRemaining = 0.0f;
	public Color poisonedColor = Color.green;
    [Header("Debuff Settings: Speed Reduction")]
    public float speedReductionDebuffSpeed = 0.0f;
    public float speedDebuffTimeRemaining = 0.0f;
    public bool isSpeedDebuffed = false;
    [Header("Powerup Settings: Quad Damage Settings")]
    public float quadDamageTimeRemaing = 0.0f;
    public bool hasQuadDamage = false;

    protected List<Color> originalColor;
	[HideInInspector]
	public float originalMovementSpeed = 0.0f;
	protected List<Renderer> renderers;

	[Header("GUI Settings")]
	public Image healthBar;
	public Text healthValue;
    public Image shieldBar;
    public Text shieldValue;
    public GameObject shieldObject;

	[Header("SFX Settings")]
	public List<AudioClip> deathSounds;
	public float deathSoundsVolume;

	[Header("Animation Settings")]
	public bool hasAnimations;
	[HideInInspector]
	public Animator entityAnimator;

	private float lastStatusCheck = 0.0f;
	public bool hasDied = false;

	#endregion

	#region Public Methods
	public void OnEnable()
	{
        baseShield = maxShield;
		DI_Events.EventCenter<Entity, Entity, float, WeaponType>.addListener("OnHit", handleOnHit);
		DI_Events.EventCenter<Entity, Entity, float, float>.addListener("OnChill", handleOnChill);
		DI_Events.EventCenter<Entity, Entity, float, float>.addListener("OnBurn", handleOnBurn);
		DI_Events.EventCenter<Entity, Entity, float>.addListener("OnFreeze", handleOnFreeze);
        DI_Events.EventCenter<Entity, Entity, float, float>.addListener("OnPoison", handleOnPoison);

        //Debuff
        DI_Events.EventCenter<Entity, Item>.addListener("DEBUFF_SPEED", handleSpeedReductionDebuff);

        //Powerup
        DI_Events.EventCenter<Entity, Item>.addListener("POWERUP_INVINCIBILITY", handleImmunePowerup);
        DI_Events.EventCenter<Entity, Item>.addListener("POWERUP_QUAD_DAMAGE", handleQuadDamage);

        health = maxHealth;

		if (!isDead) {
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
			originalMaxHealth = maxHealth;
		}
		else {
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
		}

		originalMovementSpeed = movementSpeed;
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

	public void OnDisable()
	{
		DI_Events.EventCenter<Entity, Entity, float, WeaponType>.removeListener("OnHit", handleOnHit);
		DI_Events.EventCenter<Entity, Entity, float, float>.removeListener("OnChill", handleOnChill);
		DI_Events.EventCenter<Entity, Entity, float, float>.removeListener("OnBurn", handleOnBurn);
		DI_Events.EventCenter<Entity, Entity, float>.removeListener("OnFreeze", handleOnFreeze);
		DI_Events.EventCenter<Entity, Entity, float, float>.removeListener("OnPoison", handleOnPoison);

        //Debuff
        DI_Events.EventCenter<Entity, Item>.removeListener("DEBUFF_SPEED", handleSpeedReductionDebuff);

        //Powerup
        DI_Events.EventCenter<Entity, Item>.removeListener("POWERUP_INVINCIBILITY", handleImmunePowerup);
        DI_Events.EventCenter<Entity, Item>.removeListener("POWERUP_QUAD_DAMAGE", handleQuadDamage);

    }

	public void Update()
	{
		if (GameStateController.instance.gameState == GameStates.PLAYING) {
			if (!isDead) {
				lastStatusCheck += Time.deltaTime;
				if (lastStatusCheck >= 1.0f) {
					lastStatusCheck -= 1.0f;

					if (isBurning) {
						setEffectColor(burningColor);
						burnTimeRemaining -= 1.0f;
						setHealth(health - burnDamage);
						if (burnTimeRemaining <= 0.0f) {
							isBurning = false;
							burnTimeRemaining = 0.0f;
						}
					}

					if (isPoisoned) {
						setEffectColor(poisonedColor);
						poisonedTimeRemaining -= 1.0f;
						setHealth(health - poisonDamage);
						if (poisonedTimeRemaining <= 0.0f) {
							isPoisoned = false;
							poisonedTimeRemaining = 0.0f;
						}
					}

					if (isFrozen) {
						setEffectColor(frozenColor);
						frozenTimeRemaining -= 1.0f;
						if (frozenTimeRemaining <= 0.0f) {
							isFrozen = false;
							canMove = true;
							frozenTimeRemaining = 0.0f;
						}
					}

					if (isChilled) {
						setEffectColor(chilledColor);
						chilledTimeRemaining -= 1.0f;
						if (chilledTimeRemaining <= 0.0f) {
							isChilled = false;
							setSpeed(originalMovementSpeed * (chilledSpeedReduction/100));
							chilledTimeRemaining = 0.0f;
						}
					}
					else {
						setSpeed(originalMovementSpeed);
					}

                    if (isSpeedDebuffed)
                    {
                        //setEffectColor(chilledColor);
                        speedDebuffTimeRemaining -= 1.0f;
                        if (speedDebuffTimeRemaining <= 0.0f)
                        {
                            isSpeedDebuffed = false;
                            setSpeed(originalMovementSpeed * (speedReductionDebuffSpeed / 100));
                            chilledTimeRemaining = 0.0f;
                        }
                    }
                    else
                    {
                        setSpeed(originalMovementSpeed);
                    }

                    if (isImmortal)
                    {
                        immortalTime -= 1.0f;
                        if (immortalTime <= 0.0f)
                        {
                            isImmortal = false;
                            immortalTime = 0.0f;
                        }
                    }

                    if (hasQuadDamage)
                    {
                        quadDamageTimeRemaing -= 1.0f;
                        if (quadDamageTimeRemaing <= 0.0f)
                        {
                            hasQuadDamage = false;
                            quadDamageTimeRemaing = 0.0f;
                        }
                    }

                    if (shieldObject != null)
                    {
                        if (shield <= 0.0f)
                        {
                            hasShield = false;
                            shieldObject.SetActive(false);
                        }
                        else
                        {
                            hasShield = true;
                            shieldObject.SetActive(true);
                        }
                    }

                    if (!isBurning && !isPoisoned && !isFrozen && !isChilled) {
						setEffectColor(originalColor);
					}
				}
			}
		}
	}
	
	/// <summary>
	/// Sets the health.
	/// </summary>
	/// <param name="newHealth">New health.</param>
	public void setHealth(float newHealth)
	{
		health = newHealth;
		if (health > maxHealth) {
			maxHealth = newHealth;
		}
		healthBar.fillAmount = health / maxHealth;
		healthValue.text = (healthBar.fillAmount * 100) + "%";
	}

	/// <summary>
	/// Sets the max health.
	/// </summary>
	/// <param name="newHealth">New health.</param>
	public void setMaxHealth(float newHealth)
	{
		maxHealth = newHealth;
		healthBar.fillAmount = health / maxHealth;
		healthValue.text = (healthBar.fillAmount * 100) + "%";
	}

	/// <summary>
	/// Sets the speed.
	/// </summary>
	/// <param name="newSpeed">New speed.</param>
	public void setSpeed(float newSpeed)
	{
		movementSpeed = newSpeed;
	}

	/// <summary>
	/// Raises the death event.
	/// </summary>
	/// <param name="attacker">Attacker.</param>
	public void OnDeath(Entity attacker)
	{
		isDead = true;
		hasDied = true;
		CancelInvoke();
		StopAllCoroutines();

		if (deathSounds.Count > 0) {
			DI_Events.EventCenter<AudioClip, float, Vector3>.invoke("OnPlayEffectAtPoint", 
			                                                        deathSounds[UnityEngine.Random.Range(0, deathSounds.Count)],
			                                                        deathSoundsVolume,
			                                                        this.transform.position);
		}
		DI_Events.EventCenter<Entity, Entity>.invoke("OnDeath", this, attacker);
		if (hasAnimations) {
			StartCoroutine("playDeathAnimation");
		}
		else {
			if (this.tag == "Tower") {
				Destroy(this.gameObject);
			}
			else {
				gameObject.SetActive(false);
			}
		}
	}

	/// <summary>
	/// Handles the on hit.
	/// </summary>
	/// <param name="target">Target.</param>
	/// <param name="attacker">Attacker.</param>
	/// <param name="damage">Damage.</param>
	public void handleOnHit(Entity target, Entity attacker, float damage, WeaponType weaponType)
	{
		if (target == this) {
			if (!isDead) {
				if (!isImmortal) {
                    if (!hasShield)
                    {
                        damage = handleShieldDamage(damage);

                        health -= damage;
                        if (healthBar != null)
                        {
                            healthBar.fillAmount = health / maxHealth;
                            healthValue.text = (healthBar.fillAmount * 100) + "%";
                            healthBar.transform.parent.gameObject.SetActive(true);
                        }
                        if (health <= 0)
                        {
                            this.OnDeath(attacker);
                        }
                    }
				}
			}
		}
	}

    float handleShieldDamage(float damage)
    {
        if (hasShield)
        {
            float remainingDamage =  damage - shield;
            shield -= damage;
            
            shieldBar.fillAmount = shield / maxShield;
            shieldValue.text = (shieldBar.fillAmount * 100) + "%";
            healthBar.transform.parent.gameObject.SetActive(true);

            if(remainingDamage > 0)
            {
                return remainingDamage;
            }

            shieldObject.SetActive(false);
            return 0.0f;
        }
        else
            return 0.0f;    
    }


    public void handleSpeedReductionDebuff(Entity target, Item item)
    {
        if (target == this)
        {
            if (!isDead)
            {
                if (!isImmortal)
                {
                    //setEffectColor(chilledColor);
                    //Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                    isChilled = true;
                    speedDebuffTimeRemaining = item.duration;
                    speedReductionDebuffSpeed = item.amount;
                    setSpeed(originalMovementSpeed * (speedReductionDebuffSpeed / 100));
                }
            }
        }
    }

    public void handleImmunePowerup(Entity target, Item item)
    {
        if (target == this)
        {
            immortalTime = item.duration;
            isImmortal = true;
            
        }
    }

    public void handleQuadDamage(Entity target, Item item)
    {
        if (target == this)
        {
            quadDamageTimeRemaing = item.duration;
            hasQuadDamage = true;

        }
    }


    public void handleOnChill(Entity target, Entity attacker, float duration,  float speedReduction)
	{
		if (target == this) {
			if (!isDead) {
				if (!isImmortal) {
					if (!isChilledImmune) {
						setEffectColor(chilledColor);
						//Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
						isChilled = true;
						chilledTimeRemaining = duration;
						if (speedReduction > chilledSpeedReduction) {
							chilledSpeedReduction = speedReduction;
						}
						setSpeed(originalMovementSpeed * (chilledSpeedReduction/100));
					}
				}
			}
		}
	}

	public void handleOnBurn(Entity target, Entity attacker, float burnTime, float burningDamage) {
		if (target == this) {
			if (!isDead) {
				if (!isImmortal) {
					if (!isBurningImmune) {
						setEffectColor(burningColor);
						//Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
						isBurning = true;
						burnTimeRemaining = burnTime;
						if (burningDamage > burnDamage) {
							burnDamage = burningDamage;
						}
					}
				}
			}
		}
	}

	public void handleOnFreeze(Entity target, Entity attacker, float freezeTime) {
		if (target == this) {
			if (!isDead) {
				if (!isImmortal) {
					if (!isFrozenImmune) {
						setEffectColor(frozenColor);
						//Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
						isFrozen = true;
						canMove = false;
						if (freezeTime > frozenTimeRemaining) {
							frozenTimeRemaining = freezeTime;
						}
					}
				}
			}
		}
	}

	public void handleOnPoison(Entity target, Entity attacker, float poisonTime, float poisoningDamage) {
		if (target == this) {
			if (!isDead) {
				if (!isImmortal) {
					if (!isPoisonImmune) {
						setEffectColor(poisonedColor);
						//Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
						isPoisoned = true;
						poisonedTimeRemaining = poisonTime;
						if (poisoningDamage > poisonDamage) {
							poisonDamage = poisoningDamage;
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// Plays the death animation.
	/// </summary>
	/// <returns>The death animation.</returns>
	public IEnumerator playDeathAnimation()
	{
		if (hasAnimations) {
			entityAnimator.SetBool("Dead", true);
		}
		yield return new WaitForSeconds(2.0f);
		gameObject.SetActive(false);
	}

	public void setEffectColor(List<Color> effectColors)
	{
		try {
			for (int index = 0; index < renderers.Count; ++index) {
				renderers[index].material.color = effectColors[index];
			}
		}
		catch (Exception) {
		}
	}

	public void setEffectColor(Color effectColor)
	{
		try {
			for (int index = 0; index < renderers.Count; ++index) {
				renderers[index].material.color = effectColor;
			}
		}
		catch (Exception) {
		}
	}
	#endregion
}