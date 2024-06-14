using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : IItem
{
    public ItemData itemData { get; set; } = new ItemData();


    public void OnClick(CharacterBehaviour owner, InventoryManager manager)
    {
        //Debug.Log("item clicked: " + itemData.name);
    }

    protected void ApplyItemEffect(CharacterBehaviour owner, List<ItemData.Action> effectsList)
    {
        //Debug.Log("Attemptint to apply item effects... effects in list: " + effectsList.Count);
        // unpack and execute all effects in the list on the owner of the item
        foreach (ItemData.Action action in effectsList)
        {
            //Debug.Log("Invoking item action delegate...");
            action.Invoke(owner);
        }
    }

    public void RemoveItem(CharacterBehaviour owner)
    {
        // since this itemData is a reference to the original and not a copy we can just remove it directly making this itemData null but not destroying this object
        // we dont need to destroy the object because InventoryUI redraw is called immediately which will redraw items destroying this object
        owner.itemsInPosession.Remove(itemData);
        //Debug.Log("Item removed from posession");
    }

    public string GetItemName() 
    { 
        return itemData.name;
    }

    public string GetDescription()
    {
        return itemData.description;
    }

    public ItemStats GetStats()
    {
        return itemData.displayStats;
    }
}
