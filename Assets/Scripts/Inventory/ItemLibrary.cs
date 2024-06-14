using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemLibrary
{
    public static List<ItemData> libraryOfItems = new();

    private static string iconPathTemp = "Textures/Items/ItemIconTextures/";
    private static string prefPathTemp = "Prefabs/Items/";

    public static void Create()
    {
        CreateItems();
    }



    public static ItemData LibraryItemRetriever(int requestedItemID)
    {
        ItemData obtainedItemOriginal = GetItemDataOriginalByID(requestedItemID);
        if (obtainedItemOriginal != null)
        {
            return new(obtainedItemOriginal);
        }
        else
        {
            Debug.LogError("Requested item doesn't exist in the library");
            return null;
        }

    }

    private static ItemData GetItemDataOriginalByID(int ID)
    {
        foreach (ItemData item in libraryOfItems)
        {
            if (item.ID == ID) { return item; }
        }
        return null;
    }



    private static void CreateItems()
    {
        libraryOfItems = new()
        {
            new()
            {
                ID = 0,
                name = "Default_item",
                description = "Default item.",
                itemType = ItemData.ItemType.Junk,
                prefabPath = prefPathTemp + "DefaultItem",
                iconPath = iconPathTemp + "DefaultItemIcon",
            },
            new()
            {
                ID = 1,
                name = "Test_item_equippable",
                description = "Equippable item description test",
                itemType = ItemData.ItemType.Equippable,
                equippableType = ItemData.EquippableType.None,
                prefabPath = prefPathTemp + "TestItemEquippable",
                iconPath = iconPathTemp + "EquippableIcon",
                onEquipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        health = 1,
                    }); }),
                },
                onUnequipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        health = -1,
                    }); }),
                },
            },
            new()
            {
                ID = 2,
                name = "Test_item_consumable",
                description = "Consumable item description test",
                itemType = ItemData.ItemType.Consumable,
                prefabPath = prefPathTemp + "TestItemConsumable",
                iconPath = iconPathTemp + "ConsumableIcon",
                onConsumeEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        health = 1,
                    }); }),
                },
            },

            // -------------------------- ACTUAL ITEMS 
            new()
            {
                ID = 3,
                name = "Health Potion",
                description = "A well known concoction that mends wounds.",
                prefabPath = prefPathTemp + "HealthPotion",
                iconPath = iconPathTemp + "health_potion_icon",
                itemType = ItemData.ItemType.Consumable,
                displayStats = new()
                {
                    health = 15,
                },
                onConsumeEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        health = 15,
                    }); }),
                },
            },
            new()
            {
                ID = 4,
                name = "Basic Sword",
                description = "A simple unimpressive steel sword.",
                prefabPath = prefPathTemp + "BasicSword",
                iconPath = iconPathTemp + "basic_sword_icon",
                itemType = ItemData.ItemType.Equippable,
                equippableType = ItemData.EquippableType.Weapon,
                displayStats = new()
                {
                    physicalPower = 4,
                },
                onEquipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        physicalPower = 4,
                    }); }),
                },
                onUnequipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        physicalPower = -4,
                    }); }),
                },
            },
            new()
            {
                ID = 5,
                name = "Basic Shield",
                description = "Made of wood this shield provides basic protection.",
                prefabPath = prefPathTemp + "BasicShield",
                iconPath = iconPathTemp + "basic_shield_icon",
                itemType = ItemData.ItemType.Equippable,
                equippableType = ItemData.EquippableType.Shield,
                displayStats = new()
                {
                    physicalPower = 1,
                    armor = 2,
                },
                onEquipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        physicalPower = 1,
                        armor = 2,
                    }); }),
                },
                onUnequipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        physicalPower = -1,
                        armor = -2,
                    }); }),
                },
            },
            new()
            {
                ID = 6,
                name = "Basic Magic Ring",
                description = "It looks like an ordinary ring but you can sense a hint of magical power emanating from it.",
                prefabPath = prefPathTemp + "BasicMagicRing",
                iconPath = iconPathTemp + "basic_ring_icon",
                itemType = ItemData.ItemType.Equippable,
                equippableType = ItemData.EquippableType.Ring,
                displayStats = new()
                {
                    spiritPower = 2,
                },
                onEquipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        spiritPower = 2,
                    }); }),
                },
                onUnequipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        spiritPower = -2,
                    }); }),
                },
            },
            new()
            {
                ID = 7,
                name = "Chain Mail",
                description = "Light and flexible with decent protection.",
                prefabPath = prefPathTemp + "ChainMail",
                iconPath = iconPathTemp + "chain_mail_icon",
                itemType = ItemData.ItemType.Equippable,
                equippableType = ItemData.EquippableType.Armor,
                displayStats = new()
                {
                    armor = 3,
                },
                onEquipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        armor = 3,
                    }); }),
                },
                onUnequipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        armor = -3,
                    }); }),
                },
            },
            new()
            {
                ID = 8,
                name = "Basic Magic Necklace",
                description = "Not even a necklace but a simple chain. The simplicity obscures its magical properties.",
                prefabPath = prefPathTemp + "BasicMagicNecklace",
                iconPath = iconPathTemp + "basic_necklace_icon",
                itemType = ItemData.ItemType.Equippable,
                equippableType = ItemData.EquippableType.Necklace,
                displayStats = new()
                {
                    spiritPower = 3,
                },
                onEquipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        spiritPower = 3,
                    }); }),
                },
                onUnequipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        spiritPower = -3,
                    }); }),
                },
            },
            new()
            {
                ID = 9,
                name = "Sharp Sword",
                description = "A well sharpened steel sword.",
                prefabPath = prefPathTemp + "SharpSword",
                iconPath = iconPathTemp + "sharp_sword_icon",
                itemType = ItemData.ItemType.Equippable,
                equippableType = ItemData.EquippableType.Weapon,
                displayStats = new()
                {
                    spiritPower = 8,
                },
                onEquipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        physicalPower = 8,
                    }); }),
                },
                onUnequipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        physicalPower = -8,
                    }); }),
                },
            },
            new()
            {
                ID = 10,
                name = "Village Gate Key",
                description = "Used to open the village gates.",
                prefabPath = prefPathTemp + "VillageGateKey",
                iconPath = iconPathTemp + "village_gate_key_icon",
                itemType = ItemData.ItemType.QuestItem,   
            },
            new()
            {
                ID = 11,
                name = "War Sword",
                description = "Used by human soldiers. Deadly.",
                prefabPath = prefPathTemp + "WarSword",
                iconPath = iconPathTemp + "war_sword_icon",
                itemType = ItemData.ItemType.Equippable,
                equippableType = ItemData.EquippableType.Weapon,
                displayStats = new()
                {
                    spiritPower = 13,
                },
                onEquipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        physicalPower = 13,
                    }); }),
                },
                onUnequipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        physicalPower = -13,
                    }); }),
                },
            },

            new()
            {
                ID = 12,
                name = "Improved Chain Mail",
                description = "The rings have been crafted with care. Slightly heavier but offers much better protection than regular chain mail.",
                prefabPath = prefPathTemp + "ImprovedChainMail",
                iconPath = iconPathTemp + "improved_chain_mail_icon",
                itemType = ItemData.ItemType.Equippable,
                equippableType = ItemData.EquippableType.Armor,
                displayStats = new()
                {
                    armor = 5,
                },
                onEquipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        armor = 5,
                    }); }),
                },
                onUnequipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        armor = -5,
                    }); }),
                },
            },

            new()
            {
                ID = 13,
                name = "Half Plated Armor",
                description = "A combination of leather, chain mail, and plates provides an excellent balance between protection and mobility.",
                prefabPath = prefPathTemp + "HalfPlatedArmor",
                iconPath = iconPathTemp + "plate_armor_icon",
                itemType = ItemData.ItemType.Equippable,
                equippableType = ItemData.EquippableType.Armor,
                displayStats = new()
                {
                    armor = 7,
                },
                onEquipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        armor = 7,
                    }); }),
                },
                onUnequipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        armor = -7,
                    }); }),
                },
            },
            new()
            {
                ID = 14,
                name = "Toothed Necklace",
                description = "The pendant of the necklace is adorned with rows of sharp teeth. You can hear slight whispers comming out of its mouth-like opening.",
                prefabPath = prefPathTemp + "ToothedNecklace",
                iconPath = iconPathTemp + "toothed_necklace_icon",
                itemType = ItemData.ItemType.Equippable,
                equippableType = ItemData.EquippableType.Necklace,
                displayStats = new()
                {
                    spiritPower = 5,
                },
                onEquipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        spiritPower = 5,
                    }); }),
                },
                onUnequipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        spiritPower = -5,
                    }); }),
                },
            },
            new()
            {
                ID = 15,
                name = "Draconic Necklace",
                description = "Draconians craft these necklces for their guards to help them focus.",
                prefabPath = prefPathTemp + "DraconicNecklace",
                iconPath = iconPathTemp + "draconic_necklace_icon",
                itemType = ItemData.ItemType.Equippable,
                equippableType = ItemData.EquippableType.Necklace,
                displayStats = new()
                {
                    spiritPower = 7,
                },
                onEquipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        spiritPower = 7,
                    }); }),
                },
                onUnequipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        spiritPower = -7,
                    }); }),
                },
            },
            new()
            {
                ID = 16,
                name = "Draconic Ring",
                description = "Draconian ring crafting is known to be superior.",
                prefabPath = prefPathTemp + "DraconicRing",
                iconPath = iconPathTemp + "draconic_ring_icon",
                itemType = ItemData.ItemType.Equippable,
                equippableType = ItemData.EquippableType.Ring,
                displayStats = new()
                {
                    spiritPower = 5,
                },
                onEquipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        spiritPower = 5,
                    }); }),
                },
                onUnequipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        spiritPower = -5,
                    }); }),
                },
            },

            new()
            {
                ID = 17,
                name = "Draconic Shield",
                description = "Draconians build their shield focused on defense due to their natural attitude. Ornate symbols decorating the shield help keep up their spirits.",
                prefabPath = prefPathTemp + "DraconicShield",
                iconPath = iconPathTemp + "draconic_shield_icon",
                itemType = ItemData.ItemType.Equippable,
                equippableType = ItemData.EquippableType.Shield,
                displayStats = new()
                {
                    spiritPower = 2,
                    armor = 4,
                },
                onEquipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        spiritPower = 2,
                        armor = 4,
                    }); }),
                },
                onUnequipEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        spiritPower = -2,
                        armor = -4,
                    }); }),
                },
            },
            new()
            {
                ID = 18,
                name = "Strength Potion",
                description = "Smells funny. You have a good feeling about this.",
                prefabPath = prefPathTemp + "StrengthPotion",
                iconPath = iconPathTemp + "strength_potion_icon",
                itemType = ItemData.ItemType.Consumable,
                displayStats = new()
                {
                    physicalPower = 3,
                },
                onConsumeEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                         physicalPower = 3,
                    }); }),
                },
            },
            new()
            {
                ID = 19,
                name = "Herbal Potion",
                description = "Strong herbal smells emanate from the tip. Looks like a spritually refreshing drink.",
                prefabPath = prefPathTemp + "HerbalPotion",
                iconPath = iconPathTemp + "herbal_potion_icon",
                itemType = ItemData.ItemType.Consumable,
                displayStats = new()
                {
                    spiritPower = 2,
                },
                onConsumeEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        spiritPower = 2,
                    }); }),
                },
            },
            new()
            {
                ID = 20,
                name = "Human Icon",
                description = "Humans revere this object.",
                prefabPath = prefPathTemp + "HumanIcon",
                iconPath = iconPathTemp + "human_icon_icon",
                itemType = ItemData.ItemType.QuestItem,
                onAcquireEffects = new()
                {
                    new((CharacterBehaviour owner) => { GameMaster.EndGame(); }),
                }
            },
            new()
            {
                ID = 21,
                name = "Shards Pouch",
                description = "Purple crystal shards rattle inside. Used as a local currency.",
                prefabPath = prefPathTemp + "ShardsPouch",
                iconPath = iconPathTemp + "shard_pouch_icon",
                itemType = ItemData.ItemType.Consumable,
                displayStats = new()
                {
                    shards = 50,
                },
                onConsumeEffects = new()
                {
                    new((CharacterBehaviour owner) => { owner.ItemStatEffect(new ItemStats()
                    {
                        shards = 50,
                    }); }),
                },
            },

        };
    }
}
