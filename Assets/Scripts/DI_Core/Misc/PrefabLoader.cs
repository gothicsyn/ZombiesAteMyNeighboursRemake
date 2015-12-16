// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

#region Includes
using UnityEngine;
using System.Collections.Generic;
using System;
#endregion

public class PrefabLoader : MonoBehaviour
{
	#region Public Variables
	public List<PrefabEntry> prefabs;
	public static PrefabLoader instance;
	#endregion

	#region Public Methods

	public void Awake()
	{
		instance = this;
	}

	/// <summary>
	/// Gets the prefab associated with the given name.
	/// </summary>
	/// <returns>The prefab.</returns>
	/// <param name="name">Name.</param>
	/// <exception cref="Exception">Thrown when the requested prefab does not exist.</exception>
	public GameObject getPrefab(string name)
	{
		#if DEBUG
			Debug.Log("Requested Prefab: " + name);
		#endif
		for (int iteration = 0; iteration < prefabs.Count; ++iteration) {
			if (prefabs[iteration].name == name) {
				return prefabs[iteration].gameObject;
			}
		}

		#if DEBUG
			Debug.Log("Throwing exception due to non-matched name.");
		#endif
		throw new Exception("Unable to find prefab with the requested name: " + name);
	}
	#endregion
}