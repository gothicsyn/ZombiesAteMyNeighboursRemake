// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using System;
using UnityEngine;
using System.Collections.Generic;

// TODO this needs to not be a static class so it can inherit from network behaviour and be used on the server
public static class PlayerController
{
	public static List<PlayerState> players = new List<PlayerState>();

	public static void register(PlayerState player)
	{
		if (!players.Contains(player)) {
			players.Add(player);
		}
	}

	public static void deregister(PlayerState player)
	{
		if (players.Contains(player)) {
			players.Remove(player);
		}
	}

	public static int getPlayerCount()
	{
		return players.Count;
	}

	public static PlayerState getPlayer(int playerNumber)
	{
		foreach (PlayerState player in players) {
			if (player.player == playerNumber) {
				return player;
			}
		}

		throw new ArgumentException("Player requested (" + playerNumber + ") is not registered.");
	}
}
