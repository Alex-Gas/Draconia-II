using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ItemData;

public class DialogNode
{
    public int ID { get; set; }
    public string text { get; set; }
    public Dictionary<int, int> options { get; set; } = new();
    public bool isStart { get; set; }
    //public bool isTransition { get; set; }
    public bool isIntermediate { get; set; }



    public delegate void DialogAction(DialogManager manager, PlayerBehaviour host, ITalkable talker);
    public List<DialogAction> preActions = new List<DialogAction>();
    public List<DialogAction> postActions = new List<DialogAction>();


    public DialogNode() { }
    public DialogNode(DialogNode other)
    {
        ID = other.ID;
        text = other.text;
        options = new Dictionary<int, int>(other.options);
        isStart = other.isStart;
        //isTransition = other.isTransition;
        isIntermediate = other.isIntermediate;
        
     
        preActions = other.preActions;
        postActions = other.postActions;
    }


}
