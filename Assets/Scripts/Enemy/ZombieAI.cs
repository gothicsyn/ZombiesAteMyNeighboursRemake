using UnityEngine;
using System.Collections;

public class ZombieAI : MonoBehaviour {

	public float fpsTargetDistance;
	public float enemyLookDistance;
	public float attackDistance;
	public float enemyMovementSpeed;
	public float damping;
	public Transform playerTarget;
	Rigidbody theRigidbody;
	Renderer myRender;

	// Use this for initialization
	void Start () {
		myRender = GetComponent<Renderer>();
		theRigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		fpsTargetDistance = Vector3.Distance(playerTarget.position, transform.position);
		if(fpsTargetDistance < enemyLookDistance){
			lookAtPlayer();
		}

		if(fpsTargetDistance < attackDistance){
			attackPlayer();
		}
	}

	public void lookAtPlayer(){
		Quaternion rotation =Quaternion.LookRotation(playerTarget.position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
	}

	public void attackPlayer(){
		theRigidbody.AddForce(transform.forward * enemyMovementSpeed);
	}
}
