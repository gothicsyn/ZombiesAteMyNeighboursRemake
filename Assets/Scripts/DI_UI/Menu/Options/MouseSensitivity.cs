// Devils Inc Studios
// // Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Menus/Options/Mouse Sensitivity")]
public class MouseSensitivity : MonoBehaviour
{
	public float sensitivity = 7.0f;
	public Text label;
	public Slider slider;

	public void OnEnable()
	{
		sensitivity = PlayerPrefs.GetFloat("Mouse Sensitivity", 7);
		label.text = "Mouse Sensitivity: " + sensitivity;
		slider.value = sensitivity;
	}

	public void OnChange(float mouseSensitivity)
	{
		PlayerPrefs.SetFloat("Mouse Sensitivity", mouseSensitivity);
		label.text = "Mouse Sensitivity: " + sensitivity;
		sensitivity = mouseSensitivity;
	}
}