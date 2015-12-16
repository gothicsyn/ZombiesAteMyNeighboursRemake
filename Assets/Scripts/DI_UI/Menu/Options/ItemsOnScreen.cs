// Devils Inc Studios
// // Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Menus/Options/Items On Screen")]
public class ItemsOnScreen : MonoBehaviour
{
	public float amount = 50.0f;
	public Text label;
	public Slider slider;

	public void OnEnable()
	{
		amount = PlayerPrefs.GetFloat("Items On Screen", 50);
		label.text = "Items On Screen: " + amount;
		slider.value = amount;
	}

	public void OnChange(float newAmount)
	{
		PlayerPrefs.SetFloat("Items On Screen", newAmount);
		label.text = "Items On Screen: " + newAmount;
		amount = newAmount;
	}
}