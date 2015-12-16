// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

#region Includes
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
#endregion

//[RequireComponent(typeof(PlayerEntity))]
[AddComponentMenu("Game State/Player")]
public class PlayerState : MonoBehaviour
{
	
	#region Events Used
	/*
	 * Listening:
	 * OnPickupAmmo
	 * OnPickupItem
	 * 
	 * Sending:
	 * OnUpdateHudRequest
	 *
	 */
	#endregion

	#region Private Variables
	[SerializeField]
	[Header("Inventory")]
	private PlayerInventory playerInventory;
	#endregion

	#region Public Variables
	[Header("Player Number")]
	public int player;

	[Header("Debug / Cheat Settings")]
	public bool unlimitedAmmo = false;
    public float unlimitedAmmoTime = 0.0f;
	#endregion

	#region References
	[HideInInspector]
	public PlayerEntity entity;

	#endregion

	#region Public Methods
	public void OnEnable()
	{
		entity = this.GetComponent<PlayerEntity>();

		// Register the OnPickupAmmo event
		DI_Events.EventCenter<WeaponType, float, PlayerState>.addListener("OnPickupAmmo", handlePickupAmmo);
		// Register to the OnPickupItem event.
		DI_Events.EventCenter<Item, PlayerState, float>.addListener("ITEM_HULL_REPAIR", handlePickupHealth);
        DI_Events.EventCenter<Item, PlayerState, float>.addListener("ITEM_SHIELD_REPAIR", handlePickupShield);
	
        DI_Events.EventCenter<Entity, Item>.addListener("POWERUP_UNLIMTED_AMMO", handleUnlimitedAmmo);

        DI_Events.EventCenter<Entity, Item>.addListener("DEBUFF_WARP", handleWarpPlayer);
        DI_Events.EventCenter<Entity, Item>.addListener("DEBUFF_SHIELD", handleShieldDebuff);

        // Register with the player controller.
        PlayerController.register(this);
	}

	public void OnDisable()
	{
		// Unregister the OnPickupAmmo event
		DI_Events.EventCenter<WeaponType, float, PlayerState>.removeListener("OnPickupAmmo", handlePickupAmmo);
		// Unregister the OnPickupItem event
        DI_Events.EventCenter<Item, PlayerState, float>.removeListener("ITEM_HULL_REPAIR", handlePickupHealth);
        DI_Events.EventCenter<Item, PlayerState, float>.removeListener("ITEM_SHIELD_REPAIR", handlePickupShield);

        DI_Events.EventCenter<Entity, Item>.removeListener("POWERUP_UNLIMTED_AMMO", handleUnlimitedAmmo);

        DI_Events.EventCenter<Entity, Item>.removeListener("DEBUFF_WARP", handleWarpPlayer);
        DI_Events.EventCenter<Entity, Item>.removeListener("DEBUFF_SHIELD", handleShieldDebuff);

        // Deregister from the player controller
        PlayerController.deregister(this);
	}

    public void Update()
    {
        if (unlimitedAmmo)
        {
            unlimitedAmmoTime -= Time.deltaTime;
            if (unlimitedAmmoTime <= 0)
            {
                unlimitedAmmo = false;
                unlimitedAmmoTime = 0;
            }
        }
    }

    /// <summary>
    /// Handles the pickup of items.
    /// </summary>
    /// <param name="item">Item.</param>
    /// <param name="playerId">Player identifier.</param>
    /// <remarks>
    /// Called by items when the player enters the trigger zone.
    /// Ignores the event if the playerId does not match with our playerId
    /// Ignores items that are not coins, if we add more items we will need to update this.
    /// </remarks>
    /// 
    /// TODO This isn't just for coins.
    public void handlePickupHealth(Item item, PlayerState playerState, float amount)
    {
        if (playerState.player == player)
        {
            entity.health += amount;
            if (entity.health > entity.maxHealth)
            {
                entity.health = entity.maxHealth;
            }
        }
    }

    public void handlePickupShield(Item item, PlayerState playerState, float amount)
    {
        if (playerState.player == player)
        {
            entity.shield += amount;
            if(entity.shield > entity.maxShield)
            {
                entity.shield = entity.maxShield;
            }
        }
    }

    void handleShieldDebuff(Entity target, Item item)
    {
        if(target == entity)
        {
            target.shield -= target.baseShield / 2;
        }
    }

    public void handleWarpPlayer(Entity target, Item item)
    {
        if (target == entity)
        {
            gameObject.transform.position = item.warpLocation;
        }
    }

    /// <summary>
    /// Handles the pickup of ammo.
    /// </summary>
    /// <param name="ammoBox">Ammo box.</param>
    /// <param name="playerId">Player identifier.</param>
    /// <remarks>
    /// Called by ammo boxes when the player enters the trigger zone.
    /// Ignores the event if the playerId does not match with our playerId
    /// </remarks>
    public void handlePickupAmmo(WeaponType weaponType, float amount, PlayerState playerState)
	{
		if (playerState.player == player) {
			addAmmo(weaponType, amount);
		}
	}

    public void handleUnlimitedAmmo(Entity target, Item item)
    {
        Debug.Log("Yes");
        if (target = entity)
        {
            Debug.Log("Yes2");
            unlimitedAmmo = true;
        }
    }

    /// <summary>
    /// Gets the max ammo.
    /// </summary>
    /// <returns>The max ammo.</returns>
    /// <param name="type">Type.</param>
    public float getMaxAmmo(WeaponType type)
	{
		foreach (Ammo inventorySlot in playerInventory.maxAmmo.ToArray()) {
			if (inventorySlot.weaponType == type) {
				return inventorySlot.ammoCount;
			}
		}
		#if DEBUG
			Debug.LogError("Max ammo amount requested on type that does not exist.");
		#endif
		return 0;
	}

	/// <summary>
	/// Gets the ammo type identifier.
	/// </summary>
	/// <returns>The ammo type identifier.</returns>
	/// <param name="type">Type.</param>
	public int getWeaponInventoryId(WeaponType type)
	{
		for (int index = 0; index < playerInventory.playerAmmo.Count; ++index) {
			if (playerInventory.playerAmmo[index].weaponType == type) {
				return index;
			}
		}
		return -1;
	}

	/// <summary>
	/// Sets the ammo.
	/// </summary>
	/// <param name="newAmmo">New ammo.</param>
	public void setAmmo(WeaponType type, float amount)
	{
		int index = getWeaponInventoryId(type);
		playerInventory.playerAmmo[index] = new Ammo(type, amount);
		DI_Events.EventCenter<PlayerState>.invoke("OnUpdateHudRequest", this);
	}

	/// <summary>
	/// Removes the ammo.
	/// </summary>
	/// <param name="newAmmo">New ammo.</param>
	public void removeAmmo(WeaponType type, float amount)
	{
		if (!unlimitedAmmo) {
			int index = getWeaponInventoryId(type);
			playerInventory.playerAmmo[index] = new Ammo(type, playerInventory.playerAmmo[index].ammoCount - amount);
			if (playerInventory.playerAmmo[index].ammoCount < 0) {
				playerInventory.playerAmmo[index] = new Ammo(type, 0);
			}
			DI_Events.EventCenter<PlayerState>.invoke("OnUpdateHudRequest", this);
		}
	}

	/// <summary>
	/// Adds the ammo.
	/// </summary>
	/// <param name="newAmmo">New ammo.</param>
	public void addAmmo(WeaponType type, float amount)
	{
		int index = getWeaponInventoryId(type);
		// Clamp the ammo to the max allowed.
		float newAmmo = Mathf.Clamp(playerInventory.playerAmmo[index].ammoCount + amount, 0, getMaxAmmo(type));
		playerInventory.playerAmmo[index] = new Ammo(type, newAmmo);
		DI_Events.EventCenter<PlayerState>.invoke("OnUpdateHudRequest", this);
	}

	/// <summary>
	/// Gets the ammo.
	/// </summary>
	/// <returns>The ammo.</returns>
	/// <param name="type">Type.</param>
	public float getAmmo(WeaponType type) {
		int index = getWeaponInventoryId(type);
		return playerInventory.playerAmmo[index].ammoCount;
	}

	/// <summary>
	/// Sets the selected weapon.
	/// </summary>
	/// <param name="type">Type.</param>
	public void setSelectedWeapon(WeaponType type)
	{
		playerInventory.selectedWeapon = type;
		DI_Events.EventCenter<PlayerState>.invoke("OnUpdateHudRequest", this);
	}

	/// <summary>
	/// Gets the selected weapon.
	/// </summary>
	/// <returns>The selected weapon.</returns>
	public WeaponType getSelectedWeapon()
	{
		return playerInventory.selectedWeapon;
	}

	public void selectNextWeapon()
	{
		int weaponIndex = (int)playerInventory.selectedWeapon;
		++weaponIndex;
		if (weaponIndex > (playerInventory.maxAmmo.Count -1)) {
			weaponIndex = 0;
		}
		setSelectedWeapon(playerInventory.maxAmmo[weaponIndex].weaponType);
	}

	public void selectPreviousWeapon()
	{
		int weaponIndex = (int)playerInventory.selectedWeapon;
		--weaponIndex;
		if (weaponIndex < 0) {
			weaponIndex = (playerInventory.maxAmmo.Count -1);
		}
		setSelectedWeapon(playerInventory.maxAmmo[weaponIndex].weaponType);
	}

	public void selectNextItem()
	{
	}
	public void selectPreviousItem()
	{
	}

	#endregion
}
