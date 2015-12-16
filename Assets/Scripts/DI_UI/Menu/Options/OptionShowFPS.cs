// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014
//
// TODO: Include a description of the file here.
//

using UnityEngine;

[AddComponentMenu("Menu/Options/Show FPS")]
public class OptionShowFPS : MonoBehaviour
{
	public void setFPSVisability(bool visable)
	{
		FPSCounter.instance.isEnabled = visable;
		FPSCounter.instance.fpsDisplay.gameObject.SetActive(visable);
	}
}
