using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {

	public int score = 0;
	public Text text;


	// Use this for initialization
	void Start () {
		score = 0;
		UpdateDisplay ();
	}

	public void Add (int amt) {
		score += amt;
		UpdateDisplay ();
	}

	void UpdateDisplay () {
		text.text = "Score: " + score;
	}
}
