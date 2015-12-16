using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerStatusController : MonoBehaviour {


    /*	
    DEBUFF_FUEL,
	DEBUFF_WEAPON_TARGETING,
	POWERUP_MULTISHOT,
	POWERUP_DRONE_SWARM,
	POWERUP_REVIVE_TEAMMATE,
	POWERUP_MULIPLIER_UP,
    */

    Dictionary<Items, Item> buffItems = new Dictionary<Items, Item>();
    Dictionary<Items, Item> debuffItems = new Dictionary<Items, Item>();

    int playerID;
    PlayerEntity playerEntity;
    // Use this for initialization
    void Start () {
        playerEntity = gameObject.GetComponent<PlayerEntity>();
        playerID = playerEntity.playerState.player;
        DI_Events.EventCenter<int, Items, Item>.addListener("OnPickupBuff", HandleBuffPickup);
        DI_Events.EventCenter<int, Items, Item>.addListener("OnPickupDebuff", HandleDebuffPickup);
    }

    // Update is called once per frame
    public void OnDisable()
    {
        DI_Events.EventCenter<int, Items, Item>.removeListener("OnPickupBuff", HandleBuffPickup);
        DI_Events.EventCenter<int, Items, Item>.removeListener("OnPickupDebuff", HandleDebuffPickup);
    }

    public void HandleBuffType()
    {

    }

    public void HandleBuffPickup(int player, Items buffType, Item item)
    {
        if(playerID == player)
        {
            DI_Events.EventCenter<Entity, Item>.invoke(buffType.ToString(), playerEntity, item);
            if (!buffItems.ContainsKey(buffType))
            {
                buffItems.Add(buffType, item);
            }
            else
            {
                buffItems.Remove(buffType);
                buffItems.Add(buffType, item);
            }
        }
    }

    public void HandleDebuffPickup(int player, Items debuffType, Item item)
    {
        if (playerID == player)
        {
            DI_Events.EventCenter<Entity, Item>.invoke(debuffType.ToString(), playerEntity, item);
            if (!debuffItems.ContainsKey(debuffType))
            {
                debuffItems.Add(debuffType, item);
            }
            else
            {
                debuffItems.Remove(debuffType);
                debuffItems.Add(debuffType, item);
            }
        }
    }
}
