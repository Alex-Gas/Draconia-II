using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;


// this is an item shell which accomodates itemData. 
// the shell holds all functional mechanics while itemData provides data of the item such as name, id, effects it has etc
public class EquippableItem : Item, IItem
{
    public EquippableItem(ItemData data)
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
            if (!itemData.isEquipped)
            {
                manager.UnequipItemsOfType(itemData.equippableType);
                EquipItem(owner);
            }

            else if (itemData.isEquipped)
            {
                UnequipItem(owner);
            }
        }

        else{
            Debug.LogError("Item owner not defined!");
        }
    }

    public void EquipItem(CharacterBehaviour owner)
    {
        //Debug.Log("Equipping item of name: " + itemData.name);
        itemData.isEquipped = true;
        ApplyItemEffect(owner, itemData.onEquipEffects);
    }

    public void UnequipItem(CharacterBehaviour owner)
    {
        //Debug.Log("Unequipping item of name: " + itemData.name);
        itemData.isEquipped = false;
        ApplyItemEffect(owner, itemData.onUnequipEffects);
    }

    public new void RemoveItem(CharacterBehaviour owner)
    {
        if (itemData.isEquipped)
        {
            UnequipItem(owner);
        }
        base.RemoveItem(owner);
    }
}
