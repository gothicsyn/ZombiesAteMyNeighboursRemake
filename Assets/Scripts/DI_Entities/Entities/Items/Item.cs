// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: This script is to detect when the player has walked on the item,
//		when they have a sound will be played and then the item will be destroyed
//

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(DI_Game.RotateObject))]
public class Item : NetworkBehaviour
{
	public Items item;
    public WeaponType weaponType;
    
    public AudioClip pickupSound;
	public float pickupVolume;
	public float deSpawnTime = 60.0f;
	public bool canDeSpawn = true;

    public float amount;
    public float duration = 0;

    //Debuffs
    [HideInInspector]
    public Vector3 warpLocation;

	public void OnEnable()
	{
		if (canDeSpawn) {
			StartCoroutine("deSpawn");
		}

        if (item == Items.DEBUFF_WARP)
        {
            warpLocation = new Vector3(Random.Range(-500, 500), 0, Random.Range(-500, 500));
        }

		DI_Events.EventCenter<Item>.invoke("OnDrop", this);
	}

	public void forcedPickup(PlayerState player)
	{
		DI_Events.EventCenter<Item, PlayerState>.invoke("OnPickupItem", this, player);
		DI_Events.EventCenter<AudioClip, float>.invoke("OnPlayEffect", pickupSound, pickupVolume);
		gameObject.SetActive(false);
	}

	public void forcedDespawn()
	{
		DI_Events.EventCenter<Item>.invoke("OnDespawn", this);
		gameObject.SetActive(false);
	}

	public IEnumerator deSpawn()
	{
		yield return new WaitForSeconds(deSpawnTime);
		DI_Events.EventCenter<Item>.invoke("OnDespawn", this);
		gameObject.SetActive(false);
	}

	public void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			PlayerState player = other.gameObject.GetComponent<PlayerState>();
            PlayerPickedUp(player);
			DI_Events.EventCenter<Item, PlayerState>.invoke("OnPickupItem", this, player);
			DI_Events.EventCenter<AudioClip, float>.invoke("OnPlayEffect", pickupSound, pickupVolume);
			gameObject.SetActive(false);
		}
	}

    public virtual void PlayerPickedUp(PlayerState playerState)
    {
        string itemString = item.ToString();
        string[] splitString = itemString.Split("_"[0]);
        switch (splitString[0])
        {
            case "AMMO":
                DI_Events.EventCenter<WeaponType, float, PlayerState>.invoke("OnPickupAmmo", weaponType, amount, playerState);
                break;
            case "DEBUFF":
                DI_Events.EventCenter<int, Items, Item>.invoke("OnPickupDebuff", playerState.player, item, this);
                break;
            case "ITEM":
                DI_Events.EventCenter<Item, PlayerState, float>.invoke(itemString, this, playerState, amount);
                break;
            case "POWERUP":
                DI_Events.EventCenter<int, Items, Item>.invoke("OnPickupBuff", playerState.player, item, this);
                break;
            default:
                break;
        }
    }
}