// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using System;

[RequireComponent(typeof(NavMeshAgent))]
[AddComponentMenu("AI/Waypoint AI/Waypoint AI Agent")]
public class WaypointAI : MonoBehaviour
{
	[Header("Agent Settings")]
	[HideInInspector]
	public NavMeshAgent agent;
	public bool isEnemy = true;
	public bool agentActive = true;

	[Header("Waypoint Settings")]
	public AIWaypoint target;
	public bool reachedDestination = false;
	public bool waypointIsStar = false;
	public float distanceToTarget = 0.0f;

	[Header("Animation Settings")]
	public bool hasAnimations = false;
	
	[HideInInspector]
	public Animator animator;
	[HideInInspector]
	public GameStateController gsc;
	private bool isEntityDead = false;

	public void OnEnable()
	{
		if (!isEntityDead) {
			agent = GetComponent<NavMeshAgent>();
			gsc = GameObject.Find("Game State").GetComponent<GameStateController>();
			if (hasAnimations) {
				animator = GetComponentInChildren<Animator>();
			}
		}
		else {
			isEntityDead = false;
			reachedDestination = false;
			waypointIsStar = false;

			if (hasAnimations) {
				animator.SetBool("Dead", false);
				animator.SetFloat("Speed", 0.0f);
				animator.SetBool("Attacking", false);
				animator.Play("Idle");
			}
		}

		agentActive = true;
	}
	
	public void Update()
	{
		if (!isEntityDead) {
			if (gsc.gameState == GameStates.PLAYING) {
				if (target != null) {
					distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
					agent.SetDestination(target.transform.position);
					StartRun();

					if (distanceToTarget <= agent.stoppingDistance) {
						reachedDestination = true;
						agent.Stop();
						StartIdle();
					}
				}
				else {
					stopAgent();
					StartIdle();
				}
			}
			else {
				agent.enabled = false;
				StartIdle();
			}
		}
	}
	
	public void LateUpdate()
	{
		if (reachedDestination == true) {
			// Don't look up at the star look forward towards it.
			transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
			transform.localRotation.Set(0, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
		}
		else {
			//agent.speed = enemyData.movementSpeed;
		}
	}

	public void stopAgent()
	{
		if (agentActive) {
			try {
				agent.Stop();
			}
			catch (Exception) {
			}
			agentActive = false;
		}
	}

	public void startAgent()
	{
		if (!agentActive) {
			agent.Resume();
			agentActive = true;
		}
	}

	public void OnDrawGizmos()
	{
		if (target != null) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(transform.position, target.transform.position);
			Gizmos.DrawCube(target.transform.position, new Vector3 (0.2f, 1.0f, 0.2f));
		}
	}

	public void StartIdle()
	{
		if (hasAnimations) {
			animator.SetFloat("Speed", 0.0f);
			animator.SetBool("Attacking", false);
		}
	}

	public void StartRun()
	{
		if (hasAnimations) {
			animator.SetFloat("Speed", 1.0f);
			animator.SetBool("Attacking", false);
		}
	}

	public void StartAttack()
	{
		if (hasAnimations) {
			animator.SetFloat("Speed", 0.0f);
			animator.SetBool("Attacking", true);
		}
	}
}
