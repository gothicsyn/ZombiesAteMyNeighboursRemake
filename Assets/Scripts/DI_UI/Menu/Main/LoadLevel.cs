// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
// TODO: Include a description of the file here.
//
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;

namespace DI.Menus
{
	public class LoadLevel : MonoBehaviour
	{
		public Text quoteDisplay;

		public void OnEnable()
		{
			Object.DontDestroyOnLoad(this.gameObject);
			DI_Events.EventCenter<string, Image, Text>.addListener("LoadLevel", loadLevelHandler);
		}
		
		public void OnDisable()
		{
			DI_Events.EventCenter<string, Image, Text>.removeListener("LoadLevel", loadLevelHandler);
		}
		
		public void loadLevelHandler(string levelToLoad, Image loadingBar, Text loadingProgress)
		{
			StartCoroutine(loadLevel(levelToLoad, loadingBar, loadingProgress));
		}
		
		public IEnumerator loadLevel(string levelToLoad, Image loadingBar, Text loadingProgress)
		{
			quoteDisplay.text = fetchQuote();
			AsyncOperation levelLoader = Application.LoadLevelAsync(levelToLoad);
			levelLoader.allowSceneActivation = false;
			while (!levelLoader.isDone) {
				loadingBar.fillAmount = levelLoader.progress;
				loadingProgress.text = "Loading: " + Mathf.Round(levelLoader.progress * 100) + "%";
				
				if (levelLoader.progress >= 0.9f && !levelLoader.allowSceneActivation) {
					levelLoader.allowSceneActivation = true;
				}
				yield return new WaitForEndOfFrame();
			}
			Destroy(this.gameObject);
		}

		public string fetchQuote()
		{
			TextAsset quotesFile = (TextAsset)Resources.Load("Quotes", typeof(TextAsset));
			string[] quotes = Regex.Split(quotesFile.text, "\r\n");
			return quotes[UnityEngine.Random.Range(0, quotes.Length - 1)];
		}
	}
}