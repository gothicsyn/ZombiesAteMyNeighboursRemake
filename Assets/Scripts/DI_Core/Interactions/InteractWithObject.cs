// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using System.Collections;
[RequireComponent(typeof(HighlightObject))]
[RequireComponent(typeof(Rigidbody))]
[AddComponentMenu("Interactions/Interact With Object")]
public class InteractWithObject : MonoBehaviour
{
	private HighlightObject hightlightScript;

	public void OnEnable()
	{
		hightlightScript = this.GetComponent<HighlightObject>();
	}

	public void OnRayHit()
	{
		//Debug.Log("On Mouse Over");
		if (hightlightScript.highlighted)
		{
			//Debug.Log("On Mouse Over: Highlighted");
			if (Input.GetMouseButtonDown(0)) {
			}
		}
	}
}