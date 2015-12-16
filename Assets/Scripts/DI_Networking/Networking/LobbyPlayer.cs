// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
// TODO: Include a description of the file here.
//

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;

public class LobbyPlayer : UnityEngine.Networking.NetworkLobbyPlayer
{

	public List<RectTransform> playerPositions;

	// cached components
	NetworkLobbyPlayer lobbyPlayer;
	
	void Awake()
	{
		lobbyPlayer = GetComponent<NetworkLobbyPlayer>();
	}
	
	public override void OnClientEnterLobby()
	{
	}
	
	public override void OnClientExitLobby()
	{
	}
	
	public override void OnClientReady(bool readyState)
	{
		this.readyToBegin = readyState;
	}
	
	float GetPlayerPos(int slot)
	{
		return -158 + (slot - 1 * 105);
	}
	
	public override void OnStartLocalPlayer()
	{
		this.GetComponent<RectTransform>().localPosition = playerPositions[lobbyPlayer.playerControllerId].localPosition;
	}
	
	void OnDestroy()
	{
		Destroy(this);
	}
	
	public void SetColor(Color color)
	{
	}
	
	public void SetReady()
	{
		lobbyPlayer.readyToBegin = !lobbyPlayer.readyToBegin;
	}
	
	[Command]
	public void CmdExitToLobby()
	{
		var lobby = NetworkManager.singleton as NetworkLobbyManager;
		if (lobby != null)
		{
			lobby.ServerReturnToLobby();
		}
	}
	
	// events from UI system
	
	void OnGUIColorChange()
	{
	}
	
	void OnGUIReady()
	{
		if (isLocalPlayer)
			lobbyPlayer.SendReadyToBeginMessage();
	}
	
	void OnGUIRemove()
	{
	}
}