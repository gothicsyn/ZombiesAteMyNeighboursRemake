// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
// TODO: Include a description of the file here.
//

using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class NetworkOptions : MonoBehaviour
{
	// This is a temporary hack to get network games launchable from the main menu.
	// This needs to be done properly after testing is completed.

	public int port = 25000;
	public int cheatsEnabled;
	public int gameType;
	public int lobbyType;
	public int maxPlayers = 4;
	public int difficulty;
	public List<GameObject> objectsToDestroy;

	public void setPort(string port)
	{
		this.port = int.Parse(port);
	}

	public void setCheats(int cheatsEnabled)
	{
		this.cheatsEnabled = cheatsEnabled;
	}

	public void setGameType(int gameType)
	{
		this.gameType = gameType;
	}

	public void setLobbyType(int lobbyType)
	{
		this.lobbyType = lobbyType;
	}

	public void setMaxPlayers(int maxPlayers)
	{
		// TODO fix this.
		this.maxPlayers = (4 - maxPlayers);
	}

	public void setDifficulty(int difficulty)
	{
		this.difficulty = difficulty;
	}

	public void hostGame()
	{
		foreach (GameObject obj in objectsToDestroy.ToArray())
		{
			Destroy(obj);
		}

		NetworkManager.singleton.networkAddress = "localhost";
		NetworkManager.singleton.networkPort = port;
		NetworkManager.singleton.maxConnections = maxPlayers;
		NetworkManager.singleton.StartHost();
		NetworkManager.singleton.StartClient();
	}

	public void joinGame()
	{
	}

	public void debugJoinGame()
	{
		NetworkManager.singleton.networkAddress = "localhost";
		NetworkManager.singleton.networkPort = 7777;
		NetworkManager.singleton.StartClient();
	}
}