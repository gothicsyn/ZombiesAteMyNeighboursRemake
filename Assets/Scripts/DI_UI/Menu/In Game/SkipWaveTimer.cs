// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Menus/In Game/Skip Wave Timer")]
public class SkipWaveTimer : MonoBehaviour
{
	public WaveController waveController;
	public Button button;
	public GameObject skipWaveText;

	public void Update()
	{
		if (skipWaveText.activeSelf) {
			button.interactable = true;
		}
		else {
			button.interactable = false;
		}
	}

	public void OnClick()
	{
		if (waveController.spawnInProgress && waveController.getEnemyCount() == 0) {
			waveController.skipTimer();
		}
	}
}