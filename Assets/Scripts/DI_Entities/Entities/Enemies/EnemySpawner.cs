// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(DI_Game.EditorOnlyObject))]
[AddComponentMenu("Enemies/Spawner")]
public class EnemySpawner : MonoBehaviour
{
	public SpawnPoints spawnName;
	public AIWaypoint nextWaypoint;
	public Color pathColor = Color.green;
	public List<Enemy> spawnedEnemies;
	public int enemyCount = 0;
	public WaveController waveController;
	public List<PlayerState> playerStates;
	public GameObject colorDisplayPanel;
	public bool spawnerActive = false;
	public AudioClip openSound;
	public float openVolume = 0.25f;

	public void OnEnable()
	{
		DI_Events.EventCenter<SpawnPoints>.addListener("OnSpawnerActivate", handleOnSpawnerActivate);
		DI_Events.EventCenter<SpawnPoints, GameObject>.addListener("OnRequestEnemySpawn", handleOnSpawn);
		playerStates = new List<PlayerState>();
		//TODO add player two
		playerStates.Add(GameObject.Find("Player One").GetComponent<PlayerState>());
		colorDisplayPanel.SetActive(spawnerActive);
	}

	public void OnDisable()
	{
		DI_Events.EventCenter<SpawnPoints, GameObject>.removeListener("OnRequestEnemySpawn", handleOnSpawn);
		DI_Events.EventCenter<SpawnPoints>.removeListener("OnSpawnerActivate", handleOnSpawnerActivate);
	}

	public void handleOnSpawnerActivate(SpawnPoints _spawnPoint)
	{
		if (_spawnPoint == this.spawnName) {
			if (spawnerActive == false) {
				spawnerActive = true;
				colorDisplayPanel.SetActive(spawnerActive);
				DI_Events.EventCenter<AudioClip, float>.invoke("OnPlayEffect", openSound, openVolume);
			}
		}
	}

	public void OnDrawGizmos()
	{
		if (nextWaypoint != null) {
			Gizmos.color = pathColor;
			Gizmos.DrawLine(transform.position, nextWaypoint.transform.position);
		}
	}

	public void OnDrawGizmosSelected()
	{
		if (nextWaypoint != null) {
			Gizmos.DrawIcon(nextWaypoint.transform.position, "Waypoint AI/Waypoint_Target.png", true);
		}
	}

	public int getEnemyCount()
	{
		enemyCount = spawnedEnemies.Count;
		return spawnedEnemies.Count;
	}

	public void handleOnSpawn(SpawnPoints _spawnPoint, GameObject _enemy)
	{
		if (_spawnPoint == spawnName) {
			if (!spawnerActive) {
				spawnerActive = true;
				colorDisplayPanel.SetActive(spawnerActive);
				DI_Events.EventCenter<string, string, float>.invoke("OnDisplayNotification", "New Portal Opened", "The " + spawnName.ToString() + " portal has opened!" , 5.0f);
			}
			#if DEBUG
				Debug.Log("on spawn");
			#endif
			spawnedEnemies.Add(_enemy.GetComponent<Enemy>());
			WaypointAI enemyAI = _enemy.GetComponent<WaypointAI>();
			_enemy.transform.position = this.transform.position;
			enemyAI.target = nextWaypoint;
			enemyAI.agent.enabled = true;
			DI_Events.EventCenter<GameObject>.invoke("OnSpawn", _enemy);
		}
	}
}