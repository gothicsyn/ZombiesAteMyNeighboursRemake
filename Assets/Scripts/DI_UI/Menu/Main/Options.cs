// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;

namespace DI.Menus
{
	[AddComponentMenu("Menus/Main/Options")]
	public class Options : MonoBehaviour
	{
		public GameObject mainMenu;
		public GameObject optionsMenu;

		public void goBack()
		{
			mainMenu.SetActive(true);
			optionsMenu.SetActive(false);
			DI_Events.EventCenter.invoke("OnOptionsChanged");
		}

		public void OnClick()
		{
			mainMenu.SetActive(false);
			optionsMenu.SetActive(true);
		}
	}
}