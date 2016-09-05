using UnityEngine;
using System.Collections;

public class WaterGun : MonoBehaviour {

	public Transform muzzle;
	public GameObject prefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0)) {
			Shoot();
		}
	}

	void Shoot () {
		Instantiate (prefab, muzzle.position, Quaternion.identity);
	}
}
