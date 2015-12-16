// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
// TODO: Include a description of the file here.
//

using UnityEngine;

public class MiniMap : MonoBehaviour
{
	[Header("Minimap Settings")]
	public GameObject fullScreenMap;
	public GameObject smallMap;
	public bool mapOpen = false;

	public void OnEnable()
	{
		if (mapOpen) {
			fullScreenMap.SetActive(true);
			smallMap.SetActive(false);
		}
		else {
			fullScreenMap.SetActive(false);
			smallMap.SetActive(true);
		}
	}

	public void LateUpdate()
	{
		if (Input.GetButtonDown("Toggle Map")) {
			mapOpen = !mapOpen;
			fullScreenMap.SetActive(!fullScreenMap.activeSelf);
			smallMap.SetActive(!smallMap.activeSelf);
		}
	}
}