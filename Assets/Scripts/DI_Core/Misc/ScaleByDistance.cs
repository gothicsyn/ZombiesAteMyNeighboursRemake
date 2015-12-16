// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Misc/Scale By Distance")]
public class ScaleByDistance : MonoBehaviour
{
	public float minScale;
	public float maxScale;
	public float currentScale;
	public float percentScale = 0.0f;
	public GameObject distanceObject;
	public bool useCamera = true;
	public float distanceMultiplier = 2.0f;
	public float distanceToTarget = 0.0f;

	public void OnEnable()
	{
		if (useCamera) {
			distanceObject = Camera.main.gameObject;
		}
	}

	public void Update()
	{
		currentScale = transform.localScale.x;
		percentScale = Mathf.Clamp((currentScale / maxScale) * 100, 0, 100);
		distanceToTarget = Vector3.Distance(transform.position, distanceObject.transform.position);
		percentScale = distanceToTarget * distanceMultiplier;
		currentScale = Mathf.Clamp(maxScale * (percentScale / 100), minScale, maxScale);
		this.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
	}
}