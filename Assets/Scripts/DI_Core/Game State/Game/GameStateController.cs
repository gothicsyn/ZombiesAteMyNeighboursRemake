// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

#region Includes
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
#endregion

// TODO refactor this class
// This contains elements of a menu system and gamestate controls and needs to be split for multiplayer.

[AddComponentMenu("Game State/Game State Controller")]
public class GameStateController : MonoBehaviour
{
	#region Public Variables
	public GameStates gameState;
	public PlayerState playerState;
	public DifficultyTypes difficultyLevel;
	public static GameStateController instance;

	public Vector3 mapSize;

	// Number of players
	public int players = 1;

	public bool lockPlayerCursor = false;
	//TODO REMOVE ME
	public Canvas skipInfo;
	#endregion

	public void OnEnable()
	{
		if (lockPlayerCursor) {
			lockCursor();
		}
		// TODO make this an option
		//Application.targetFrameRate = 60;
		difficultyLevel = (DifficultyTypes)PlayerPrefs.GetInt("Difficulty", 0);
	}

	/// <summary>
	/// Locks the cursor.
	/// </summary>
	public void lockCursor()
	{
		if (lockPlayerCursor) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	/// <summary>
	/// Unlocks the cursor.
	/// </summary>
	public void unlockCursor()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	/// <summary>
	/// Called when a cinema starts that is unskipable
	/// </summary>
	public void enterCinemaNoSkip()
	{
		#if DEBUG
			Debug.Log("enterCinemaNoSkip");
		#endif
		gameState = GameStates.IN_CUT_SCENE;
	}

	/// <summary>
	/// Called when a cinema starts
	/// </summary>
	public void enterCinema()
	{
		#if DEBUG
			Debug.Log("enterCinema");
		#endif
		gameState = GameStates.IN_CUT_SCENE;
		skipInfo.gameObject.SetActive(true);
	}

	/// <summary>
	/// Called when the cinema exits
	/// </summary>
	public void exitCinema()
	{
		#if DEBUG
			Debug.Log("exitCinema");
		#endif
		gameState = GameStates.PLAYING;
		skipInfo.gameObject.SetActive(false);
	}

	/// <summary>
	/// Called when the cinema exits
	/// </summary>
	public void exitCinemaNoSkip()
	{
		#if DEBUG
		Debug.Log("exitCinemaNoSkip");
		#endif
		gameState = GameStates.PLAYING;
	}

	/// <summary>
	/// Called when the menu is entered
	/// </summary>
	public void enterMenu()
	{
		#if DEBUG
			Debug.Log("enterMenu");
		#endif
		unlockCursor();
		gameState = GameStates.IN_MENU;
	}

	/// <summary>
	/// Called when the menu is exited
	/// </summary>
	public void exitMenu()
	{
		#if DEBUG
			Debug.Log("exitMenu");
		#endif
		lockCursor();
		gameState = GameStates.PLAYING;
	}

	/// <summary>
	/// Called when the game is paused.
	/// </summary>
	public void onPause()
	{
		#if DEBUG
			Debug.Log("OnPause");
		#endif
		unlockCursor();
		gameState = GameStates.PAUSED;
	}

	/// <summary>
	/// Called when the game is unpaused
	/// </summary>
	public void onResume()
	{
		#if DEBUG
			Debug.Log("OnResume");
		#endif
		lockCursor();
		gameState = GameStates.PLAYING;
	}

	public void Awake()
	{
		instance = this;
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (gameState == GameStates.IN_MENU) {
				MenuController.instance.onMenuExit();
			}
			else if (gameState == GameStates.PLAYING) {
				MenuController.instance.onMenuEnter();
			}
		}

		if (Input.GetKeyDown(KeyCode.Pause)) {
			if (gameState == GameStates.PAUSED) {
				MenuController.instance.onResume();
			}
			else {
				MenuController.instance.onPause();
			}
		}
	}


	public void onCheatChangeUnlimitedAmmo(bool state)
	{
		playerState.unlimitedAmmo = state;
	}
}
