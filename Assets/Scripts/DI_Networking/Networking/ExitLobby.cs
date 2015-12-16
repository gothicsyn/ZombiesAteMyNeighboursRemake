// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
// TODO: Include a description of the file here.
//

using UnityEngine.Networking;
using UnityEngine;

public class ExitLobby : MonoBehaviour
{
	public string mainMenuScene;
	public string lobbyScene;

	public void exitLobby()
	{
		var lobby = NetworkManager.singleton as NetworkLobbyManager;
		if (lobby != null)
		{
			lobby.lobbyScene = mainMenuScene;
			lobby.ServerReturnToLobby();
			lobby.StopHost();
			lobby.lobbyScene = lobbyScene;
		}
	}
}