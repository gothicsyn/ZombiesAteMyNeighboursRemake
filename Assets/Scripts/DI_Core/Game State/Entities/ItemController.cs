// Devils Inc Studios// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ItemController : NetworkBehaviour
{
	public List<Item> spawnedItems;
	public float maxItemCount = 50.0f;
	public PlayerState player;

	public void OnEnable()
	{
		// Only run on the server
		if (!isServer) {
			this.enabled = false;
		}

		DI_Events.EventCenter<Item, PlayerState>.addListener("OnPickupItem", handleOnPickup);
		DI_Events.EventCenter<Item>.addListener("OnDespawn", handleOnDespawn);
		DI_Events.EventCenter<Item>.addListener("OnDrop", handleOnDrop);
		DI_Events.EventCenter<int>.addListener("OnWaveEnd", handleWaveEnd);
		DI_Events.EventCenter.addListener("OnOptionsChanged", handleOptionsChanged);
		spawnedItems = new List<Item>();
		player = GameObject.Find("Player One").GetComponent<PlayerState>();
		maxItemCount = PlayerPrefs.GetFloat("Items On Screen", 50.0f);
	}
	
	public void OnDisable()
	{
		DI_Events.EventCenter<Item, PlayerState>.removeListener("OnPickupItem", handleOnPickup);
		DI_Events.EventCenter<Item>.removeListener("OnDespawn", handleOnDespawn);
		DI_Events.EventCenter<Item>.removeListener("OnDrop", handleOnDrop);
		DI_Events.EventCenter<int>.removeListener("OnWaveEnd", handleWaveEnd);
		DI_Events.EventCenter.removeListener("OnOptionsChanged", handleOptionsChanged);
	}

	public void handleOptionsChanged()
	{
		maxItemCount = PlayerPrefs.GetFloat("Items On Screen", 50.0f);
	}

	public void handleWaveEnd(int wave)
	{
		foreach (Item _item in spawnedItems.ToArray()) {
			if (_item != null) {
				try {
					_item.forcedPickup(player);
				}
				catch (Exception) {
					spawnedItems.Remove(_item);
				}
			}
			else {
				spawnedItems.Remove(_item);
			}
		}

		foreach (GameObject _item in GameObject.FindGameObjectsWithTag("Item")) {
			Item itemScript = _item.GetComponent<Item>();
			if (itemScript != null) {
				itemScript.forcedPickup(player);
			}
		}
	}

	public void handleOnPickup(Item _item, PlayerState _player)
	{
		if (_item != null) {
			if (spawnedItems.Contains(_item)) {
				spawnedItems.Remove(_item);
			}
		}
	}

	// TODO Clean up snow references.
	public void handleOnDrop(Item _item)
	{
		if (_item != null) {
			if (!_item.name.Contains("Snow Pile")) {
				spawnedItems.Add(_item);
				if (spawnedItems.Count > maxItemCount) {
					if (spawnedItems.Count > 0) {
						if (spawnedItems[0] != null) {
							spawnedItems[0].forcedDespawn();
						}
						spawnedItems.Remove(spawnedItems[0]);
					}
				}
			}
		}
	}

	public void handleOnDespawn(Item _item)
	{
		if (spawnedItems.Contains(_item)) {
			spawnedItems.Remove(_item);
		}
	}
}
