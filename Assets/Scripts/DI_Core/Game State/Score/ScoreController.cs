// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Handles all the score routes to allow the player to gain
//	 	score to buy things from the shop
//

#region includes
using UnityEngine;
using DI_Events;
using System.Collections.Generic;
using UnityEngine.Networking;
#endregion

[AddComponentMenu("Game State/Score")]
public class ScoreController : MonoBehaviour
{
	#region Events Used
	/*
	 * Listening:
	 * OnDeath
	 * OnPickupItem
	 * OnStarDamage
	 * 
	 * Sending:
	 * OnUpdateHudRequest
	 *
	 */
	#endregion

	#region Public Vars
	// Amount of points lost per 1 point of damage to the star.
	public float starHealthDeduction = 10.0f;

	// A List of score objects, stored per player.
	public List<Score> score;
	[HideInInspector]
	public static ScoreController instance;
	#endregion

	#region Public Methods
	public void OnEnable()
	{
		instance = this;
		// Register to the OnDeath event.
		EventCenter<Entity, Entity>.addListener("OnDeath", handleKillEnemy);
		// Register to the OnPickupItem event.
		EventCenter<Item, PlayerState>.addListener("OnPickupItem", handlePickupItem);
		// Register to the OnStarDamage event.
		EventCenter<float>.addListener("OnStarDamage", handleStarDamage);
	}

	public void OnDisable()
	{
		// Remove the OnDeath listener.
		EventCenter<Entity, Entity>.removeListener("OnDeath", handleKillEnemy);
		// Remove the OnPickupItem listener.
		EventCenter<Item, PlayerState>.removeListener("OnPickupItem", handlePickupItem);
		// Remove the OnStarDamage listener.
		EventCenter<float>.removeListener("OnStarDamage", handleStarDamage);
	}

	/// <summary>
	/// Handles the kill enemy request.
	/// </summary>
	/// <param name="enemy">Enemy.</param>
	/// <param name="player">Player.</param>
	/// <remarks>
	/// Invoked by enemies in their OnDeath event handlers.
	/// </remarks>
	public void handleKillEnemy(Entity enemy, Entity player)
	{
		// Otherwise its an enemy killing a tower.
//		if (player.tag == "Player") {
//			PlayerState playerState = ((PlayerEntity)player).playerState;
//			Enemy _enemy = (Enemy)enemy;
//			score[playerState.player - 1].enemiesKilled[(int)_enemy.type] += 1;
//			score[playerState.player - 1].points += _enemy.points * WaveController.instance.waveMultiplier;
//			DI_Events.EventCenter<PlayerState>.invoke("OnUpdateHudRequest", playerState);
//		}
	}

	/// <summary>
	/// Handles the pickup of items.
	/// </summary>
	/// <param name="item">Item.</param>
	/// <param name="player">Player.</param>
	/// <remarks>
	/// Called when an player enters the trigger collider of an item pickup.
	/// </remarks>
	public void handlePickupItem(Item item, PlayerState playerState)
	{
//		if (item.type == Items.ITEM_COIN) {
//			score[playerState.player - 1].coinsCollected += item.amount;
//			score[playerState.player - 1].points += item.amount;
//			DI_Events.EventCenter<PlayerState>.invoke("OnUpdateHudRequest", playerState);
//		}
	}

	/// <summary>
	/// Handles the star damage.
	/// </summary>
	/// <param name="damage">Damage.</param>
	/// <remarks>
	/// Called by the star when it gets damaged.
	/// </remarks>
	public void handleStarDamage(float damage)
	{
		for (int index = 0; index < score.Count; ++index) {
			score[index].damageToStar += damage;
			DI_Events.EventCenter.invoke("OnUpdateHudRequest");
		}
	}

	/// <summary>
	/// Gets the final score.
	/// </summary>
	/// <returns>The final score.</returns>
	/// <param name="player">Player.</param>
	/// <returns>Returns the final score for the requested played which is points - damage dealt to the star * star damage penalty</returns>
	public float getFinalScore(int player)
	{
		return (score[player - 1].points - (score[player - 1].damageToStar * starHealthDeduction));
	}
	#endregion
}