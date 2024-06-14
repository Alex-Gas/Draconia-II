using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorBehaviour : EntityBehaviour
{
    [SerializeField] private GameObject doorObject;


    private bool isCharged = true;
    public bool isOneTimeUse;

    public bool isOpenable;
    public bool isClosable;

    private bool isOpen;

    public new void Start()
    {
        base.Start();

        if (gameObject.activeSelf)
        {
            isOpen = false;
        }
        else{
            isOpen = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ToggleDoor();
    }



    private void ToggleDoor()
    {
        if (isOpenable && !isOpen && isCharged)
        {
            if (isOneTimeUse)
            {
                isCharged = false;
            }

            doorObject.SetActive(false);
            isOpen = true;
        }

        else if (isClosable && isOpen && isCharged)
        {
            if (isOneTimeUse)
            {
                isCharged = false;
            }

            doorObject.SetActive(true);
            isOpen = false;
        }
    }
}
