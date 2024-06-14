using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : EntityBehaviour, IInteractable
{
    public ItemData itemData = new ItemData();
    public int manualID;

    public new void Start()
    {
        // if there is already a data setup prior to Start() running that means it was spawned in game (dropped) meaning it carries itemData over from the source of drop.
        // if the id is 0 that means the item wasn't dropped but instead spawned in inspector meaning it needs to use manualID (assigned in inspector) to know what item to actually spawn in.
        // if the id is 0 and the manualID has not been assigned in inspector it will spawn an item of id = 0 meaning default placeholder item
        if (itemData.ID == 0)
        {
            itemData = ItemLibrary.LibraryItemRetriever(manualID);
        }

        base.Start();
    }

    public void TransfuseItemData(ItemData originalData)
    {
        itemData = new ItemData(originalData);
    }


    // what happens to item data due to being picked up
    public void OnPickup()
    {
    }

    // what happens to item due to being dropped
    public void OnDrop()
    {
        // unequip item
        itemData.isEquipped = false;
    }

    public void OnInteract(PlayerBehaviour entity)
    {
        //Debug.Log("called for item pickup");

        itemData.isEquipped = false;

        if (entity.PickupItem(itemData))
        {
            OnPickup();
            Destroy(gameObject);
        }
    }
}
