using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudManager
{
    private GameObject UIobj;
    private HUD UIscript;
    private PlayerBehaviour host;
    private string prefabPath = "Prefabs/UI/HUD";

    public List<IAbility> abilityList;
    public float maxHealth;
    public float health;
    public float maxStamina;
    public float stamina;
    public float humanXP;
    public float dragonXP;

    public float formCooldownCurrTime;
    public float formCooldownTime;

    public HudManager(PlayerBehaviour host)
    {
        this.host = host;
        this.abilityList = host.abilityList;
        formCooldownTime = host.formCooldownTime;

        GameObject UIprefab = Resources.Load<GameObject>(prefabPath);
        UIobj = MonoBehaviour.Instantiate(UIprefab);
        UIscript = UIobj.GetComponent<HUD>();
        UIscript.Prepare(this);
    }
    
    // this is for updating things like ability timer which requires constant updating through framerate
    public void UpdateHud()
    {
        UIscript.UpdateHud();
    }

    // this must be called from playerbehaviour every time the list of abilities changes in some way
    public void RedrawAbilityIcons()
    {
        UIscript.RedrawAbilityIcons();
    }

    public void SetQuestMessage()
    {
        if (UIscript != null)
        {
            UIscript.SetQuest();
        }
    }
}
