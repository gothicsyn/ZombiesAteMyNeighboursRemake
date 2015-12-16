// Devils Inc Studios
// // Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//
using UnityEngine;

public class CheatMenu : MonoBehaviour
{
	public GameStateController gsc;
	public MenuController menuController;

	public void OnEnable()
	{
		gsc = GameObject.Find("Game State").GetComponent<GameStateController>();
		menuController = GameObject.Find("Game State").GetComponent<MenuController>();
	}

	public void OnChangeWave(string waveNumber)
	{
		gsc.SendMessage("onCheatChangeWave", waveNumber, SendMessageOptions.RequireReceiver);
	}

	public void OnChangeCoins(string coins)
	{
		gsc.SendMessage("onCheatChangeCoins", coins, SendMessageOptions.RequireReceiver);
	}

	public void OnCheatKillAll()
	{
		gsc.SendMessage("onCheatKillAll", SendMessageOptions.RequireReceiver);
	}

	public void OnCheatChangeUnlimitedAmmo(bool unlimited)
	{
		gsc.SendMessage("onCheatChangeUnlimitedAmmo", unlimited, SendMessageOptions.RequireReceiver);
	}

	public void OnCheatChangeStarInvincible(bool invincible)
	{
		gsc.SendMessage("onCheatChangeStarInvincible", invincible, SendMessageOptions.RequireReceiver);
	}

	public void OnOpenMenu()
	{
		menuController.SendMessage("onCheatMenuEnter", SendMessageOptions.RequireReceiver);
	}

	public void OnCloseMenu()
	{
		menuController.SendMessage("onCheatMenuExit", SendMessageOptions.RequireReceiver);
	}
}
