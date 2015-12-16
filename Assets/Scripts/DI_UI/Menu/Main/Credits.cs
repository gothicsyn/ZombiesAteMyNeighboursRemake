// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: This script is used to allow button presses which enables and disables the GUI
//

using UnityEngine;

namespace DI.Menus
{
	[AddComponentMenu("Menus/Main/Credits")]
	public class Credits : MonoBehaviour
	{
		public Canvas mainMenu;
		public Canvas creditsMenu;

		public void goBack()
		{
			mainMenu.gameObject.SetActive(true);
			creditsMenu.gameObject.SetActive(false);
		}
		
		public void OnClick()
		{
			mainMenu.gameObject.SetActive(false);
			creditsMenu.gameObject.SetActive(true);
		}
	}
}