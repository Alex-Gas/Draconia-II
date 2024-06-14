using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    public ItemData itemData { get; set; }

    public void OnClick(CharacterBehaviour owner, InventoryManager manager);

    public void RemoveItem(CharacterBehaviour owner);

    public string GetItemName();
    public string GetDescription();

    public ItemStats GetStats(); 
    
}
