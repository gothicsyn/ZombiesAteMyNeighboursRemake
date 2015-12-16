// Devils Inc Studios
// // Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;

public class InGameOptions : MonoBehaviour
{
	public void OnClick()
	{
		MenuController.instance.onOptionsMenuEnter();
	}

	public void OnBack()
	{
		MenuController.instance.onMenuEnter();
	}
}