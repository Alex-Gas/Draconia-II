using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item, IItem
{

    public ConsumableItem(ItemData data)
    {
        // make sure if by reference or value
        this.itemData = data;
    }

    // what happens when item is clicked in inventory
    public new void OnClick(CharacterBehaviour owner, InventoryManager manager)
    {
        base.OnClick(owner, manager);

        if (owner != null)
        {
            //Debug.Log("Consuming item of name: " + itemData.name);
            ApplyItemEffect(owner, itemData.onConsumeEffects);
            RemoveItem(owner);
        }

        else
        {
            Debug.LogError("Item owner not defined!");
        }
    }

    public new void RemoveItem(CharacterBehaviour owner)
    {
        base.RemoveItem(owner);
    }
}
