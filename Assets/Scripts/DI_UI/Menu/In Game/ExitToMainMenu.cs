// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;

[AddComponentMenu("Menus/In Game/Exit to Main Menu")]
public class ExitToMainMenu : MonoBehaviour
{	
	public void OnClick()
	{
		Application.LoadLevel("Main");
	}
}