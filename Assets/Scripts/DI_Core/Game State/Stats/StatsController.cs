// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//
using UnityEngine;
using System.Collections.Generic;
using System;
//using MBS;

[AddComponentMenu("Game State/Stats Controller")]
public class StatsController : MonoBehaviour
{
	public int waveTimersSkipped = 0;
	public int highestWave = 0;
	public Dictionary<Items, int> itemsPickedUp;
	public Dictionary<WeaponType, int> shotsFired;
	public Dictionary<WeaponType, int> shotsLanded;
	public Dictionary<Enemies, int> enemiesKilled;
	public Dictionary<Enemies, float> damageToEnemies;
	public string nextLevel;
	public bool isTutorial = false;
	public static StatsController instance;

	public void saveStats()
	{
			ScoreController scoreController = GameObject.Find("Game State").GetComponent<ScoreController>();
//			HighScoreController highscoreController = GameObject.Find("Game State").GetComponent<HighScoreController>();
			//highscoreController.CallPostScore(WULogin.UID, WULogin.display_name,
				//(int)scoreController.getFinalScore(GameObject.Find("Game State").GetComponent<GameStateController>().playerState.player), (int)waveController.currentWave);

			#region Basic Stats
			// Added Type Stats
			saveAddStat("Wave Timers Skipped", waveTimersSkipped);
			// Max Type Stats
			saveMaxStat("Highest Wave", highestWave);
			saveMaxStat (Application.loadedLevelName + "|" + PlayerPrefs.GetInt("Difficulty", 0) + "|" + "Solo", (int)scoreController.getFinalScore(GameObject.Find("Game State").GetComponent<GameStateController>().playerState.player));

			#endregion

			#region Advanced Stats
			try {
				// Items Picked Up
				foreach (KeyValuePair<Items, int> data in itemsPickedUp) {
					saveAddStat("Items Picked Up: " + data.Key.ToString(), data.Value);
				}

				// Shots Fired
				foreach (KeyValuePair<WeaponType, int> data in shotsFired) {
					saveAddStat("Shots Fired: " + data.Key.ToString(), data.Value);
				}

				// Shots Landed
				foreach (KeyValuePair<WeaponType, int> data in shotsLanded) {
					saveAddStat("Shots Landed: " + data.Key.ToString(), data.Value);
				}

				// Enemies Killed
				foreach (KeyValuePair<Enemies, int> data in enemiesKilled) {
					saveAddStat("Enemies Killed: " + data.Key.ToString(), data.Value);
				}

				// Damage to enemies
				foreach (KeyValuePair<Enemies, float> data in damageToEnemies) {
					saveAddStat("Damage To Enemies: " + data.Key.ToString(), (int)data.Value);
				}
			}
			catch (Exception err) {
				Debug.LogException(err);
			}
			#endregion
	}
	
	public void OnEnable()
	{
		instance = this;
		DI_Events.EventCenter<int>.addListener("OnWaveEnd", handleOnWaveEnd);
		DI_Events.EventCenter<Item, PlayerState>.addListener("OnPickupItem", handlePickupItem);
		DI_Events.EventCenter<Entity, Entity>.addListener("OnDeath", handleKillEnemy);
		DI_Events.EventCenter<WeaponType, PlayerState>.addListener("OnFire", handleOnFire);
		DI_Events.EventCenter<float, Enemies, WeaponType>.addListener("OnDamage", handleOnDamage);
		DI_Events.EventCenter.addListener("OnVictory", handleVictory);
		DI_Events.EventCenter<string>.addListener("OnSetNextLevel", handleSetNextLevel);
		DI_Events.EventCenter.addListener("OnSkipWaveTimer", handleSkipWaveTimer);

		itemsPickedUp = new Dictionary<Items, int>();
		shotsFired = new Dictionary<WeaponType, int>();
		shotsLanded = new Dictionary<WeaponType, int>();
		enemiesKilled = new Dictionary<Enemies, int>();
		damageToEnemies = new Dictionary<Enemies, float>();
	}

	public void OnDisable()
	{
		DI_Events.EventCenter<int>.removeListener("OnWaveEnd", handleOnWaveEnd);
		DI_Events.EventCenter<Item, PlayerState>.removeListener("OnPickupItem", handlePickupItem);
		DI_Events.EventCenter<Entity, Entity>.removeListener("OnDeath", handleKillEnemy);
		DI_Events.EventCenter<WeaponType, PlayerState>.removeListener("OnFire", handleOnFire);
		DI_Events.EventCenter<float, Enemies, WeaponType>.removeListener("OnDamage", handleOnDamage);
		DI_Events.EventCenter.removeListener("OnVictory", handleVictory);
		DI_Events.EventCenter<string>.removeListener("OnSetNextLevel", handleSetNextLevel);
		DI_Events.EventCenter.removeListener("OnSkipWaveTimer", handleSkipWaveTimer);
	}

	public void saveAddStat(string prefName, int currentValue)
	{
		PlayerPrefs.SetInt(prefName, PlayerPrefs.GetInt(prefName, 0) + currentValue);
		Debug.Log("Saving Stat: " + prefName + " value: " + currentValue);
	}

	public void saveMaxStat(string prefName, int currentValue)
	{
		int oldStat = PlayerPrefs.GetInt(prefName, 0);
		if (oldStat < currentValue) {
			PlayerPrefs.SetInt(prefName, currentValue);
			Debug.Log("Saving Stat: " + prefName + " value: " + currentValue);
		}
	}

	public void handleSetNextLevel(string level)
	{
		nextLevel = level;
	}

	public void handleVictory()
	{
		//TODO save all the stats before loading the victory level.
		#if DEBUG
		Debug.Log("OnVictory");
		#endif
		if (!isTutorial) {
			saveStats();
		}
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		Application.LoadLevel(nextLevel);
	}

	public void handleOnWaveEnd(int wave)
	{
		#if DEBUG
		Debug.Log("OnWaveEnd");
		#endif
		highestWave = wave;
	}

	public void handlePickupItem(Item item, PlayerState playerState)
	{
		#if DEBUG
		Debug.Log("OnPickupItem");
		#endif
//		if (itemsPickedUp.ContainsKey(ammoType)) {
//			itemsBought[ammoType] += 1;
//		}
//		else {
//			itemsBought.Add(ammoType, 1);
//		}
	}

	public void handleKillEnemy(Entity target, Entity attacker)
	{
		#if DEBUG
		Debug.Log("OnKillEnemy");
		#endif
		// Check the tag if its not enemy then a tower has died.
		if (target.tag == "Enemy") {
			Enemy enemy = target.gameObject.GetComponent<Enemy>();
			if (enemiesKilled.ContainsKey(enemy.type)) {
				enemiesKilled[enemy.type] += 1;
			}
			else {
				enemiesKilled.Add(enemy.type, 1);
			}
		}
	}

	public void handleOnFire(WeaponType weaponType, PlayerState playerState)
	{
		#if DEBUG
		Debug.Log("OnFire");
		#endif
		if (shotsFired.ContainsKey(weaponType)) {
			shotsFired[weaponType] += 1;
		}
		else {
			shotsFired.Add(weaponType, 1);
		}
	}

	public void handleOnDamage(float damage, Enemies victim, WeaponType weaponType)
	{
		#if DEBUG
		Debug.Log("OnDamage");
		#endif
		if (shotsLanded.ContainsKey(weaponType)) {
			shotsLanded[weaponType] += 1;
		}
		else {
			shotsLanded.Add(weaponType, 1);
		}

		if (damageToEnemies.ContainsKey(victim)) {
			damageToEnemies[victim] += damage;
		}
		else {
			damageToEnemies.Add(victim, damage);
		}
	}

	public void handleSkipWaveTimer()
	{
		#if DEBUG
		Debug.Log("OnSkipWaveTimer");
		#endif
		waveTimersSkipped += 1;
	}
}