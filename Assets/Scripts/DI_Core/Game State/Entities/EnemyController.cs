// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class EnemyController : NetworkBehaviour
{
	public List<Enemy> spawnedEnemies;
	public List<PlayerState> playerStates;
	public int enemiesKilledInWave = 0;

	public void OnEnable()
	{
		// Only run this on the server
		if (!isServer) {
			this.enabled = false;
		}
		DI_Events.EventCenter<Entity, Entity>.addListener("OnDeath", handleOnDeath);
		DI_Events.EventCenter<GameObject>.addListener("OnSpawn", handleOnSpawn);
		DI_Events.EventCenter<int>.addListener("OnWaveEnd", handleWaveEnd);
		spawnedEnemies = new List<Enemy>();
		playerStates = new List<PlayerState>();
		//TODO add player two
		playerStates.Add(GameObject.Find("Player One").GetComponent<PlayerState>());
	}

	public void OnDisable()
	{
		DI_Events.EventCenter<Entity, Entity>.removeListener("OnDeath", handleOnDeath);
		DI_Events.EventCenter<GameObject>.removeListener("OnSpawn", handleOnSpawn);
		DI_Events.EventCenter<int>.removeListener("OnWaveEnd", handleWaveEnd);
	}

	public void handleWaveEnd(int wave)
	{
		enemiesKilledInWave = 0;
		foreach (PlayerState playerState in playerStates) {
			DI_Events.EventCenter<PlayerState>.invoke("OnUpdateHudRequest", playerState);
		}
	}

	public void handleOnDeath(Entity _enemy, Entity _player)
	{
		// Check the tag to make sure its an enemy, otherwise its a tower dying due to a ramboman
		if (_enemy.tag == "Enemy") {
			if (spawnedEnemies.Contains((Enemy) _enemy)) {
				spawnedEnemies.Remove((Enemy)_enemy);
				#if DEBUG
				Debug.Log("On Death");
				#endif
			}
			++enemiesKilledInWave;
		}
	}

	public void handleOnSpawn(GameObject _enemy)
	{
		spawnedEnemies.Add(_enemy.GetComponent<Enemy>());
		foreach (PlayerState playerState in playerStates) {
			DI_Events.EventCenter<PlayerState>.invoke("OnUpdateHudRequest", playerState);
		}
	}
}
