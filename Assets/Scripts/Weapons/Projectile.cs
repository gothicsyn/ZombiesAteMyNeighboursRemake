using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public float timer = 3f;
	public float speed = 4f;

	// Use this for initialization
	void Start () {
		Invoke ("Die", timer);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * speed * Time.deltaTime;
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Enemy") {
			CancelInvoke ();
			Destroy (other.gameObject);
			Debug.Log("Point Call + 100 Zombie");
			Die ();
		}

		Debug.Log ("Collison Detected" + other.tag);
	}

	void Die () {
		Destroy(gameObject);
	}
}
