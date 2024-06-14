using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    // ACCORDING TO THIS AN ITEM (for inventory) AND ITEMBEHAVIOUR (for item objects) CAN BE CREATED
    // there must be a list of each item in the game all of which are instances of this class

    public int ID { get; set; }
    public long instanceID { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string prefabPath { get; set; }
    public string iconPath {  get; set; }
    public ItemType itemType { get; set; }
    public EquippableType equippableType { get; set; }
    public PlayerBehaviour player { get; set; }
    public bool isEquipped { get; set; }
    public ItemStats displayStats { get; set; } = new();


    public delegate void Action(CharacterBehaviour owner);
    public List<Action> onEquipEffects = new List<Action>();
    public List<Action> onUnequipEffects = new List<Action>();
    public List<Action> onConsumeEffects = new List<Action>();
    public List<Action> onAcquireEffects = new List<Action>();

    public enum ItemType
    {
        Junk,
        Equippable,
        Consumable,
        QuestItem,
    }

    public enum EquippableType
    {
        None,
        Weapon,
        Shield,
        Armor,
        Necklace,
        Ring,
    }

    public ItemData() { }

    public ItemData(ItemData other)
    {
        ID = other.ID;
        name = other.name;
        description = other.description;
        itemType = other.itemType;
        equippableType = other.equippableType;
        prefabPath = other.prefabPath;
        iconPath = other.iconPath;
        displayStats = other.displayStats;

        onEquipEffects = new List<Action>(other.onEquipEffects);
        onUnequipEffects = new List<Action>(other.onUnequipEffects);
        onConsumeEffects = new List<Action>(other.onConsumeEffects);
        onAcquireEffects = new List<Action>(other.onAcquireEffects);
    }

}
