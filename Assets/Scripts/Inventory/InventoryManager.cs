using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager
{
    private GameObject UIobj;
    private InventoryUI UIscript;
    private PlayerBehaviour host;
    private string prefabPath = "Prefabs/UI/InventoryUI";
    public List<ItemData> itemsInPosession;

    public float physicalPower;
    public float spiritualPower;
    public float armor;

    public float totalXP;
    public float nextLevelXpGoal;
    public float humanXP;
    public float dragonXP;

    public int level;
    public int humanLevel;
    public int dragonLevel;

    public int shards;

    public InventoryManager(PlayerBehaviour host)
    {
        this.host = host;
        this.itemsInPosession = host.itemsInPosession;
    }

    public void UpdateInventoryStats()
    {
        host.UpdateInventoryStats(this);
    }

    public void ToggleInventory()
    {
        if (UIscript == null)
        {
            GameObject UIprefab = Resources.Load<GameObject>(prefabPath);
            UIobj = MonoBehaviour.Instantiate(UIprefab);
            UIscript = UIobj.GetComponent<InventoryUI>();
            UIscript.Prepare(this);

            GameMaster.isInventoryOpen = true;
        }

        else {
            UIscript.CloseInventory();

            GameMaster.isInventoryOpen = false;
        }
    }

    public void AddItemsToInventoryByID(List<int> listOfItemIDs)
    {
        foreach (int ID in listOfItemIDs)
        {
            ItemData itemData = ItemLibrary.LibraryItemRetriever(ID);
            AddItemToInventory(itemData);
        }
    }

    public void AddItemToInventory(ItemData incomingItemData)
    {
        ItemData itemData = new ItemData(incomingItemData);
        itemsInPosession.Add(itemData);
        InvokeOnAcquireEffects(itemData);

        Debug.Log("Acquired item: " + itemData.name);
    }

    private void InvokeOnAcquireEffects(ItemData itemData)
    {
        foreach (ItemData.Action action in itemData.onAcquireEffects)
        {
            action.Invoke(host);
        }
    }

    // Runs when player clicks on inventory slot occupied by an item
    public void SlotClick(int slotNo)
    {
        //Debug.Log("slot clicked: " + slotNo);
        IItem item = UIscript.itemsInInventory[slotNo];
        item.OnClick(host, this);
        UIscript.RedrawInventory();
    }

    public void SlotHoverEnter(int slotNo)
    {
        IItem item = UIscript.itemsInInventory[slotNo];
        UIscript.SetItemName(item.GetItemName());
        UIscript.SetItemDescription(item.GetDescription());
        UIscript.SetItemStats(item.GetStats());
    }

    public void SlotHoverExit(int slotNo)
    {
        UIscript.SetItemName("");
        UIscript.SetItemDescription("");
        UIscript.SetItemStats(new() { physicalPower = 0, spiritPower = 0, });
    }

    // this is for dropping items directly without inventory
    public void DropItemDirect(ItemData itemData)
    {
        // this is for automatically unequipping item that is dropped directly (without opening inventory)
        if (itemData.itemType == ItemData.ItemType.Equippable && itemData.isEquipped == true)
        {
            EquippableItem item = new(itemData);
            item.UnequipItem(host);
        }
        // --------
        SpawnDroppedItem(itemData);

        host.itemsInPosession.Remove(itemData);
    }

    // this is for dropping items from inventory
    public void DropItemFromInventory(int slotNo) 
    {
        IItem item = UIscript.itemsInInventory[slotNo];
        if (item.itemData.itemType != ItemData.ItemType.QuestItem)
        {
            SpawnDroppedItem(item.itemData);
            // removing original itemData from inventory through the item
            item.RemoveItem(host);
            // important keep this here other wise the inventory will not refresh 
            UIscript.RedrawInventory();
        }
    }

    public void SpawnDroppedItem(ItemData itemData)
    {
        //Resources.Load<GameObject>(prefabPath);
        GameObject prefab = Resources.Load<GameObject>(itemData.prefabPath);
        GameObject droppedItem = MonoBehaviour.Instantiate(prefab, host.transform.position, Quaternion.identity);
        ItemBehaviour script = droppedItem.GetComponent<ItemBehaviour>();
        script.TransfuseItemData(itemData);
        script.OnDrop();
    }

    public string GetStatsDescription()
    {
        // Grab item stats from character's equipped items and compile a string here
        int random = Random.Range(0, 100);
        return "Stats Here: " + random.ToString();
    }


    public void UnequipItemsOfType(ItemData.EquippableType equippableType)
    {
        //Debug.Log(equippableType);
        foreach (ItemData itemData in itemsInPosession)
        {
            if (itemData.itemType == ItemData.ItemType.Equippable && itemData.equippableType != ItemData.EquippableType.None && itemData.equippableType == equippableType && itemData.isEquipped)
            {
                EquippableItem item = new(itemData);
                item.UnequipItem(host);
            }
        }
    }

    public bool CheckIfItemInInventory(int id)
    {
        foreach (ItemData item in host.itemsInPosession)
        {
            if (item.ID == id)
            {
                return true;
            }
        }
        return false;
    }
}
