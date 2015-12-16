// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;

[AddComponentMenu("Interactions/Highlight")]
public class HighlightObject : MonoBehaviour
{
	private Material oldMaterial;
	public Material highlightedMaterial;
	public bool highlighted;
	[HideInInspector]
	public float maxDistance = 3.0f;
	private float lastHit = 0.0f;

	public void OnEnable()
	{
		oldMaterial = GetComponent<Renderer>().material;
		if (highlighted)
		{
			if (highlightedMaterial != null) {
				GetComponent<Renderer>().material = highlightedMaterial;
			}
		}
	}

	public void Update()
	{
		if (highlighted) {
			if (lastHit < 0.1f) {
				lastHit += Time.deltaTime;
			}
			else {
				highlighted = false;
				updateHighlight();
			}
		}
	}

	public void OnRayHit()
	{
		if (this.enabled) {
			if (!highlighted) {
				float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
				if (distance <= maxDistance) {
					highlighted = true;
					updateHighlight();
				}
			}
			lastHit = 0.0f;
		}
		else {
			highlighted = false;
			updateHighlight();
		}
	}

	public void updateHighlight()
	{
		if (highlighted)
		{
			GetComponent<Renderer>().material = highlightedMaterial;
		}
		else {
			GetComponent<Renderer>().material = oldMaterial;
		}
	}
}