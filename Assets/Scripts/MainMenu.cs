// <summary>
/// MainMenu.cs
/// 28-07-15
/// M A Plant
/// 
/// A simple (ish) script that will run as a main menu for most simplistic styled games. 
/// </summary>

using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public GUISkin gameSkin;

	private bool _isFirstMenu 		= true;		// Sets the system to read First Menu
	private bool _isLoadGame		= false;	// Brings up the load game options
	private bool _isLevelSelect 	= false;	// Sets the system to read Level Select
	private bool _isOptionsMenu 	= false;	// Sets the system to read Options Menu
	private bool _isUpdate 			= false;	// Sets the system to read Update System
	private bool _isNewGameMenu		= false;	// Will access new game options
	private bool _isOnlineGame		= false;	// Will access the online components

	private string _playerName		= "";		// Sets the Player Name
	private string _playerGender	= "";		// Sets gender

	public string gameTitle			= "";		// Game Title
	public string Version;						// Allows manual setting of the Version Number in the Inspector
	public Texture title;						// Image for Title Screen or new title texture

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	// Sets up details and information to be call inside OnGUI.
	void OnGUI () {
		GUI.skin = gameSkin;

		GUI.DrawTexture(new Rect(Screen.width / 2 - 50, 10, 350, 200), title);

		FirstMenu ();
		NewGameOptions();
		OnlineGame();
		LoadGameMenu ();
		LevelSelectMenu ();
		OptionsMenu ();
		UpdateMenu ();
		displayVersion();

		// Will allow the user to return to the Main Menu from any other menu screen
		if(_isLevelSelect == true || _isNewGameMenu == true || _isOnlineGame == true || _isLoadGame == true || _isOptionsMenu == true || _isUpdate == true) {
			if(GUI.Button(new Rect(10, Screen.height - 35, 150, 25), "Main Menu")){
				_isLevelSelect 	= false;
				_isLoadGame 	= false;
				_isOptionsMenu	= false;
				_isUpdate		= false;
				_isNewGameMenu	= false;
				_isOnlineGame 	= false;
				
				_isFirstMenu	= true;
			}
		}
	}

	// Provides and displays the available options of the Main Menu
	public void FirstMenu () {
		if(_isFirstMenu) {
			if(GUI.Button(new Rect(10, Screen.height / 2 - 100, 150, 25), "Start Game")){
				_isFirstMenu = false;
				_isNewGameMenu = true;
		}

		// Will switch to Load Game Menu and run relevant checks.
		if(GUI.Button(new Rect(10, Screen.height / 2 - 75, 150, 25), "Load Game")){
			_isFirstMenu = false;
			_isLoadGame = true;
		}

		// Will switch to Online Menu and run relevant checks.
		if(GUI.Button(new Rect(10, Screen.height / 2 - 50, 150, 25), "Online Co-Op")){
			_isFirstMenu = false;
			_isUpdate = true;
		}
		
		// Will switch to Level Select Menu and run relevant checks.
		if(GUI.Button(new Rect(10, Screen.height / 2 - 25, 150, 25), "Level Select")){
			_isFirstMenu = false;
			_isLevelSelect = true;
		}

		// Will switch to Options and allow changing of visual and audio options saving them to file to be installed .
		// globally.
		if(GUI.Button(new Rect(10, Screen.height / 2 + 0, 150, 25), "Options")){
			_isFirstMenu = false;
			_isOptionsMenu = true;
		}

		// Will switch to Update Game Menu and run relevant checks for a new or updated game version.
		if(GUI.Button(new Rect(10, Screen.height / 2 + 25, 150, 25), "Update Game")){
			_isFirstMenu = false;
			_isUpdate = true;
		}

		// Quits the game application completely.
		if(GUI.Button(new Rect(10, Screen.height / 2 + 50, 150, 25), "Quit Game")){
			Application.Quit();
		}
	}
}

	//Will display the new game options
	public void NewGameOptions() {
		if(_isNewGameMenu) {
			GUI.Label(new Rect(30, Screen.height / 2 - 200, 200, 50), "Select Player", "Sub Menu Title");

			GUI.Label(new Rect(10, Screen.height / 2 - 100, 90, 25), "Player Name: ");
			_playerName = GUI.TextField(new Rect(120, Screen.height / 2 - 100, 200, 25), _playerName);	

			GUI.Label(new Rect(10, Screen.height / 2 - 65, 90, 25), "Player Gender: ");
			_playerGender = GUI.TextField(new Rect(120, Screen.height / 2 - 65, 200, 25), _playerGender);

			if(_playerName != "" && _playerGender != "") {
				if(GUI.Button(new Rect(10, Screen.height / 2 - 30, 150, 25), "Apply")){
					PlayerPrefs.SetString(_playerName, "Player Name");
					PlayerPrefs.SetString(_playerGender, "Player Gender");

					Application.LoadLevel("Level1");
					}
				}
			else 
				if(GUI.Button(new Rect(10, Screen.height / 2 - 30, 150, 25), "Generating")){
					
				}
			}
		}

	// Function to display the Load Game Menu and Options
	public void OnlineGame () {
		if(_isOnlineGame) {
			GUI.Label(new Rect(30, Screen.height / 2 - 200, 200, 50), "Online Options", "Sub Menu Title");

			if(GUI.Button(new Rect(10, Screen.height / 2 - 100, 200, 150), "Start Online Game")){
				Application.LoadLevel("Online");
			}
		}
	}

	// Function to display the Load Game Menu and Options
	public void LoadGameMenu () {
		if(_isLoadGame) {
			GUI.Label(new Rect(30, Screen.height / 2 - 200, 200, 50), "Load Game", "Sub Menu Title");

			GUI.Box(new Rect(10, Screen.height / 2 - 100, Screen.width / 2 + 100, Screen.height - 450), "Select Save File");
		}
	}

	// Function to display the Level Select Menu
	public void LevelSelectMenu () {
		if(_isLevelSelect) {
			GUI.Label(new Rect(30, Screen.height / 2 - 200, 200, 50), "Level Select", "Sub Menu Title");

			// Top Row of Level Select Buttons
			if(GUI.Button(new Rect(10, Screen.height / 2 - 100, 200, 150), "Level1")){
				Application.LoadLevel("Level1");
			}

			if(GUI.Button(new Rect(220, Screen.height / 2 - 100, 200, 150), "Level2")){
			
			}

			if(GUI.Button(new Rect(430, Screen.height / 2 - 100, 200, 150), "Level3")){
					
			}

			if(GUI.Button(new Rect(640, Screen.height / 2 - 100, 200, 150), "Level4")){
							
			}

			if(GUI.Button(new Rect(850, Screen.height / 2 - 100, 200, 150), "Level5")){
				
			}

			// Second Row of Level Select Buttons
			if(GUI.Button(new Rect(10, Screen.height / 2 + 60, 200, 150), "Level6")){
				
			}
			
			if(GUI.Button(new Rect(220, Screen.height / 2 + 60, 200, 150), "Level7")){
				
			}
			
			if(GUI.Button(new Rect(430, Screen.height / 2 + 60, 200, 150), "Level8")){
				
			}

			if(GUI.Button(new Rect(640, Screen.height / 2 + 60, 200, 150), "Level9")){
				
			}
			
			if(GUI.Button(new Rect(850, Screen.height / 2 + 60, 200, 150), "Level10")){
				
			}
		}
	}

	// Function to display the Options Menu
	public void OptionsMenu () {
		if(_isOptionsMenu) {
			GUI.Label(new Rect(30, Screen.height / 2 - 200, 200, 50), "Options", "Sub Menu Title");

			GUI.Box(new Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height), "");

			// Will allow Audio options to be set.
			if(GUI.Button(new Rect(10, Screen.height / 2 - 50, 150, 25), "Audio")){
			}
			
			// Will allow graphics options to be set
			if(GUI.Button(new Rect(10, Screen.height / 2 - 25, 150, 25), "Graphics")){
			}
		}
	}

	// Function to display the Update Menu
	public void UpdateMenu () {
		if(_isUpdate) {
			return;
		}
	}

	// Function to display the Version Number
	public void displayVersion (){
		GUI.Label(new Rect(Screen.width / 2 - 10, Screen.height - 35, 150, 25), "Version " + Version, "VersionNo");
		
	}
}
