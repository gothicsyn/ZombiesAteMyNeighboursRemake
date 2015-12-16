// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace DI.Menus
{
	[AddComponentMenu("Menus/Main/Play")]
	public class Play : MonoBehaviour
	{
		public List<GameObject> objectsToDisable;
		public GameObject loading;
		public Image loadingBar;
		public Text loadingProgress;
		public string levelToLoad;
		public DifficultyTypes difficultyLevel;

		public void OnClick()
		{
			loading.SetActive(true);
			PlayerPrefs.SetInt ("Difficulty", (int)difficultyLevel);
			DI_Events.EventCenter<string, Image, Text>.invoke("LoadLevel", levelToLoad, loadingBar, loadingProgress);
			foreach (GameObject toDisable in objectsToDisable.ToArray()) {
				toDisable.SetActive(false);
			}
		}
	}
}