using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : EntityBehaviour, ITalkable, IInteractable
{
    [SerializeField] private GameObject theDoor;
    private BoxCollider2D doorCollider;

    public new void Start()
    {
        base.Start();

        doorCollider = theDoor.GetComponent<BoxCollider2D>();
    }

    public void OnInteract(PlayerBehaviour entity)
    {
        if (conversation != null)
        {
            entity.RequestConversation(conversation, this);
        }
    }

    public new void DialogInteract()
    {
        OpenDoor();
    }

    private void OpenDoor()
    {
        doorCollider.enabled = false;
    }
}
