// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Character/Shoot Em Up Controller")]

public class ShootEmUpController : NetworkBehaviour
{
	public bool usingController = false;

	protected Animator animator;
	protected CharacterController characterController;
	[Header("Control Settings")]
	public bool inControl = true;
	public string vericalAxisName = "Vertical";
	public string horizontalAxisName = "Horizontal";
	public float verticalSpeed;
	public float horizontalSpeed;
	public Vector3 moveDirection;
	protected float currentSpeed = 0.0f;
    public bool isInverted = false;
    public float invertedTimeLeft = 0.0f;

	[Header("Camera Settings")]
	public GameObject followingCamera;

	[Header("Speed Settings")]
	public float maxSpeed = 10.0f;

	[Header("Thruster Settings")]
	public float movementSpeed = 15.0f;
	public float turningSpeed = 15.0f;
	public GameObject leftRearThruster;
	public GameObject rightRearThruster;
	public GameObject mainRearThruster;
	public GameObject leftFrontThruster;
	public GameObject rightFrontThruster;
	public GameObject mainFrontThruster;
	public bool reachedSpeedLimit = false;

	[Header("Fuel Settings")]
	public bool unlimitedFuel = false;
	public float fuel = 0.0f;

	[Header("Debug Velocity")]
	// Store the velocity of the ship so we can reset it after the game resumes.
	public float speed;

	public void OnEnable()
	{
		if (GameStateController.instance.gameState != GameStates.PLAYING) {
			disablePlayerControl();
		}

		characterController = this.GetComponent<CharacterController>();
		animator = this.GetComponent<Animator>();
        DI_Events.EventCenter<Entity, Item>.addListener("DEBUFF_BACKWARDS_CONTROLS", handleBackwardsDebuff);
	}

	public void disablePlayerControl()
	{
		inControl = false;
        DI_Events.EventCenter<Entity, Item>.removeListener("DEBUFF_BACKWARDS_CONTROLS", handleBackwardsDebuff);
		disableThrusters();
	}
	
	public void enablePlayerControl()
	{
		inControl = true;
	}

	public void disableThrusters()
	{
		leftRearThruster.SetActive(false);
		rightRearThruster.SetActive(false);
		mainRearThruster.SetActive(false);
		leftFrontThruster.SetActive(false);
		rightFrontThruster.SetActive(false);
		mainFrontThruster.SetActive(false);
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.name == "Outer Edge") {
			wrapWorld();
		}
	}

	public void applyForce()
	{
	}

    public void handleBackwardsDebuff(Entity target, Item item)
    {
        if (target = GetComponent<Entity>())
        {
            invertedTimeLeft = item.duration;
            isInverted = true;
        }
    }

	// TODO move this to a server only script
	public void wrapWorld()
	{
		Vector3 newPosition = new Vector3(0,0,0);

		if (transform.position.x > 0) {
			newPosition.x = Mathf.Clamp(-transform.position.x + 25.0f, -475f, 475f);
		}
		else {
			newPosition.x = Mathf.Clamp(-transform.position.x - 25.0f, -475f, 475f);
		}

		if (transform.position.z > 0) {
			newPosition.z = Mathf.Clamp(-transform.position.z + 25.0f, -475f, 475f);
		}
		else {
			newPosition.z = Mathf.Clamp(-transform.position.z - 25.0f, -475f, 475f);
		}

		transform.position = newPosition;
	}

	public Vector3 getLookAtRotation ()
	{
		// Find the center of the screen.
		Vector3 screen = new Vector3(Screen.width * 0.5f, 0, Screen.height * 0.5f);
		Vector3 mousePosition = Input.mousePosition;
		// Swap the Y and Z values of the mouse position so it lines up with the screen vector.
		mousePosition.z = mousePosition.y;
		mousePosition.y = 0;

		// Find the rotation to aim at based on the center of the screen.
		return (mousePosition - screen);
	}

	public void LateUpdate()
	{
		// Only run this on the local player
		Debug.Log("isLocalPlayer:" + isLocalPlayer);
		Debug.Log("isServer:" + isServer);
		
		if (!isLocalPlayer) {
			this.enabled = false;
		}

		if (inControl) {
			if (GameStateController.instance.gameState != GameStates.PLAYING) {
				disablePlayerControl();
			}
			else {

                //Inverted Debuff
                if(isInverted)
                {
                    if(invertedTimeLeft <= 0.0f)
                    {
                        isInverted = false;
                    }
                    else
                    {
                        invertedTimeLeft -= Time.deltaTime;
                    }
                }

				followingCamera.transform.position = new Vector3(transform.position.x, 100.0f, transform.position.z);

				if (transform.position.x > 500 || transform.position.x < -500 || transform.position.z > 500 || transform.position.z < -500) {
					wrapWorld();
				}

                float vertical;
                float horizontal;

                // Cache the input calls
                if (isInverted)
                {
                    vertical = -1 * Input.GetAxis(vericalAxisName);
                    horizontal = Input.GetAxis(horizontalAxisName);
                }
                else
                {
                    vertical = Input.GetAxis(vericalAxisName);
                    horizontal = -1 * Input.GetAxis(horizontalAxisName);
                }

				// Cache the time delta
				float timeDelta = Time.deltaTime;
				
				if (vertical != 0 || horizontal != 0) {
					verticalSpeed = Mathf.Clamp(vertical * movementSpeed, -1 * maxSpeed, maxSpeed);
					horizontalSpeed = Mathf.Clamp(horizontal * movementSpeed, -1 * maxSpeed, maxSpeed);
				}
				
				if (vertical == 0 && horizontal == 0) {
					verticalSpeed = 0;
					horizontalSpeed = 0;
				}

				if (vertical == 0 && horizontal == 0) {
					disableThrusters();
				}
				else if (vertical == 0 && horizontal > 0) {
					disableThrusters();
					rightRearThruster.SetActive(true);
					//leftFrontThruster.SetActive(true);
				}
				else if (vertical == 0 && horizontal < 0) {
					disableThrusters();
					leftRearThruster.SetActive(true);
					//rightFrontThruster.SetActive(true);
				}
				else if (vertical < 0 && horizontal == 0) {
					disableThrusters();
					mainFrontThruster.SetActive(true);
				}
				else if (vertical < 0 && horizontal > 0) {
					disableThrusters();
					mainFrontThruster.SetActive(true);
					rightFrontThruster.SetActive(true);
				}
				else if (vertical < 0 && horizontal < 0) {
					disableThrusters();
					mainFrontThruster.SetActive(true);
					leftFrontThruster.SetActive(true);
				}
				else if (vertical > 0 && horizontal == 0) {
					disableThrusters();
					mainRearThruster.SetActive(true);
				}
				else if (vertical > 0 && horizontal > 0) {
					disableThrusters();
					mainRearThruster.SetActive(true);
					rightRearThruster.SetActive(true);
				}
				else if (vertical > 0 && horizontal < 0) {
					disableThrusters();
					mainRearThruster.SetActive(true);
					leftRearThruster.SetActive(true);
				}

				Vector3 directionVector = new Vector3(verticalSpeed, 0, horizontalSpeed);
				moveDirection = transform.rotation * directionVector;
				moveDirection.y = -1;
				characterController.Move(moveDirection * timeDelta);


				if (!usingController) {
					transform.rotation = Quaternion.LookRotation(getLookAtRotation());
//					float turn = Input.GetAxis(turningAxisName);
//					float turnAmount = turn * turningSpeed;
//					float currentAngle = transform.rotation.eulerAngles.y;
//					transform.rotation = Quaternion.AngleAxis(currentAngle + (Time.deltaTime * turnAmount), Vector3.up);
				}
				else {
					if (horizontal != 0.0 || vertical != 0.0) {
						float angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;
						transform.rotation = Quaternion.AngleAxis(90.0f - angle, Vector3.up);
					}
				}
			}
		}

		if (!inControl) {
			if (GameStateController.instance.gameState == GameStates.PLAYING) {
				enablePlayerControl();
			}
		}
	}
}