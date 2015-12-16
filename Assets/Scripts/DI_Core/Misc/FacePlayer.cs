// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using System;
[AddComponentMenu("Misc/Face Player")]
public class FacePlayer : MonoBehaviour
{
	public bool correctRotation = false;
	public bool lockX = false;
	private float startingX = 0.0f;

	public void OnEnable()
	{
		if (lockX) {
			startingX = transform.rotation.x;
		}
	}

	public void Update()
	{
		transform.LookAt(Camera.main.transform.position);

		if (correctRotation) {
			transform.Rotate(0, 180, 0);
		}
		if (lockX) {
			transform.Rotate(transform.rotation.x * -1, 0, 0);
			transform.Rotate(startingX, 0, 0);
		}
	}
}