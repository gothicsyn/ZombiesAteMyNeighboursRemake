using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	private bool _isFirstMenu 		= true;		// Sets the system to read First Menu
	private bool _isLoadGame		= false;	// Brings up the load game options
	private bool _isLevelSelect 	= false;	// Sets the system to read Level Select
	private bool _isOptionsMenu 	= false;	// Sets the system to read Options Menu
	private bool _isUpdate 			= false;	// Sets the system to read Update System
//	private bool _isQuit 			= false;	// Sets the system to read Quit Game

	public string Version;			// Allows manual setting of the Version Number in the Inspector

	//Value used for placing version number in inspector
	public int width1 	= 50;
	public int height1 	= 200;
	public int offset1 	= 350;
	public int pos1 	= 10;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	// Sets up details and information to be call inside OnGUI.
	void OnGUI () {
		FirstMenu ();
		LoadGameMenu ();
		LevelSelectMenu ();
		OptionsMenu ();
		UpdateMenu ();
		displayVersion();

		// Will allow the user to return to the Main Menu from any other menu screen
		if(_isLevelSelect == true || _isLoadGame == true || _isOptionsMenu == true || _isUpdate == true) {
			if(GUI.Button(new Rect(10, Screen.height - 35, 150, 25), "Main Menu")){
				_isLevelSelect 	= false;
				_isLoadGame 	= false;
				_isOptionsMenu	= false;
				_isUpdate		= false;

				_isFirstMenu	= true;
			}
		}
	}

	// Provides and displays the available options of the Main Menu
	void FirstMenu () {
		if(_isFirstMenu) {
			if(GUI.Button(new Rect(10, Screen.height / 2 - 100, 150, 25), "Start Game")){
				Application.LoadLevel("Level1");
			}
		}

		// Will switch to Load Game Menu and run relevant checks.
		if(_isFirstMenu) {
			if(GUI.Button(new Rect(10, Screen.height / 2 - 75, 150, 25), "Load Game")){
				_isFirstMenu = false;
				_isLoadGame = true;


			}
		}

		// Will switch to Level Select Menu and run relevant checks.
		if(_isFirstMenu) {
			if(GUI.Button(new Rect(10, Screen.height / 2 - 50, 150, 25), "Level Select")){
				_isFirstMenu = false;
				_isLevelSelect = true;

			}
		}

		// Will switch to Options and allow changing of visual and audio options saving them to file to be installed .
		// globally.
		if(_isFirstMenu) {
			if(GUI.Button(new Rect(10, Screen.height / 2 - 25, 150, 25), "Options")){
				_isFirstMenu = false;
				_isOptionsMenu = true;

			}
		}

		// Will switch to Update Game Menu and run relevant checks for a new or updated game version.
		if(_isFirstMenu) {
			if(GUI.Button(new Rect(10, Screen.height / 2 + 0, 150, 25), "Update Game")){
				_isFirstMenu = false;
				_isUpdate = true;

			}
		}

		// Quits the game application completely.
		if(_isFirstMenu) {
			if(GUI.Button(new Rect(10, Screen.height / 2 + 25, 150, 25), "Quit Game")){
				Application.Quit();
			}
		}
	}

	void LoadGameMenu () {
		if(_isLoadGame) {
			return;
		}
	}

	void LevelSelectMenu () {
		if(_isLevelSelect) {
			return;
		}
	}

	void OptionsMenu () {
		if(_isOptionsMenu) {
			return;
		}
	}

	void UpdateMenu () {
		if(_isUpdate) {
			return;
		}
	}

	public void displayVersion (){
		GUI.contentColor = Color.yellow;
		GUI.Label(new Rect(40, Screen.height / 2 + 50, 150, 25), "Version " + Version);
	}
}
