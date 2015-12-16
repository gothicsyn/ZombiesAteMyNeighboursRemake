// Devils Inc Studios
// // Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

[AddComponentMenu("Triggers/Trigger")]
public class Trigger : MonoBehaviour
{
	public bool singleUse = true;
	public bool triggered = false;
	public float coolDown = 15.0f;
	public bool useTagFilter = true;
	public List<string> allowedTags;

	public IEnumerator resetTrigger()
	{
		yield return new WaitForSeconds(coolDown);
		triggered = false;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!singleUse) {
			StartCoroutine("resetTrigger");
		}
		if (singleUse && triggered) {
			return;
		}
	}

	public bool shouldTrigger(Collider other)
	{
		// Its already been used.
		if (singleUse && triggered) {
			return false;
		}

		// It hasn't cooled down yet.
		if (!singleUse && triggered) {
			return false;
		}

		// It didn't match the tag filter.
		if (useTagFilter) {
			if (!allowedTags.Contains(other.tag)) {
				return false;
			}
		}

		// It passed all the checks
		return true;
	}
}