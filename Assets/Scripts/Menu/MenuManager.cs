using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager
{
    private GameObject UIobj;
    private MenuUI UIscript;
    private string prefabPath = "Prefabs/UI/MenuUI";

    public enum MenuMode
    {
        Pause,
        Start,
        End,
    }


    public void ToggleMenu(MenuMode mode)
    {
        if (mode == MenuMode.Pause)
        {
            if (UIscript == null)
            {
                OpenMenu(mode);
            }
            else if (mode == UIscript.mode)
            {
                CloseMenu();
            }
        }
        
        else if (mode == MenuMode.Start)
        {
            if (UIscript == null)
            {
                OpenMenu(mode);
            }
            else if (mode == UIscript.mode)
            {
                CloseMenu();
            }
        }

        else if (mode == MenuMode.End)
        {
            if (UIscript == null) {
                OpenMenu(mode);
            }
            else if (mode == UIscript.mode)
            {
                CloseMenu();
            }
        }
        
    }

    private void OpenMenu(MenuMode mode)
    {
        if (UIscript == null)
        {
            GameObject UIprefab = Resources.Load<GameObject>(prefabPath);
            UIobj = MonoBehaviour.Instantiate(UIprefab);
            UIscript = UIobj.GetComponent<MenuUI>();
            UIscript.Prepare(this, mode);
        }

        GameMaster.isMenuOpen = true;
    }

    public void CloseMenu()
    {
        if (UIscript != null)
        {
            UIscript.CloseMenu();
        }

        GameMaster.isMenuOpen = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
