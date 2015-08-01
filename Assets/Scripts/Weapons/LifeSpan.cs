using UnityEngine;
using System.Collections;

public class LifeSpan : MonoBehaviour {

	public int 			lifetime	 = 45;			// Lifetime of object


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Destroy(gameObject, lifetime);
	}
}
