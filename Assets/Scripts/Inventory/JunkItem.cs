using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkItem : Item, IItem
{
    public JunkItem(ItemData data)
    {
        // make sure if by reference or value
        this.itemData = data;
    }

    public new void RemoveItem(CharacterBehaviour owner)
    {
        base.RemoveItem(owner);
    }
}
