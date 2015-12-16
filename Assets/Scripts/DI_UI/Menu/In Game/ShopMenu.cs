using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("Menus/In Game/Shop Menu")]
public class ShopMenu : MonoBehaviour {

	public void OpenShopMenu(){
		Canvas shopCanvas = GameObject.Find("Shop Menu").GetComponent<Canvas>();
		Canvas mainMenu = GameObject.Find("Game Menu").GetComponent<Canvas>();
	
		shopCanvas.enabled = true;
		mainMenu.enabled = false;
	}
}
