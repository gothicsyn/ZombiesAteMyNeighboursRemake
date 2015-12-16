// Devils Inc Studios
// // Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using System.Collections.Generic;

public class EnableOptions : MonoBehaviour
{
	public EnterCheatMode cheatMenu;
	public List<GameObject> menuItems;
	public List<GameObject> disableOptions;

	public void OnEnable()
	{
		if (cheatMenu.codeEntered) {
			foreach (GameObject item in menuItems.ToArray()) {
				item.SetActive(true);
			}
			if (disableOptions != null) {
				foreach (GameObject item in disableOptions.ToArray()) {
					item.SetActive(false);
				}
			}
		}
		else {
			foreach (GameObject item in menuItems.ToArray()) {
				item.SetActive(false);
			}
			if (disableOptions != null) {
				foreach (GameObject item in disableOptions.ToArray()) {
					item.SetActive(true);
				}
			}
		}
	}
}