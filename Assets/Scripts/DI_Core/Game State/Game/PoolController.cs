// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

[AddComponentMenu("Game State/Pool Controller")]
public class PoolController : MonoBehaviour
{
	public bool willGrow = true;
	public int startingSize = 1;
	public Dictionary<string, List<GameObject>> poolContents;
	public List<GameObject> prefabs;
	[HideInInspector]
	public static PoolController instance;

	public void Awake()
	{
		instance = this;
		poolContents = new Dictionary<string, List<GameObject>>();

		for (int prefabIndex = 0; prefabIndex < prefabs.Count; ++prefabIndex) {
			for (int index = 0; index < startingSize; ++index) {
				string key = prefabs[prefabIndex].name;
				GameObject tempObject = (GameObject)Instantiate(prefabs[prefabIndex]);
				tempObject.SetActive(false);
				if (!poolContents.ContainsKey(key)) {
					List<GameObject> objectPool = new List<GameObject>();
					poolContents.Add(key, objectPool);
					GameObject poolItemHeader = new GameObject("Pool Members - " + key);
					poolItemHeader.transform.parent = this.transform;
				}
				tempObject.transform.parent = this.transform.GetChild(prefabIndex);
				poolContents[key].Add(tempObject);
			}
		}
	}

	public GameObject getPrefabByName(string name)
	{
		for (int index = 0; index < prefabs.Count; ++index) {
			if (prefabs[index].name == name) {
				return prefabs[index];
			}
		}
		return null;
	}

	public int getPrefabId(GameObject prefab)
	{
		if (prefabs.Contains(prefab)) {
			for (int index = 0; index < prefabs.Count; ++index) {
				if (prefabs[index] == prefab) {
					return index;
				}
			}
		}
		return 0;
	}

	public GameObject getPooledObject(string type)
	{
		if (poolContents.ContainsKey(type)) {
			// We found an object we can use, return it.
			for (int index = 0; index < poolContents[type].Count; ++index) {
				if (poolContents[type][index] != null) {
					if (!poolContents[type][index].activeInHierarchy) {
						return poolContents[type][index];
					}
				}
			}
			// We are out of pooled objects to use, spawn more if we can.
			if (willGrow) {
				GameObject prefab = getPrefabByName(type);
				if (prefab != null) {
					GameObject tempObject = (GameObject)Instantiate(prefab);
					tempObject.SetActive(false);
					tempObject.transform.parent = this.transform.GetChild(getPrefabId(prefab));
					poolContents[prefab.name].Add(tempObject);
					return tempObject;
				}
				else {
					throw new Exception("A non-managed item is being requested from the pool manager - Item Requested: " + type);
				}
			}

			// We couldn't grow and we didn't have any left return null.
			return null;
		}

		// A non managed object is being requested.
		throw new Exception("A non-managed item is being requested from the pool manager - Item Requested: " + type);
	}
}