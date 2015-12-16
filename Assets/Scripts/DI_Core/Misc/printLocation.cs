// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
// TODO: Include a description of the file here.
//

using UnityEngine;

public class printLocation : MonoBehaviour
{
	public void Awake()
	{
		Debug.Log(Application.persistentDataPath);
	}
}