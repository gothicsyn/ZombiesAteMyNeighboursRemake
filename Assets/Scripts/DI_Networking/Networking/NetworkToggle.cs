// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
// TODO: Include a description of the file here.
//

using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class NetworkToggle : MonoBehaviour
{
	public List<GameObject> toggledObjects;
	public bool objectsEnabled = false;
	public NetworkManager networkManager;

	public void OnEnable()
	{
		networkManager = GameObject.Find("Network Manager").GetComponent<NetworkManager>();
	}

	public void Update()
	{
		if (networkManager.isNetworkActive) {
			foreach (GameObject toggledObject in toggledObjects.ToArray()) {
				toggledObject.SetActive(objectsEnabled);
			}
			this.enabled = false;
		}
	}
}