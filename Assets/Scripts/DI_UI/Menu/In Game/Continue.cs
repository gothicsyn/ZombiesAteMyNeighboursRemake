// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;

[AddComponentMenu("Menus/In Game/Continue")]
public class Continue : MonoBehaviour
{
	public void OnClick()
	{
		MenuController.instance.onMenuExit();
	}
}