using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject startButtonSlot, exitButtonSlot;

    private MenuManager manager;

    public MenuManager.MenuMode mode;

    public void Prepare(MenuManager manager, MenuManager.MenuMode mode)
    {
        this.manager = manager;


        this.mode = mode;
        SetupMenu(mode);
    }

    private void SetupMenu(MenuManager.MenuMode mode)
    {
        switch (mode)
        {
            case MenuManager.MenuMode.Start:
                SetupStartMenu();
                break;
            case MenuManager.MenuMode.Pause:
                SetupPauseMenu();
                break;
            case MenuManager.MenuMode.End:
                SetupEndMenu();
                break;
            default:
                SetupEndMenu();
                break;
        }
    }


    private void SetupStartMenu()
    {
        SetStartButton(buttonPrefab, startButtonSlot.transform);
        SetExitButton(buttonPrefab, exitButtonSlot.transform);
    }

    private void SetupPauseMenu()
    {
        SetResumeButton(buttonPrefab, startButtonSlot.transform);
        SetExitButton(buttonPrefab, exitButtonSlot.transform);
    }

    private void SetupEndMenu()
    {
        SetExitButton(buttonPrefab, exitButtonSlot.transform);
    }

    private void SetStartButton(GameObject prefab, Transform parent)
    {
        GameObject button = Instantiate(prefab, parent);
        button.GetComponent<MenuButton>().onLeftClick = () => manager.ToggleMenu(MenuManager.MenuMode.Start);
        button.GetComponent<MenuButton>().SetText("New Game");
    }

    private void SetResumeButton(GameObject prefab, Transform parent)
    {
        GameObject button = Instantiate(prefab, parent);
        button.GetComponent<MenuButton>().onLeftClick = () => manager.ToggleMenu(MenuManager.MenuMode.Pause);
        button.GetComponent<MenuButton>().SetText("Resume");
    }

    private void SetExitButton(GameObject prefab, Transform parent)
    {
        GameObject button = Instantiate(prefab, parent);
        button.GetComponent<MenuButton>().onLeftClick = () => manager.ExitGame();
        button.GetComponent<MenuButton>().SetText("Exit Game");
    }



    public void CloseMenu()
    {
        Debug.Log("closing menu");
        Destroy(gameObject);
    }

}
