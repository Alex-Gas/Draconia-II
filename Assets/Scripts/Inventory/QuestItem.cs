using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : Item, IItem
{
    public QuestItem(ItemData data)
    {
        // make sure if by reference or value
        this.itemData = data;
    }

    public new void RemoveItem(CharacterBehaviour owner)
    {
        base.RemoveItem(owner);
    }
}
