using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ContainerBehaviour : EntityBehaviour, IInteractable
{
    private bool isContainerSpent = false;

    public int containerContentsID;

    private List<ItemData> itemsInContainer = new();

    public GameObject dropPoint;

    private List<float> dropPosX = new List<float>() { 0, -0.2f, 0.2f };
    private List<float> dropPosY = new List<float>() { 0 , -0.2f, 0.2f };

    [SerializeField] GameObject modelOpen;
    [SerializeField] GameObject modelClosed;

    public new void Start()
    {
        base.Start();

        PrepareItems();
    }

    private void PrepareItems()
    {
        List<int> listOfItemIDs = ContainerLibrary.GetContainerContents(containerContentsID);

        foreach (int itemID in listOfItemIDs)
        {
            ItemData newItemData = ItemLibrary.LibraryItemRetriever(itemID);
            itemsInContainer.Add(newItemData);
        }
    }
    public void OnInteract(PlayerBehaviour entity)
    {
        if (!isContainerSpent)
        {
            OpenContainer();
            isContainerSpent = true;
            Visual();

            namePlate.GetComponent<NamePlate>().OverrideVisible(false);
        }

    }

    private void OpenContainer()
    {

        int no = 0;

        for (int i = 0; i < dropPosY.Count; i++)
        {
            for(int j = 0; j < dropPosX.Count; j++)
            {
        
                if (no < 9 && no < itemsInContainer.Count)
                {
            
                    ItemData itemData = itemsInContainer[no];
                    GameObject prefab = Resources.Load<GameObject>(itemData.prefabPath);
                    Vector2 dropLocation = new Vector3(dropPoint.transform.position.x + dropPosX[j], dropPoint.transform.position.y + dropPosY[i], -1f);
                    GameObject droppedItem = MonoBehaviour.Instantiate(prefab, dropLocation, Quaternion.identity);
                    ItemBehaviour script = droppedItem.GetComponent<ItemBehaviour>();
                    script.TransfuseItemData(itemData);
                    script.OnDrop();
                }
                no++;
            }
        }
    }

    private void Visual()
    {
        if (modelOpen != null && modelClosed != null)
        {
            modelOpen.GetComponent<SpriteRenderer>().enabled = true;
            modelClosed.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
