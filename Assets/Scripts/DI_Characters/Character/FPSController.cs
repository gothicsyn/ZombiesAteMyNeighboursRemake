// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PhysicsEnabledActor))]
[AddComponentMenu("Character/FPS Controller")]
public class FPSController : MonoBehaviour
{
	private Animator animator;
	private CharacterController controller;
	private AudioSource footStepSource;
	private AudioSource breathSource;

	public bool inControl = true;
	public bool unlimitedStamina = false;
	public bool canJump = true;

	public float verticalSpeed;
	public float horizontalSpeed;
	private Vector3 moveDirection;

	public float jumpSpeed = 5.0f;
	public float walkSpeed = 3.0f;
	public float walkSpeedBackwards = 2.0f;
	public float walkSpeedIncrement = 0.1f;

	public float runSpeed = 5.0f;
	public float runSpeedBackwards = 4.0f;
	public float runSpeedIncrement = 0.3f;

	public float walkSideStepSpeed = 2.75f;
	public float walkSideStepSpeedIncrement = 0.1f;

	public float runSideStepSpeed = 4.75f;
	public float runStepSpeedIncrement = 0.1f;

	public List<AudioClip> walkingFootSteps;
	public List<AudioClip> runningFootSteps;
	public AudioClip sprintSound;
	public AudioClip exhaustedSound;

	private int walkFootStepSequence = 0;
	private int runFootStepSequence = 0;

	// With these settings you can sprint for 5 seconds on, 5 seconds off.
	public float maxStamina = 100.0f;
	public float currentStamina = 100.0f;
	public float staminaDrain = 10.0f;
	public float staminaDrainTick = 0f;
	public float staminaDrainDelay = 0.5f;
	public float staminaRegen = 10.0f;
	private float staminaRegenTick = 0.0f;
	public float staminaRegenDelay = 0.5f;

	public float sprintCoolDownTime = 5.0f;
	private float sprintCoolDownRemaining = 0.0f;

	public bool isSprinting = false;
	public bool isWalking = false;
	public bool isIdle = true;

	public float walkStepDelay = 1.0f;
	public float runStepDelay = 1.0f;
	private float stepAudioDelay = 0.0f;

	public float walkPushPower = 0.0f;
	public float runPushPower = 0.0f;
	
	public PhysicsEnabledActor actor;

	public bool isGrounded = true;
	public float groundFudge = 0.07f;
	public float gravity = 0.0f;
	public float distanceFromGround = 0.0f;

	//public bool isSieged = false;
	private float originalWalkSpeed = 0.0f;
	private float originalRunSpeed = 0.0f;

	public void OnEnable()
	{
		if (GameStateController.instance.gameState != GameStates.PLAYING) {
			disablePlayerControl();
		}
		controller = this.GetComponent<CharacterController>();
		animator = this.GetComponent<Animator>();
		footStepSource = this.GetComponents<AudioSource>()[0];
		breathSource = this.GetComponents<AudioSource>()[1];
		actor = this.GetComponent<PhysicsEnabledActor>();
		animator.applyRootMotion = false;
		startIdle();
		originalRunSpeed = runSpeed;
		originalWalkSpeed = walkSpeed;
	}

	public void setWalkSpeed(float speed)
	{
		walkSpeed = speed;
		walkSideStepSpeed = speed - 0.5f;
		walkSpeedBackwards = speed - 0.5f;
		walkSpeedIncrement = speed;
	}

	public float getWalkSpeed()
	{
		return originalWalkSpeed;
	}

	public void setRunSpeed(float speed)
	{
		runSpeed = speed;
		runSideStepSpeed = speed - 1.0f;
		runSpeedIncrement = runSpeed - 1;
		runSpeedBackwards = runSpeed - 1;
	}

	public float getRunSpeed()
	{
		return originalRunSpeed;
	}

	private void startSprint()
	{
		isWalking = false;
		isSprinting = true;
		isIdle = false;

		if (animator.enabled) {
			animator.SetFloat("Speed", 5);
		}
		if (!unlimitedStamina) {
			sprintCoolDownRemaining = sprintCoolDownTime;

			if (!breathSource.isPlaying) {
				breathSource.loop = true;
				breathSource.clip = sprintSound;
				breathSource.Play();
			}
		}
		actor.pushPower = runPushPower;
	}

	// Callted when the player runs out of stamina.
	private void endSprint()
	{
		if (!unlimitedStamina) {
			if (breathSource.isPlaying) {
				breathSource.Stop();
			}
			breathSource.loop = false;
			breathSource.clip = exhaustedSound;
			breathSource.Play();
		}
		startWalk();
	}

	private void startWalk()
	{
		// We should be playing the exhausted clip at 0 stam and its a one shot.
		if (!unlimitedStamina) {
			if (currentStamina > 0) {
				breathSource.loop = false;
				breathSource.Stop();
			}
		}

		isWalking = true;
		isSprinting = false;
		isIdle = false;

		if (animator.enabled) {
			animator.SetFloat("Speed", 3);
		}
	}

	private void startIdle()
	{
		actor.pushPower = walkPushPower;
		isWalking = false;
		isSprinting = false;
		isIdle = true;

		if (animator.enabled) {
			animator.SetFloat("Speed", 0);
		}
	}

	private void playFootSteps(float timeDelta)
	{
		if (isSprinting) {
			if (stepAudioDelay >= runStepDelay) {
				footStepSource.clip = runningFootSteps[runFootStepSequence];
				++runFootStepSequence;
				if (runFootStepSequence >= runningFootSteps.Count - 1) {
					runFootStepSequence = 0;
				}
				footStepSource.PlayOneShot(footStepSource.clip);
				stepAudioDelay = 0.0f;
			}
		}
		else if (isWalking) {
			if (stepAudioDelay >= walkStepDelay) {
				footStepSource.clip = walkingFootSteps[walkFootStepSequence];
				++walkFootStepSequence;
				if (walkFootStepSequence >= walkingFootSteps.Count - 1) {
					walkFootStepSequence = 0;
				}
				footStepSource.PlayOneShot(footStepSource.clip);
				stepAudioDelay = 0.0f;
			}
		}
		stepAudioDelay = Mathf.Clamp(stepAudioDelay + timeDelta, 0, walkStepDelay);
	}

	public void disablePlayerControl()
	{
		inControl = false;
		Camera.main.gameObject.GetComponent<MouseLook>().enabled = false;
		GetComponent<MouseLook>().enabled = false;
	}

	public void enablePlayerControl()
	{
		inControl = true;
		Camera.main.gameObject.GetComponent<MouseLook>().enabled = true;
		GetComponent<MouseLook>().enabled = true;
	}

	public void SiegeModeEnable()
	{
		walkSpeed = 0f;
		walkSpeedBackwards = 0f;
		walkSideStepSpeed = 0f;
		//isSieged = true;
	}

	public void SiegeModeDisable()
	{
		walkSpeed = 3.0f;
		walkSpeedBackwards = 2.0f;
		walkSideStepSpeed = 2.75f;
		//isSieged = false;
	}


	
	public void FixedUpdate()
	{
		RaycastHit hit;
		Ray ray = new Ray(transform.position, Vector3.down);
		Debug.DrawRay(transform.position, Vector3.down, Color.red);
		if (Physics.Raycast(ray, out hit)) {
			distanceFromGround = hit.distance;
			if (hit.distance > groundFudge) {
				isGrounded = false;
			}
			else {
				isGrounded = true;
			}
		}
	}

	public void LateUpdate()
	{
		if (inControl) {
			if (GameStateController.instance.gameState != GameStates.PLAYING) {
				disablePlayerControl();
				startIdle();
			}

			// Cache the input calls
			float vertical = Input.GetAxis("Vertical");
			float horizontal = Input.GetAxis("Horizontal");
			float shiftKey = Input.GetAxis("Sprint");

			// Cache the time delta
			float timeDelta = Time.deltaTime;

			if (isGrounded) {
				if (vertical != 0 || horizontal != 0) {
					if (shiftKey > 0) {
						if (currentStamina > 0 && sprintCoolDownRemaining == 0.0f) {
							if (!isSprinting) {
								startSprint();
							}
						}
					}
					else {
						if (isSprinting || !isWalking) {
							startWalk();
						}
					}

					if (isSprinting) {
//						verticalSpeed = Mathf.Clamp(vertical * runSpeedIncrement * timeDelta, -1 * runSpeedBackwards, runSpeed);
//						horizontalSpeed = Mathf.Clamp(horizontal * runStepSpeedIncrement * timeDelta, -1 * runSideStepSpeed, runSideStepSpeed);
						verticalSpeed = Mathf.Clamp(vertical * runSpeedIncrement, -1 * runSpeedBackwards, runSpeed);
						horizontalSpeed = Mathf.Clamp(horizontal * runStepSpeedIncrement, -1 * runSideStepSpeed, runSideStepSpeed);
					}
					else {
//						verticalSpeed = Mathf.Clamp(vertical * walkSpeedIncrement * timeDelta, -1 * walkSpeedBackwards, walkSpeed);
//						horizontalSpeed = Mathf.Clamp(horizontal * walkSideStepSpeedIncrement * timeDelta, -1 * walkSideStepSpeed, walkSideStepSpeed);
						verticalSpeed = Mathf.Clamp(vertical * walkSpeedIncrement, -1 * walkSpeedBackwards, walkSpeed);
						horizontalSpeed = Mathf.Clamp(horizontal * walkSideStepSpeedIncrement, -1 * walkSideStepSpeed, walkSideStepSpeed);
					}
				}

				// Burn Stamina
				if (!unlimitedStamina) {
					if (isSprinting) {
						staminaDrainTick += timeDelta;
						if (staminaDrainTick >= staminaDrainDelay) {
							currentStamina = Mathf.Clamp(currentStamina - staminaDrain, 0, maxStamina);
							if (currentStamina == 0) {
								endSprint();
							}
						}
					}
					// Regain Stamina
					if (!isSprinting) {
						staminaRegenTick += timeDelta;
						if (staminaRegenTick >= staminaRegenDelay) {
							currentStamina = Mathf.Clamp(currentStamina + staminaRegen, 0, maxStamina);
							staminaRegenTick = 0.0f;
							sprintCoolDownRemaining = Mathf.Clamp(sprintCoolDownRemaining - staminaRegenDelay, 0, sprintCoolDownTime);
						}
					}
				}

				if (vertical == 0 && horizontal == 0) {
					verticalSpeed = 0;
					horizontalSpeed = 0;
					startIdle();
				}

				Vector3 directionVector = new Vector3(horizontalSpeed, 0, verticalSpeed);
				moveDirection = transform.rotation * directionVector;
				if (canJump) {
					if (Input.GetButtonDown("Jump")) {
						moveDirection.y = jumpSpeed;
					}
				}
				playFootSteps(timeDelta);
			}
			moveDirection.y -= gravity * timeDelta;
			controller.Move(moveDirection * timeDelta);
		}
		if (!inControl) {
			if (GameStateController.instance.gameState == GameStates.PLAYING) {
				enablePlayerControl();
			}
		}
	}
}
