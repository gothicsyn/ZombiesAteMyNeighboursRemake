// Devils Inc Studios
// // Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using System.Collections.Generic;

public class EnterCheatMode : MonoBehaviour
{
	private List<KeyCode> cheatMenuCode;
	public List<KeyCode> lastButtons;
	public bool codeEntered = false;
	public GameObject cheatMenuButon;
	public bool mainMenu = false;

	public void OnEnable()
	{
		lastButtons = new List<KeyCode>();
		cheatMenuCode = new List<KeyCode>(new KeyCode[]{KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow, 
			KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.B, KeyCode.A, KeyCode.Return});
	}

	public void OnGUI()
	{
		Event guiEvent = Event.current;
		if (guiEvent.isKey && Input.anyKeyDown) {
			updateList(guiEvent.keyCode);
		}
	}
	
	public bool checkCode()
	{
		int index = 0;
		if (lastButtons.Count == cheatMenuCode.Count) {
			foreach (KeyCode key in lastButtons.ToArray()) {
				if (key != cheatMenuCode[index]) {
					return false;
				}
				++index;
			}
			return true;
		}
		return false;
	}

	public void updateList(KeyCode key)
	{
		if (lastButtons.Count >= cheatMenuCode.Count) {
			lastButtons.RemoveAt(0);
		}
		if (key != KeyCode.None) {
			lastButtons.Add (key);
		}

		if (checkCode()) {
			codeEntered = true;
			if (!mainMenu) {
				cheatMenuButon.SetActive(true);
			}
		}
	}
}
