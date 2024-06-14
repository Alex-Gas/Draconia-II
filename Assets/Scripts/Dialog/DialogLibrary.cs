using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
public static class DialogLibrary
{
    public static Dictionary<int, List<DialogNode>> libraryOfConversations = new();
    private static XmlDocument npcDialogLineDocument;

    public static void Create()
    {
        CreateConversations();
        LoadDialogLines();
    }

    public static Conversation GetConversationByID(int id)
    {
        List<DialogNode> originalNodes = libraryOfConversations[id];
        if(originalNodes != null)
        {
            List<DialogNode> copiedNodes = new();
            foreach (var node in originalNodes)
            {
                DialogNode nodeCopy = new(node);
                copiedNodes.Add(nodeCopy);
            }

            Conversation conversation = new();
            conversation.ID = id;
            conversation.dialogNodes = copiedNodes;

            return conversation;
        }
        else
        {
            Debug.LogError("No conversation of the id: " + id + " exists in dialog library.");
            return null;
        }
    }

    public static string GetOptionText(int conversationID, int textID)
    {
        XmlNode conversationNode = npcDialogLineDocument.SelectSingleNode("/dialog/conversation[@id='" + conversationID + "']");
        if (conversationNode != null)
        {
            XmlNode lineNode = conversationNode.SelectSingleNode("line[@id='" + textID + "']");
            if (lineNode != null)
            {
                return lineNode.InnerText;
            }
        }

        Debug.LogError("Option text not found in xml document");
        return null;
    }

    private static void LoadDialogLines()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("DialogFiles/NpcDialogLines");
        if (textAsset != null)
        {
            npcDialogLineDocument = new XmlDocument();
            npcDialogLineDocument.LoadXml(textAsset.text);
        }

        else
        {
            Debug.LogError("NpcDialogLines document not found");
        }
    }



    // INSTRUCTIONS FOR MYSELF
    // each conversation has to have one and only one isDialogInitial = true to indicate which is the starting dialogue node (initial dialogue can dynamically change depending on conversation choices the player made)
    private static void CreateConversations()
    {
        libraryOfConversations = new()
        {
            {
                1, // Test
                new List<DialogNode>()
                {
                    new()
                    {
                        ID = 1,
                        text = "NPC line 1",
                        options = new()
                        {
                            { 2, 1 },
                            { 3, 2 },
                            { 4, 3 },
                            { 5, 4 },
                        },
                        isStart = true,
                    },
                    new()
                    {
                        ID = 2,
                        text = "NPC line 2",
                        options = new()
                        {
                            { 1, 1 },
                            { 5, 4 },
                        },
                        preActions = new()
                        {
                            new((DialogManager manager, PlayerBehaviour host, ITalkable talker) =>
                            {
                                manager.ReplaceOptions( 1, new(){
                                    { 3, 2 },
                                    { 5, 4 },
                                });
                            }),
                        },
                    },
                    new()
                    {
                        ID = 3,
                        text = "NPC line 3",
                        options = new()
                        {
                            { 5, 4 },
                        },
                    },
                    new()
                    {
                        ID = 4,
                        text = "NPC line 4",
                        options = new()
                        {
                            { 1, 1 },
                        },
                        preActions = new()
                        {
                            new((DialogManager manager, PlayerBehaviour host, ITalkable talker) =>
                            {
                                manager.RemoveOptions(1, new()
                                    {2, 3, 4}
                                );
                                manager.AddOptions( 1, new(){
                                    { 2, 1 },
                                });
                            }),
                        },
                    },
                    new()
                    {
                        ID = 5,
                    },
                }
            },
            {
                2, // Astero
                new List<DialogNode>()
                {
                    new()
                    {
                        ID = 1,
                        isStart = true,
                        isIntermediate = true,
                        postActions = new()
                        {
                            new((DialogManager manager, PlayerBehaviour host, ITalkable talker) =>
                            {
                                if (GameMaster.talkedToKharim == false)
                                {
                                    manager.CycleDialog(3);
                                }
                                else
                                {
                                    manager.CycleDialog(4);
                                }
                            }),
                        },
                    },
                    new()
                    {
                        ID = 2, // closing node
                    },
                    new()
                    {
                        ID = 3,
                        text = "Hey friend. Did you talk to Kharim yet? I think he wants to speak with you.",
                        options = new()
                        {
                            { 2, 1 },
                        },
                    },
                    new()
                    {
                        ID = 4,
                        text = "Ah, good I see you talked to Kharim. I'll provide you with some equipment you can take on your quest. Here's the basic set. You can also choose one additional item.",
                        options = new()
                        {
                            { 5, 2 },
                            { 6, 3 },
                            { 7, 4 }
                        },
                        preActions = new()
                        {
                            new ((DialogManager manager, PlayerBehaviour host, ITalkable talker) =>
                            {
                                manager.SetNewStartFlag(9);
                                host.AddItemsToInventory(new()
                                    { 4, 8, 3, 3, 3}
                                );
                            })
                        },
                    },
                    new()
                    {
                        ID = 5,
                        text = "Smart choice. Protection is always useful.",
                        options = new()
                        {
                            { 9, 7 },
                            { 8, 5 },
                        },
                        preActions = new()
                        {
                            new((DialogManager manager, PlayerBehaviour host, ITalkable talker) =>
                            {
                                host.AddItemsToInventory(new()
                                    { 7 }
                                );
                            }),
                        },
                    },
                    new()
                    {
                        ID = 6,
                        text = "Good choice. It'll make you stong! Like me!",
                        options = new()
                        {
                            { 9, 7 },
                            { 8, 5 },
                        },
                        preActions = new()
                        {
                            new((DialogManager manager, PlayerBehaviour host, ITalkable talker) =>
                            {
                                host.AddItemsToInventory(new()
                                    { 18 }
                                );
                            }),
                        },
                    },
                    new()
                    {
                        ID = 7,
                        text = "You've really taken Arshel's teachings on spiritual strength huh? Respectable.",
                        options = new()
                        {
                            { 9, 7 },
                            { 8, 5 },
                        },
                        preActions = new()
                        {
                            new((DialogManager manager, PlayerBehaviour host, ITalkable talker) =>
                            {
                                host.AddItemsToInventory(new()
                                    { 19 }
                                );
                            }),
                        },
                    },
                    new()
                    {
                        ID = 8,
                        text = "My pleasure. Go get them boy. I'm here if you need my advice or want to buy something.",
                        options = new()
                        {
                            { 2, 6 },
                        },
                    },
                    new()
                    {
                        ID = 9,
                        text = "How can I help friend?",
                        options = new()
                        {
                            { 10, 8 },
                            { 11, 9 },
                            { 2, 6 },
                        },
                    },
                    new()
                    {
                        ID = 10,
                        text = "Sure, don't forget to use my equipment, haha. Open your inventory with 'TAB' and click on an item to equip it or consume it. To drop an item right click it. Simple as.",
                        options = new()
                        {
                            { 9, 7 },
                            { 2, 6 },
                        },
                    },
                    new()
                    {
                        ID = 11,
                        text = "Music to my ears as always. Here, check this out.",
                        options = new()
                        {
                            { 12, 11 },
                            { 13, 12 },
                            { 9, 10},
                        },
                    },
                    new()
                    {
                        ID = 12,
                        isIntermediate = true,
                        postActions = new()
                        {
                            new((DialogManager manager, PlayerBehaviour host, ITalkable talker) => {
                                int price = 300;
                                if (host.shards >= price)
                                {
                                    host.shards -= price;
                                    host.AddItemsToInventory(new(){ 5 });
                                    manager.RemoveOptions( 11, new(){ 12 });
                                    manager.RemoveOptions( 14, new(){ 12 });
                                    manager.RemoveOptions( 15, new(){ 12 });
                                    manager.CycleDialog( 14 );
                                }
                                else
                                {
                                    manager.CycleDialog(15);
                                }
                            }),
                        },
                    },
                    new()
                    {
                        ID = 13,
                        isIntermediate = true,
                        postActions = new()
                        {
                            new((DialogManager manager, PlayerBehaviour host, ITalkable talker) => {
                                int price = 50;
                                if (host.shards >= price)
                                {
                                    host.shards -= price;
                                    host.AddItemsToInventory(new(){3});
                                    manager.CycleDialog(14);
                                }
                                else
                                {
                                    manager.CycleDialog(15);
                                }
                            }),
                        },
                    },
                    new()
                    {
                        ID = 14,
                        text = "Good choice. Anything else?",
                        options = new()
                        {
                            { 12, 11 },
                            { 13, 12 },
                            { 9, 10},
                        },
                    },
                    new()
                    {
                        ID = 15,
                        text = "Sorry lad but you don't seem to have enough shards to buy that. I'll keep it for you for later don't worry.",
                        options = new()
                        {
                            { 12, 11 },
                            { 13, 12 },
                            { 9, 10},
                        },
                    },
                }
            },
            {
                3, // Kharim
                new List<DialogNode>
                {
                    new()
                    {
                        ID = 1,
                        text = "Hello friend. I'm glad you're here. Listen, we have a problem, a big one. Knights of the Randa Order are amassing not far from here. Judging by their armor and weapons, they don't plan to sell us flowers. I wish it wouldn't have to come to this but it looks like we'll have to ask you to save us once again.",
                        isStart = true,
                        options = new()
                        {
                            { 4, 1},
                            { 5, 2},
                        },
                    },
                    new()
                    {
                        ID = 2, // closing node
                    },
                    new()
                    {
                        ID = 3,
                        text = "Anything else?",
                        options = new()
                        {
                            { 4, 1},
                            { 5, 2},
                        },
                    },
                    new()
                    {
                        ID = 4,
                        text = "We have barricaded the entrance so you'll need to use the teleporter on the south-east corner of the village. You'll have to make your way through the lands and find their leader sorcerer. " +
                        "       Human sorcerers always cary an idol. This idol is what gives humans their spiritual strength. Take it and they'll retreat. How you get to it is up to you.",

                        options = new()
                        {
                            { 3, 3},
                            { 5, 2},
                        },
                    },
                    new()
                    {
                        ID = 5,
                        text = "Wait you'll need the key to open the gate to the town portal. Here's  the key. Oh, also, don't forget to talk to Arshel and Astero. They can help you prepare. Good luck.",
                        options = new()
                        {
                            { 2, 5},
                        },
                        preActions = new()
                        {
                            new ((DialogManager manager, PlayerBehaviour host, ITalkable talker) =>
                            {
                                manager.SetNewStartFlag(6);
                                host.AddItemsToInventory(new()
                                    { 10 }
                                );
                                manager.RemoveOptions( 3, new(){ 5 });
                                manager.AddOptions ( 3, new()
                                {
                                    { 7, 4 },
                                });
                                manager.RemoveOptions( 4, new(){ 5 });
                                manager.AddOptions ( 4, new()
                                {
                                    { 7, 4 },
                                });
                                host.SetupQuest();
                            })
                        }
                    },
                    new()
                    {
                        ID = 6,
                        text = "Welcome back. How can I help?",
                        options = new()
                        {
                            { 4, 1},
                            { 7, 4},
                        },
                    },
                    new()
                    {
                        ID = 7,
                        text = "Wait! Oh no nevermind I already gave you the key. Off you go then.",
                        options = new()
                        {
                            { 2, 5},
                        },
                    },
                }
            },
            {
                4, // Arshel
                new List<DialogNode>
                {
                    new()
                    {
                        ID = 1,
                        isStart = true,
                        isIntermediate = true,
                        postActions = new()
                        {
                            new((DialogManager manager, PlayerBehaviour host, ITalkable talker) =>
                            {
                                if (GameMaster.talkedToKharim == false)
                                {
                                    manager.CycleDialog(3);
                                }
                                else
                                {
                                    manager.CycleDialog(4);
                                }
                            }),
                        },
                    },
                    new()
                    {
                        ID = 2, // closing node
                    },
                    new()
                    {
                        ID = 3,
                        text = "Greetings friend. I believe Kharim wishes to speak with you. You should see him first and then come back",
                        options = new()
                        {
                            { 2, 1},
                        },
                    },
                    new()
                    {
                        ID = 4,
                        text = "Good to see you. We may talk. ",
                        options = new()
                        {
                            { 6, 2},
                            { 5, 8},
                        },
                    },
                    new()
                    {
                        ID = 5,
                        text = "Of course. I'll be here if you need my advice",
                        options = new()
                        {
                            { 2, 9},
                        },
                    },
                    new()
                    {
                        ID = 6,
                        text = "With pleasure, what would you like to know?",
                        options = new()
                        {
                            { 7, 3},
                            { 8, 4},
                            { 9, 5},
                            { 5, 8},
                        },
                    },
                    new()
                    {
                        ID = 7,
                        text = "Your abilities serve different purposes and behave differently. In general, casting an ability entails a cost like your stamina. " +
                                "Additionally you need to wait for a period of time before casting the same ability again. Also, be careful as being struck by an enemy will interrupt the ability you are currently casting.",
                        options = new()
                        {
                            { 6, 6},
                            { 5, 8},
                        },
                    },
                    new()
                    {
                        ID = 8,
                        text = "By defeating your enemies you become stronger and can use more powerful abilities. Remember, the part of yourself you improve depends on which part you use.Killing, for example, will make your human side stronger, which I don't recommend. " +
                                "I advice to refrain from killing and embrace your dragon side. But its up to you in the end. ",
                        options = new()
                        {
                            { 6, 6},
                            { 10, 7},
                            { 5, 8},
                        },
                    },
                    new()
                    {
                        ID = 9,
                        text = "Do you see this monument next to me. Your dragon spirit can bind you to them. Moment before death it will pull you to it saving you from death. ",
                        options = new()
                        {
                            { 6, 6},
                            { 5, 8},

                        },
                    },
                    new()
                    {
                        ID = 10,
                        text = "Through violence and blood you make your human side stronger making your attacks deal more damage. On the contrary incapacitating enemies and sparing them makes you faster and more cunning. Focus and unleash your dragon side by pressing SPACE. " +
                                "Return to your human form by pressing SPACE again. You must rest for a bit before switching forms.",
                        options = new()
                        {
                            { 6, 6},
                            { 5, 8},
                        },
                    },
                }
            },
            {
                5, // Village Gate
                new()
                {
                    new()
                    {
                        ID = 1,
                        isStart = true,
                        isIntermediate = true,
                        postActions = new()
                        {
                            new((DialogManager manager, PlayerBehaviour host, ITalkable talker) =>
                            {
                                if (host.CheckIfItemInInventory(10))
                                {
                                    manager.CycleDialog(4);
                                }
                                else
                                {
                                    manager.CycleDialog(3);
                                }
                            }),
                        },
                    },
                    new()
                    {
                        ID = 2,
                    },
                    new()
                    {
                        ID = 3,
                        text = "The gate seems to be securely shut, you don't have anything that would allow you to open it yet.",
                        options = new()
                        {
                            { 2, 1 },
                        },
                    },
                    new()
                    {
                        ID = 4,
                        text = "The key you got from Kharim fits perfectly into the key hole. Open and pass through the gate? (You will leave the area!)",
                        options = new()
                        {
                            { 5, 2 },
                            { 2, 3 },  
                        },
                    },
                    new()
                    {
                        ID = 5,
                        isIntermediate = true,
                        postActions = new()
                        {
                            new((DialogManager manager, PlayerBehaviour host, ITalkable talker) =>
                            {
                                talker.DialogInteract();
                                manager.CycleDialog(2);                           
                            }),
                        },
                    },
                }
            },
        };
    }
}
