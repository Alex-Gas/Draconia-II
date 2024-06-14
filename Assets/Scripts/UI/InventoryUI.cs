using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private InventoryManager manager;

    public GameObject slotPrefab, buttonPrefab, InventorySlotsBox, nameBox, descriptionBox, 
        physicalPowerBox, spiritualPowerBox, armourBox, healthBox, staminaBox, shardsValueBox,  
        playerPhysicalPowerBox, playerSpiritualPowerBox, playerArmourBox,
        xpBar, totalXP, level, humanLevel, dragonLevel, shardsBox;

    private List<GameObject> slotList = new();
    private List<GameObject> buttonList = new();
    private List<ItemData> itemsInPosession;    // this is original list of items carried over from the host
    public List<IItem> itemsInInventory = new();       // this is a list of item objects for the purpose of being manipulated by the inventory system

    private readonly int[] slotHeights = new int[] { 190, 115, 40, -35, -110, -185 };
    private readonly int[] slotWidths = new int[] { -75, 0, 75 };

    private void Awake()
    {
        foreach (int height in slotHeights)
        {
            foreach (int width in slotWidths)
            {
                GameObject slot = Instantiate(slotPrefab, InventorySlotsBox.transform);
                slot.transform.localPosition = new Vector3(width, height, 0);
                slotList.Add(slot);
            }
        }
    }

    public void Prepare(InventoryManager manager)
    {
        this.manager = manager;
        this.itemsInPosession = manager.itemsInPosession;

        RedrawInventory();
    }

    public void CloseInventory()
    {
        Destroy(gameObject);
    }

    public void DisableButtons()
    {
        foreach (GameObject button in buttonList)
        {
            button.SetActive(false);
        }
    }

    private void RedrawButtons()
    {
        foreach(GameObject button in buttonList)
        {
            Destroy(button);
        }
        buttonList.Clear();

        for (int i = 0; i < itemsInInventory.Count && i < slotList.Count; i++)
        {
            SetButton(i);
        }
    }

    private void RedrawPlayerStats()
    {
        playerPhysicalPowerBox.GetComponent<TextMeshProUGUI>().text = "Physical Power: " + manager.physicalPower.ToString();
        playerSpiritualPowerBox.GetComponent<TextMeshProUGUI>().text = "Spiritual Power: " + manager.spiritualPower.ToString();
        playerArmourBox.GetComponent<TextMeshProUGUI>().text = "Armour: " + manager.armor.ToString();

        xpBar.GetComponent<Image>().fillAmount = manager.totalXP / manager.nextLevelXpGoal;
        totalXP.GetComponent<TextMeshProUGUI>().text = manager.totalXP.ToString() + "/" + manager.nextLevelXpGoal.ToString();
        level.GetComponent<TextMeshProUGUI>().text = "Total Level: " + manager.level.ToString();
        humanLevel.GetComponent<TextMeshProUGUI>().text = "Human Level: " + manager.humanLevel.ToString();
        dragonLevel.GetComponent<TextMeshProUGUI>().text = "Dragon Level: " + manager.dragonLevel.ToString();

        shardsBox.GetComponent<TextMeshProUGUI>().text = "Shards: " + manager.shards.ToString();
    }

    public void RedrawInventory()
    {
        //Debug.Log("Redrawing inventory");
        itemsInInventory = new();

        manager.UpdateInventoryStats();

        PrepareItems();
        
        RedrawButtons();

        RedrawPlayerStats();
        //SetStatsDescription(manager.GetStatsDescription());
    }

    public void PrepareItems()
    {
        foreach(ItemData itemData in itemsInPosession)
        {
            switch (itemData.itemType)
            {
                case ItemData.ItemType.Equippable:
                    EquippableItem equippableItem = new EquippableItem(itemData);
                    itemsInInventory.Add(equippableItem);   
                    break;

                case ItemData.ItemType.Consumable:
                    ConsumableItem consumableItem = new ConsumableItem(itemData);
                    itemsInInventory.Add(consumableItem);
                    break;

                case ItemData.ItemType.Junk:
                    JunkItem junkItem = new JunkItem(itemData);
                    itemsInInventory.Add(junkItem);
                    break;

                case ItemData.ItemType.QuestItem:
                    QuestItem questItem = new QuestItem(itemData);
                    itemsInInventory.Add(questItem);
                    break;
            }
        } 
    }

    // TEMPORARY
    private void SetActiveHighlight(bool isActive, GameObject button)
    {
        Image image = button.GetComponent<Image>();
        if (isActive)
        {
            image.color = new Color32(190, 174, 114, 255);
        }
        else
        {
            image.color = new Color32(91, 91, 91, 255);
        }
    }
    
    private void SetButton(int i)
    {
        GameObject slot = slotList[i];
        GameObject button = Instantiate(buttonPrefab, slot.transform);

        button.GetComponent<InventoryButton>().onLeftClick = () => manager.SlotClick(i);
        button.GetComponent<InventoryButton>().onRightClick = () => manager.DropItemFromInventory(i);
        button.GetComponent<InventoryButton>().onHoverEnter = () => manager.SlotHoverEnter(i);
        button.GetComponent<InventoryButton>().onHoverExit = () => manager.SlotHoverExit(i);

        SetImage(button, itemsInInventory[i].itemData.iconPath);
        SetActiveHighlight(itemsInPosession[i].isEquipped, button);

        buttonList.Add(button);

        button.SetActive(true);
    }

    /*
    public void SetStatsDescription(ItemStats itemStats)
    {
        physicalPowerBox.GetComponent<TextMeshProUGUI>().text = itemStats.physicalPower.ToString();
        spiritualPowerBox.GetComponent<TextMeshProUGUI>().text = itemStats.spiritPower.ToString();
        armourBox.GetComponent<TextMeshProUGUI>().text = itemStats.armor.ToString();
    }
    */

    public void SetItemName(string name)
    {
        nameBox.GetComponent<TextMeshProUGUI>().text = name;
    }

    public void SetItemDescription(string description)
    {
        descriptionBox.GetComponent<TextMeshProUGUI>().text = description;
    }

    public void SetItemStats(ItemStats stats)
    {
        physicalPowerBox.GetComponent<TextMeshProUGUI>().text = "";
        spiritualPowerBox.GetComponent<TextMeshProUGUI>().text = "";
        armourBox.GetComponent<TextMeshProUGUI>().text = "";
        healthBox.GetComponent<TextMeshProUGUI>().text = "";
        staminaBox.GetComponent<TextMeshProUGUI>().text = "";
        shardsValueBox.GetComponent<TextMeshProUGUI>().text = "";

        if (stats.physicalPower > 0)
        {
            physicalPowerBox.GetComponent<TextMeshProUGUI>().text = "Physical Power: " + stats.physicalPower.ToString();
        }
        if (stats.spiritPower > 0)
        {
            spiritualPowerBox.GetComponent<TextMeshProUGUI>().text = "Spiritual Power: " + stats.spiritPower.ToString();
        }
        if (stats.armor > 0)
        {
            armourBox.GetComponent<TextMeshProUGUI>().text = "Armour: " + stats.armor.ToString();
        }
        if (stats.health > 0)
        {
            healthBox.GetComponent<TextMeshProUGUI>().text = "Health Effect: " + stats.health.ToString();
        }
        if (stats.stamina > 0)
        {
            staminaBox.GetComponent<TextMeshProUGUI>().text = "Stamina Effect: " + stats.stamina.ToString();
        }
        if (stats.shards > 0)
        {
            shardsValueBox.GetComponent<TextMeshProUGUI>().text = "Shards Value: " + stats.shards.ToString();
        }
    }



    private void SetImage(GameObject obj, string iconPath)
    {
        GameObject imageEle = obj.transform.Find("Image").gameObject;
        Image imageComp = imageEle.GetComponent<Image>();
        Texture2D iconTexture = Resources.Load<Texture2D>(iconPath);
        imageComp.sprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), Vector2.zero);
    }
}
