using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	private bool _isFirstMenu 		= true;		// Sets the system to read First Menu
	private bool _isLevelSelect 	= false;	// Sets the system to read Level Select
	private bool _isOptionsMenu 	= false;	// Sets the system to read Options Menu
	private bool _isUpdate 			= false;	// Sets the system to read Update System
	private bool _isQuit 			= false;	// Sets the system to read Quit Game

	public string Version;			// Allows manual setting of the Version Number in the Inspector

	//Value used for placing version number in inspector
	public int width1 = 50;
	public int height1 = 200;
	public int offset1 = 350;
	public int pos1 = 10;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}


	void OnGUI () {
		FirstMenu ();
		displayVersion();
	}


	void FirstMenu () {
		if(_isFirstMenu) {
			if(GUI.Button(new Rect(10, Screen.height / 2 - 100, 150, 25), "Start Game")){
				Application.LoadLevel("Level1");
			}
		}

		if(_isFirstMenu) {
			if(GUI.Button(new Rect(10, Screen.height / 2 - 75, 150, 25), "Load Game")){
			}
		}

		if(_isFirstMenu) {
			if(GUI.Button(new Rect(10, Screen.height / 2 - 50, 150, 25), "Level Select")){
			}
		}

		if(_isFirstMenu) {
			if(GUI.Button(new Rect(10, Screen.height / 2 - 25, 150, 25), "Options")){
			}
		}

		if(_isFirstMenu) {
			if(GUI.Button(new Rect(10, Screen.height / 2 + 0, 150, 25), "Update Game")){
			}
		}

		if(_isFirstMenu) {
			if(GUI.Button(new Rect(10, Screen.height / 2 + 25, 150, 25), "Quit Game")){
				Application.Quit();
			}
		}
	}

	public void displayVersion (){
		GUI.contentColor = Color.yellow;
		GUI.Label(new Rect(40, Screen.height / 2 + 50, 150, 25), "Version " + Version);
	}
}
