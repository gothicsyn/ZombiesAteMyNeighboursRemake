using UnityEngine;
using System.Collections;

public class Spawn_Point : MonoBehaviour {

	public GameObject prefab;
	public float repeatTime = 3f;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("Spawn", 2f, repeatTime);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void Spawn () {
		Instantiate (prefab, transform.position, Quaternion.identity);
	}
}
