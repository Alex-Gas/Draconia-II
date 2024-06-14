using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager
{
    private GameObject UIobj;
    private DialogUI UIscript;
    private PlayerBehaviour host;
    private string prefabPath = "Prefabs/UI/DialogUI";

    public Conversation conversation;
    private ITalkable talker;

    public DialogManager(PlayerBehaviour host)
    {
        this.host = host;
    }

    public void InitiateDialog(Conversation conversation, ITalkable talker)
    {
        if (conversation != null)
        {
            this.conversation = conversation;
            this.talker = talker;

            if (conversation.dialogNodes.Count > 0)
            {
                OpenDialogBox();
                StartDialog();
            }

            else
            {
                Debug.LogError("Incoming list of dialog nodes is empty!");
            }
        }
        
        else {
            Debug.LogError("Incoming conversation is null!");
        }

    }

    public void OpenDialogBox()
    {
        if (UIobj == null)
        {
            GameObject UIprefab = Resources.Load<GameObject>(prefabPath);
            UIobj = MonoBehaviour.Instantiate(UIprefab);
            UIscript = UIobj.GetComponent<DialogUI>();
            UIscript.Prepare(this);

            GameMaster.isDialogOpen = true;
        }
    }

    private void StartDialog()
    {
        // looking for a node with "initial" flag
        foreach(DialogNode node in conversation.dialogNodes)
        {
            if (node.isStart == true)
            {
                CycleDialog(node.ID);
                //Debug.Log("Start node found, node id: " + node.ID);
                return;
            }
        }
        Debug.LogError("No start node found");
    }

    public void CycleDialog(int ID)
    {
        DialogNode node = SearchForNodeByID(ID);

        if (node != null)
        {
            PreExecuteActions(node);

            UIscript.ShowNode(node);

            PostExecuteActions(node);
        }

        else {
            Debug.LogError("Error, dialog force close.");
            CloseDialog();
        }
    }


    private DialogNode SearchForNodeByID(int ID)
    {
        foreach (DialogNode node in conversation.dialogNodes)
        {
            if (node.ID == ID)
            {
                return node;
            }
        }
        Debug.LogError("No dialog node in the conversation!");
        return null;
    }

    public void CloseDialog()
    {
        UIscript.CloseUI();
        GameMaster.isDialogOpen = false;
    }
    
    public void ClearData()
    {
        conversation = null;
        talker = null;
    }


    private void PreExecuteActions(DialogNode node)
    {
        foreach (DialogNode.DialogAction action in node.preActions)
        {
            action.Invoke(this, host, talker);
        }
    }

    private void PostExecuteActions(DialogNode node)
    {
        foreach (DialogNode.DialogAction action in node.postActions)
        {
            action.Invoke(this, host, talker);
        }
    }

    public void ReplaceOptions(int id, Dictionary<int, int> newOptions)
    {
        DialogNode node = SearchForNodeByID(id);
        node.options = newOptions;
    }

    public void AddOptions(int id, Dictionary<int, int> newOptions)
    {
        DialogNode node = SearchForNodeByID(id);
        foreach (KeyValuePair<int,int> newOption in newOptions )
        {
            node.options.Add(newOption.Key, newOption.Value);
        }
    }

    public void RemoveOptions(int id, List<int> optionsToRemove)
    {
        DialogNode node = SearchForNodeByID(id);
        Dictionary<int,int> temp = node.options;

        foreach (int optionID in optionsToRemove)
        {
            temp.Remove(optionID);
        }
        node.options = temp;
    }

    public void SetNewStartFlag(int id)
    {
        foreach (DialogNode node in conversation.dialogNodes)
        {
            node.isStart = false;
            if (node.ID == id)
            {
                node.isStart = true;
            }
        }
    }
}
