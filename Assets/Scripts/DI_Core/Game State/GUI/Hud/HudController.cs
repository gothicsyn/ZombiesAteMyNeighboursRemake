// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

#region Includes
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
#endregion

[AddComponentMenu("Game State/Hud Controller")]
public class HudController : MonoBehaviour
{
	#region Public Variables
	public List<HudElements> hudElements;
	public Sprite hudSelected;
	public Sprite hudNormal;
	public bool displayWave = false;
	[HideInInspector]
	public static HudController instance;
	#endregion

	#region Public Methods
	public void OnEnable()
	{
		instance = this;
		DI_Events.EventCenter<PlayerState>.addListener("OnUpdateHudRequest", handleUpdateHudRequest);
	}

	public void OnDisable()
	{
		DI_Events.EventCenter<PlayerState>.removeListener("OnUpdateHudRequest", handleUpdateHudRequest);
	}

	#endregion

	#region Private Methods
	/// <summary>
	/// Updates the selected weapon.
	/// </summary>
	/// <param name="playerState">Player state.</param>
	private void updateSelected(PlayerState playerState)
	{
		hudElements[(playerState.player - 1)].slot1.sprite = hudNormal;
		hudElements[(playerState.player - 1)].slot2.sprite = hudNormal;
		hudElements[(playerState.player - 1)].slot3.sprite = hudNormal;
		hudElements[(playerState.player - 1)].slot4.sprite = hudNormal;
		hudElements[(playerState.player - 1)].slot5.sprite = hudNormal;

		switch (playerState.getSelectedWeapon()) {
			case WeaponType.MAIN_GUN:
				hudElements[(playerState.player - 1)].slot1.sprite = hudSelected;
			break;
			case WeaponType.LASER_SHOT:
				hudElements[(playerState.player - 1)].slot2.sprite = hudSelected;
			break;
			case WeaponType.TRACKING_SHOT:
				hudElements[(playerState.player - 1)].slot3.sprite = hudSelected;
			break;
			case WeaponType.DRONE:
				hudElements[(playerState.player - 1)].slot4.sprite = hudSelected;
			break;
			case WeaponType.SHOCKWAVE:
				hudElements[(playerState.player - 1)].slot5.sprite = hudSelected;
			break;
		}
	}

	/// <summary>
	/// Updates the hud.
	/// </summary>
	/// <param name="playerState">Player state.</param>
	private void updateHud(PlayerState playerState)
	{
		hudElements[(playerState.player - 1)].score.text = "Score: " + ScoreController.instance.getFinalScore(playerState.player);
		if (displayWave) {
			hudElements[(playerState.player - 1)].wave.text = "Wave: " + WaveController.instance.currentWave;
		}
		hudElements[(playerState.player - 1)].enemies.text = "Enemies: " + WaveController.instance.getEnemyCount();
		hudElements[(playerState.player - 1)].slot1Ammo.text = playerState.getAmmo(WeaponType.MAIN_GUN).ToString();
		hudElements[(playerState.player - 1)].slot2Ammo.text = playerState.getAmmo(WeaponType.LASER_SHOT).ToString();
		hudElements[(playerState.player - 1)].slot3Ammo.text = playerState.getAmmo(WeaponType.TRACKING_SHOT).ToString();
		hudElements[(playerState.player - 1)].slot4Ammo.text = playerState.getAmmo(WeaponType.DRONE).ToString();
		hudElements[(playerState.player - 1)].slot5Ammo.text = playerState.getAmmo(WeaponType.SHOCKWAVE).ToString();
	}

	/// <summary>
	/// Handles the update hud request.
	/// </summary>
	/// <param name="playerState">Player state.</param>
	private void handleUpdateHudRequest(PlayerState playerState)
	{
		if ((playerState.player - 1) < hudElements.Count) {
			updateHud(playerState);
			updateSelected(playerState);
		}
		else {
			#if DEBUG
				Debug.Log("Requested to update hud for player: " + playerState.player + " but that player does not have hud elements configured.");
			#endif
		}
	}
	#endregion
}
