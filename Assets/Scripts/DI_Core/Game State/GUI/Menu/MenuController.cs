// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;

[AddComponentMenu("Game State/Menu Controller")]
public class MenuController : MonoBehaviour
{
	public Canvas menu;
	public Canvas pause;
	public Canvas options;
	public Canvas hud;
	public Canvas cheatMenu;
	[HideInInspector]
	public static MenuController instance;

	public void OnEnable()
	{
		instance = this;
	}

	public void disableAllMenus()
	{
		menu.gameObject.SetActive(false);
		pause.gameObject.SetActive(false);
		cheatMenu.gameObject.SetActive(false);
		hud.gameObject.SetActive(true);
		options.gameObject.SetActive(false);
	}

	public void onCheatMenuEnter()
	{
		disableAllMenus();
		cheatMenu.gameObject.SetActive(true);
		GameStateController.instance.enterMenu();
	}

	public void onCheatMenuExit()
	{
		disableAllMenus();
		GameStateController.instance.exitMenu();
	}
	
	public void onMenuEnter()
	{
		disableAllMenus();
		menu.gameObject.SetActive(true);
		GameStateController.instance.enterMenu();
	}

	public void onOptionsMenuEnter()
	{
		disableAllMenus();
		options.gameObject.SetActive(true);
		GameStateController.instance.enterMenu();
	}
	
	public void onMenuExit()
	{
		disableAllMenus();
		GameStateController.instance.exitMenu();
	}

	public void onPause()
	{
		disableAllMenus();
		pause.gameObject.SetActive(true);
		GameStateController.instance.onPause();
	}

	public void onResume()
	{
		disableAllMenus();
		GameStateController.instance.onResume();
	}
}
