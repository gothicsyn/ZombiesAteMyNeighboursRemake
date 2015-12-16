// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2013, 2014
//
// TODO: Include a description of the file here.
//

#region Includes
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
#endregion

[AddComponentMenu("Game State/Wave Controller")]
public class WaveController : MonoBehaviour
{
	#region Public Variables
	[Header("Enemy Settings")]
	// How many enemies can be spawned at a time
	public int maxEnemies = 30;
	// How long do we delay between spawns.
	public float spawnDelay = 0.5f;
	// Is a wave currently spawning
	//[HideInInspector]
	public bool spawnInProgress = false;
	
	[Header("Wave Settings")]
	// What wave are we currently on
	public float currentWave = 0;
	// List containing the wave data.
	public List<Wave> waves;
	// List of the spawn points on the map
	public List<EnemySpawner> spawnPoints;	
	// Delay between wave start and the first spawn
	public float waveDelay = 30.0f;
	public float waveDelayTimer = 0.0f;
	public float spawnDelayTimer = 0.0f;
	public bool gameOver = false;

	[Header("Endless Mode Settings")]
	//Seed for the sudo random mode
	public int seed;
	// Endless Mode
	public bool endlessMode = false;
	// Sudo Random Endless Mode
	public bool sudoRandom = true;
	public EnemySpawnChances weights;
	// What order the portals open in.
	public List<SpawnPoints> spawnOrder;

	[Header("GUI Settings")]
	public Text nextWaveInText;

	[Header("Game Settings")]
	// How many waves are in this level.
	public int maxWave = 15;
	public bool waitForEventToStart = false;
	public string nextLevel;
	public bool sendVictoryEventToDialog = false;
	public string dialogName;

	[HideInInspector]
	public PlayerState playerState;
	[HideInInspector]
	public GameStateController gsc;
	[HideInInspector]
	public PrefabLoader prefabLoader;
	[HideInInspector]
	public EnemyController enemyController;

	[HideInInspector]
	public int enemiesInWave = 0;
	[HideInInspector]
	public int enemiesSpawned = 0;
	[HideInInspector]
	public float waveMultiplier;

	private Queue<EnemySpawnData> spawnQueue;
	[HideInInspector]
	public static WaveController instance;

	#endregion

	#region Public Methods
	public void OnEnable()
	{
		instance = this;
		// Pre-Populate the dictionary
		spawnQueue = new Queue<EnemySpawnData>();

		enemyController = GameObject.Find("Game State").GetComponent<EnemyController>();
		playerState = GameStateController.instance.playerState;

		if (endlessMode && sudoRandom) {
			// This should make the waves always the same if sudo random is set.
			UnityEngine.Random.seed = seed;
		}

		maxEnemies = (int)PlayerPrefs.GetFloat("Enemies On Screen", 25.0f);
		DI_Events.EventCenter.addListener("OnOptionsChanged", handleOptionsChanged);
		DI_Events.EventCenter.addListener("OnStartSpawns", handleStartSpawns);
		waveMultiplier = 1.0f;
	}

	public void OnDisable()
	{
		DI_Events.EventCenter.removeListener("OnOptionsChanged", handleOptionsChanged);
		DI_Events.EventCenter.removeListener("OnStartSpawns", handleStartSpawns);
	}

	public void handleOptionsChanged()
	{
		maxEnemies = (int)PlayerPrefs.GetFloat("Enemies On Screen", 25.0f);
	}

	public void skipTimer()
	{
		//float coins = (float)Mathf.RoundToInt(waveDelayTimer * (currentWave + 1) / 4);
		//playerState.addCoins(coins);
		waveDelayTimer = 0.0f;
		DI_Events.EventCenter.invoke("OnSkipWaveTimer");
	}

	public float getDifficultyHealthModifier()
	{
		return ((int)GameStateController.instance.difficultyLevel + 1.0f);
	}

	public float getDifficultySpeedModifier()
	{
		return ((int)GameStateController.instance.difficultyLevel + 1.0f) / 2.0f;
	}

	public void Update()
	{
		if (!gameOver) {
			if (!waitForEventToStart) {
				if (GameStateController.instance.gameState == GameStates.PLAYING) {
					// If we are not currently spawning a wave.
					if (!spawnInProgress) {
						// We spawned all the enemies and the player has killed them all.
						if (spawnsCompleted() && getEnemyCount() == 0) {
							// We have waited the required time between waves.
							if (waveDelayTimer == 0.0f) {
								// Start the wave spawning coroutine.
								StartCoroutine("startWave");
								waveDelayTimer = waveDelay;
								spawnInProgress = true;
							}
						}
					}
				}
			}
		}
	}
	
	public void handleStartSpawns()
	{
		//Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
		waitForEventToStart = false;
		waveDelayTimer = 0.0f;
		spawnInProgress = false;
	}

	public bool spawnsCompleted()
	{
		if (enemiesSpawned < 0) {
			enemiesSpawned = 0;
		}
		if (enemiesInWave < 0) {
			enemiesInWave = 0;
		}
		return (enemiesSpawned == enemiesInWave);
	}
	
	public List<EnemySpawnData> generateWave()
	{
		List<EnemySpawnData> spawnData = new List<EnemySpawnData>();

		// Generate The Wave
		if (currentWave > 0) {
			// Every 10 waves increase the number of spawn points but limit the maximum to the number of spawn points available
			float numberOfSpawnPoints = Mathf.Clamp(Mathf.Ceil((currentWave / 10)), 0, spawnPoints.Count);
			List<EnemySpawner> activeSpawnPoints = new List<EnemySpawner>();
			for (int index = 0; index < numberOfSpawnPoints; ++index) {
				activeSpawnPoints.Add(spawnPoints[index]);
			}

			// Each spawner is treated differently
			// Roll for the number of enemies that each spawner will spawn.
			// Clamp it at 50 per spawner so it doesn't take forever per wave.
			int numberOfEnemies = Mathf.Clamp((int) UnityEngine.Random.Range(currentWave + 3, Mathf.Ceil(currentWave * 5)), 0, 75 * activeSpawnPoints.Count);
			// Roll each enemy seperately until we get to the cap for that spawner.
			for (int enemyIndex = 0; enemyIndex < numberOfEnemies; ++enemyIndex) {
				EnemySpawnData enemyData = default(EnemySpawnData);
				enemyData.enemyType = selectRandomEnemyType();
				enemyData.spawnOrder = 1;
				//enemyData.spawnPoint = spawner.spawnName;
				int selectedSpawnIndex = UnityEngine.Random.Range(0, activeSpawnPoints.Count);
				EnemySpawner selectedSpawner = activeSpawnPoints [selectedSpawnIndex];
				enemyData.spawnPoint = selectedSpawner.spawnName;
				// We currently don't edit speed
				enemyData.speed = 1.0f;
				// Enemy health increases by 200% every 10 waves.
				enemyData.health = waveMultiplier;
				spawnData.Add(enemyData);
			}
		}
		return spawnData;
	}


	/// <summary>
	/// Selects a random type of the enemy.
	/// </summary>
	/// <description>
	/// The range is assigned with ints between 0 and 100.
	/// No two weights should be the same, the first item in the list with the weight in range will be selected.
	/// This needs a custom ui later on to assign weights and make sure they are kept clean.
	/// </description>
	/// <returns>The random enemy type.</returns>
	public Enemies selectRandomEnemyType()
	{
		int randomNumber = UnityEngine.Random.Range(0, 100);
		foreach (EnemyWeights data in weights.weights) {
			if (randomNumber > data.rangeMin && randomNumber <= data.rangeMax) {
				#if DEBUG
				Debug.Log("selectRandomEnemyType - Rolled a " + randomNumber + " this is translated to: " + data.type.ToString());
				#endif
				return data.type;
			}
		}
		return weights.weights[0].type;
	}

	/// <summary>
	/// Gets the enemy count and the count of the ones queued for spawn.
	/// </summary>
	/// <returns>The enemy count.</returns>
	public float getEnemyCount()
	{
		if (waitForEventToStart) {
			enemyController.enemiesKilledInWave = 0;
			return enemiesInWave;
		}
		else {
			return enemiesInWave - enemyController.enemiesKilledInWave;
		}
	}

	/// <summary>
	/// Gets the spawner by enum.
	/// </summary>
	/// <returns>The spawner by enum.</returns>
	/// <param name="_point">_point.</param>
	public EnemySpawner getSpawnerByEnum(SpawnPoints _point)
	{
		foreach (EnemySpawner spawner in spawnPoints) {
			if (spawner.spawnName == _point) {
				return spawner;
			}
		}
		return null;
	}

	/// <summary>
	/// Spawns the enemy.
	/// </summary>
	/// <returns>The enemy.</returns>
	/// <param name="data">Data.</param>
	public IEnumerator spawnEnemy(EnemySpawnData data)
	{
		while(true) {
			if (GameStateController.instance.gameState != GameStates.PLAYING) {
				yield return new WaitForSeconds(0.5f);
			}
			else {
				break;
			}
		}

		EnemySpawner spawner = getSpawnerByEnum(data.spawnPoint);
		if (spawner != null) {
			#if DEBUG
				Debug.Log("[Wave Controller] Found spawn point: " + spawner.name);
			#endif
		}
		else {
			#if DEBUG
				Debug.Log("[Wave Controller] Unable to find spawn point! " + data.spawnPoint);
			#endif
		}
		if (spawner != null) {
			// Wait for seconds equal to the spawn order * the delay
			yield return new WaitForSeconds(spawnDelay * data.spawnOrder);
			GameObject enemy = null;
			enemy = PoolController.instance.getPooledObject(data.enemyType.ToString());
			enemy.transform.position = spawner.transform.position;
			enemy.SetActive(true);

			if (enemy != null) {
				#if DEBUG
					Debug.Log("[Wave Controller] Sending Spawn Event");
				#endif
				Enemy enemyData = enemy.GetComponent<Enemy>();
				// Standard Health * wave difficulty modifer * game difficulty modifer
				enemyData.setHealth(enemyData.originalMaxHealth * waveMultiplier * getDifficultyHealthModifier());
				enemyData.setSpeed(enemyData.originalMovementSpeed * getDifficultySpeedModifier());
				DI_Events.EventCenter<SpawnPoints, GameObject>.invoke("OnRequestEnemySpawn", data.spawnPoint, enemy);
			}
		}
	}

	/// <summary>
	/// Starts the wave.
	/// </summary>
	/// <returns>The wave.</returns>
	public IEnumerator startWave()
	{
		enemiesInWave = 0;
		enemiesSpawned = 0;
		waveDelayTimer = waveDelay;

		if (currentWave == 0) {
			DI_Events.EventCenter<string>.invoke("OnSetNextLevel", nextLevel);
		}

		if (currentWave != 0) {
			DI_Events.EventCenter<int>.invoke("OnWaveEnd", (int) currentWave - 1);
		}

		#if DEBUG
			Debug.Log("Starting Wave");
			Debug.Log("[Wave Controller] Waiting for: " + waveDelay);
		#endif

		++currentWave;
		//Increase health by an extra 50% every 5 waves.
		if (currentWave % 5 == 0) {
			waveMultiplier += 0.5f;
		}

		if (!endlessMode && currentWave == maxWave) {
			if (sendVictoryEventToDialog) {
				DI_Events.EventCenter<string, DialogEventTypes>.invoke("OnDialogEvent", dialogName, DialogEventTypes.EVENT_START_DIALOG);
				gameOver = true;
				return true;
			}
			else {
				DI_Events.EventCenter.invoke("OnVictory");
				gameOver = true;
				return true;
			}
		}

		// Count down the time left till next wave.
		while (true) {
			yield return new WaitForSeconds(0.1f);
			if (GameStateController.instance.gameState == GameStates.PLAYING) {
				if (getEnemyCount() > 0) {
					nextWaveInText.gameObject.SetActive(false);
					waveDelayTimer = waveDelay;
				}
				else {
					nextWaveInText.gameObject.SetActive(true);
					waveDelayTimer -= 0.1f;
				}
			}

			// This would be handled by GUI Controller but I think calling the event every frame would be over kill.
			nextWaveInText.text = "Next Wave In: " + Mathf.CeilToInt(waveDelayTimer);

			if (waveDelayTimer <= 0.0f) {
				waveDelayTimer = 0.0f;
				break;
			}
		}

		// Timer finished, hide the counter.
		nextWaveInText.gameObject.SetActive(false);
		#if DEBUG
			Debug.Log("[Wave Controller] Starting Spawns");
		#endif
		//int index = 0;

		// Spawn Enemies
		List<EnemySpawnData> wave;
		if (endlessMode) {
			if (currentWave % 10 == 0) {
				// Every 10th wave is a boss wave.
				wave = new List<EnemySpawnData>();
				EnemySpawnData bossWave = default(EnemySpawnData);
				bossWave.enemyType = Enemies.ENEMY_BOSS;
				bossWave.spawnPoint = SpawnPoints.RED;
				bossWave.health = waveMultiplier;
				bossWave.speed = 1.0f;
			}
			else {
				wave = generateWave();
			}
		}
		else {
			wave = waves[(int)currentWave - 1].enemies;
		}

		enemiesInWave = wave.Count;
		foreach (EnemySpawnData enemySpawnData in wave) {
			spawnQueue.Enqueue(enemySpawnData);
		}

		while (true) {
			yield return new WaitForSeconds(spawnDelay);
			if (GameStateController.instance.gameState == GameStates.PLAYING) {
				if (spawnQueue.Count == 0) {
					if (spawnsCompleted()) {
						spawnInProgress = false;
						break;
					}
				}
				else {
					if ((getEnemyCount() - spawnQueue.Count) < maxEnemies) {
						EnemySpawnData enemyData = spawnQueue.Dequeue();
						StartCoroutine("spawnEnemy", enemyData);
						enemiesSpawned++;
					}
				}
			}
		}
	}
	#endregion
}