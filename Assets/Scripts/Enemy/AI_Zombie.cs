using UnityEngine;
using System.Collections;

public class AI_Zombie : MonoBehaviour {

	public UnityEngine.AI.NavMeshAgent agent;
	public Transform target;

	void Awake () {
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	// Use this for initialization
	void Start () {
		agent.SetDestination (target.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
