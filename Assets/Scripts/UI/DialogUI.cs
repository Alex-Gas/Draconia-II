using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogUI : MonoBehaviour
{
    private DialogManager manager;

    public GameObject slotPrefab, buttonPrefab, dialogOptionSlotsBox, dialogTextBox;

    private List<GameObject> slotList = new();
    private List<GameObject> buttonList = new();
    private Conversation conversation = new();


    private readonly int[] slotHeights = new int[] { 50, 30, 10, -10, -30, -50 };
    private readonly int width = 0;


    private void Awake()
    {
        SetSlots();
    }

    private void SetSlots()
    {
        foreach (int height in slotHeights)
        {
            GameObject slot = Instantiate(slotPrefab, dialogOptionSlotsBox.transform);
            slot.transform.localPosition = new Vector3(width, height, 0);
            slotList.Add(slot);
        }
    }

    public void Prepare(DialogManager manager)
    {
        this.manager = manager;
        this.conversation = manager.conversation;
    }

    public void CloseUI()
    {
        manager.ClearData();
        Destroy(gameObject);
    }

    public void ShowNode(DialogNode node)
    {
        SetButtons(node);
        // at this point if there are no options the dialog would have closed now and nothing else will execute
        SetMainText(node);
    }

    private void SetMainText(DialogNode node)
    {
        TextMeshProUGUI textMesh = dialogTextBox.GetComponent<TextMeshProUGUI>();
        textMesh.text = node.text;
    }

    public void ClearButtons()
    {
        foreach (GameObject button in buttonList)
        {
            Destroy(button);
        }
        buttonList.Clear();
    }

    public void SetButtons(DialogNode node)
    {
        ClearButtons();

        Dictionary<int, int> options = node.options;

        if (options.Count > 0)
        {
            int slotNo = 0;
            foreach (KeyValuePair<int, int> option in options)
            {
                string text = DialogLibrary.GetOptionText(conversation.ID, option.Value);
                int nextNodeID = option.Key;
                SetButton(nextNodeID, text, slotNo);
                slotNo++;              
            }
        }

        else if (node.isIntermediate == true )
        {
            Debug.Log("redirecting node");
        }

        else{
            // if the options list is empty that means its a closing node
            manager.CloseDialog();
        }
    }

    private void SetButton(int nextNodeID, string text, int slotNo)
    {
        GameObject button = Instantiate(buttonPrefab, slotList[slotNo].transform);
        buttonList.Add(button);

        DialogButton btnScript = button.GetComponent<DialogButton>();
        btnScript.onLeftClick = () => manager.CycleDialog(nextNodeID);

        TextMeshProUGUI textMesh = btnScript.textObj.GetComponent<TextMeshProUGUI>();
        textMesh.text = text;



        button.SetActive(true);
    }
    

}
