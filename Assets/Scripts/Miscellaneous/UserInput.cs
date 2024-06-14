using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput
{
    public float GetHorizontalVal()
    {
        return Input.GetAxis("Horizontal");
    }

    public float GetVerticalVal()
    {
        return Input.GetAxis("Vertical");
    }

    public bool GetMouseLeftDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool GetMouseRightDown()
    {
        return Input.GetMouseButtonDown(1);
    }

    public Vector2  GetMousePosition() 
    {
        return Input.mousePosition;
    }

    public bool GetAbility1Input()
    {
        return Input.GetMouseButtonDown(0);
    }
    public bool GetAbility2Input()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }

    public bool GetAbility3Input()
    {
        return Input.GetKeyDown(KeyCode.LeftControl);
    }

    public bool GetInventoryInput()
    {
        return Input.GetKeyDown(KeyCode.Tab);
    }

    public bool GetFormSwitchInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public bool GetInteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
}
