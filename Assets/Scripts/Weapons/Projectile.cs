using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public float timer = 3f;
	public float speed = 4f;

	public Score score;

	void Awake () {
		score = GameObject.FindGameObjectWithTag ("Score").GetComponent<Score> ();
	}

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
			score.Add (100);
			Die ();
		}
	}

	void Die () {
		Destroy(gameObject);
	}
}
