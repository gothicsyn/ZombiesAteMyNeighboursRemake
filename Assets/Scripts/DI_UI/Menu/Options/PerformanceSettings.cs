// Devils Inc Studios
// // Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Menus/Options/Performance Settings")]
public class PerformanceSettings : MonoBehaviour
{
	public int itemsOnScreen = 0;
	public int enemiesOnScreen = 0;
	public PerformanceNames performanceSetting;

	public void OnClick()
	{
		PlayerPrefs.SetFloat("Items On Screen", itemsOnScreen);
		PlayerPrefs.SetFloat("Enemies On Screen", enemiesOnScreen);
		PlayerPrefs.SetInt("Performance", (int) performanceSetting);
		DI_Events.EventCenter.invoke("OnOptionsChanged");
	}
}