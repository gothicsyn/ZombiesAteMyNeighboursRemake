// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Misc/FPS Counter")]
public class FPSCounter : MonoBehaviour
{
	[Header("Display Settings")]
	public Text fpsDisplay;
	private float deltaTime = 0.0f;
	public bool isEnabled = true;
	[HideInInspector]
	public static FPSCounter instance;

	public void Awake()
	{
		instance = this;
	}

	public void Update()
	{
		if (isEnabled) {
			deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
			float msec = deltaTime * 1000.0f;
			float fps = 1.0f / deltaTime;
			fpsDisplay.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		}
	}
}
