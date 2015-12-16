// Devils Inc Studios
// // Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Menus/Options/Enemies On Screen")]
public class EnemiesOnScreen : MonoBehaviour
{
	public float amount = 25.0f;
	public Text label;
	public Slider slider;

	public void OnEnable()
	{
		amount = PlayerPrefs.GetFloat("Enemies On Screen", 50);
		label.text = "Enemies On Screen: " + amount;
		slider.value = amount;
	}
	
	public void OnChange(float newAmount)
	{
		PlayerPrefs.SetFloat("Enemies On Screen", newAmount);
		label.text = "Enemies On Screen: " + newAmount;
		amount = newAmount;
	}
}