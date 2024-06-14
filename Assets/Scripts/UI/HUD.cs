using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HUD : MonoBehaviour
{
    [SerializeField]
    private GameObject abFrame1, abFrame2, abFrame3, healthBarFrame, staminaBarFrame, ratioXPbarFrame, cooldownBarFrame, questFrame;
    private List<GameObject> frames;

    private HudManager manager;

    private List<IAbility> abilityList;
    private List<AbilityIcon> abIconControllers = new();

    private GameObject healthBar;
    private HealthBar healthBarScript;
    private GameObject staminaBar;
    private StaminaBar staminaBarScript;
    private GameObject ratioBar;
    private RatioStatBar ratioStatBarScript;
    private GameObject cooldownBar;
    private FormCooldownBar cooldownBarScript;

    private string healthBarPrefabPath = "Prefabs/UI/HUDHealthBar";
    private string staminaBarPrefabPath = "Prefabs/UI/HUDStaminaBar";
    private string abilityIconPrefabPath = "Prefabs/UI/AbilityIcon";
    private string ratioBarPrefabPath = "Prefabs/UI/XpRatioBar";
    private string formCooldownPath = "Prefabs/UI/CooldownBar";
    private string questPopupPath = "Prefabs/UI/QuestPopup";

    private void Awake()
    {
        frames = new List<GameObject>
        {
            abFrame1,
            abFrame2,
            abFrame3
        };

    }

    public void Prepare(HudManager manager)
    {
        this.manager = manager;
        this.abilityList = manager.abilityList;

        SpawnHealthBar();
        SpawnStaminaBar();
        SpawnRatioBar();
        SpawnCooldownBar();
        RedrawAbilityIcons();
    }

    // UPDATE CARRIED OVER FROM PLAYER BEHAVIOUR FOR BETTER CONTROL.
    // This will stop being called every frame when player disappears
    public void UpdateHud()
    {
        UpdateAbilities();
        UpdateHealthBar();
        UpdateStaminaBar();
        UpdateRatioBar();
        UpdateFormCooldownBar();
    }

    private void SpawnHealthBar()
    {
        GameObject prefab = Resources.Load<GameObject>(healthBarPrefabPath);
        healthBar = Instantiate(prefab, healthBarFrame.transform);
        healthBarScript = healthBar.GetComponent<HealthBar>();
    }

    private void SpawnStaminaBar()
    {
        GameObject prefab = Resources.Load<GameObject>(staminaBarPrefabPath);
        staminaBar = Instantiate(prefab, staminaBarFrame.transform);
        staminaBarScript = staminaBar.GetComponent<StaminaBar>();
    }

    private void SpawnRatioBar()
    {
        GameObject prefab = Resources.Load<GameObject>(ratioBarPrefabPath);
        ratioBar = Instantiate(prefab, ratioXPbarFrame.transform);
        ratioStatBarScript = ratioBar.GetComponent<RatioStatBar>();
    }
    private void SpawnCooldownBar()
    {
        GameObject prefab = Resources.Load<GameObject>(formCooldownPath);
        cooldownBar = Instantiate(prefab, cooldownBarFrame.transform);
        cooldownBarScript = cooldownBar.GetComponent<FormCooldownBar>();
    }

    public void SetQuest()
    {
        GameObject prefab = Resources.Load<GameObject>(questPopupPath);
        GameObject questMessage = Instantiate(prefab, questFrame.transform);
        questMessage.GetComponent<TextMeshProUGUI>().text = "Goal: Make your way through the wilderness and obtain the Human Idol to drive the knights away.";
    }

    // this shouldn't be called every frame but rather every time the ability list changes 
    public void RedrawAbilityIcons()
    {
        GameObject prefab = Resources.Load<GameObject>(abilityIconPrefabPath);
        
        // cleaning up icons
        foreach(AbilityIcon iconController in abIconControllers)
        {
            Destroy(iconController.gameObject);
        }
        abIconControllers.Clear();

        // setting up new icons
        for (int i = 0; i < frames.Count; i++)
        {
            if (i < abilityList.Count)
            {
                IAbility ability = abilityList[i];

                GameObject abIcon = Instantiate(prefab, frames[i].transform);
                AbilityIcon iconController = abIcon.GetComponent<AbilityIcon>();
                abIconControllers.Add(iconController);

                Texture2D iconBckgrTexture = Resources.Load<Texture2D>(ability.iconBckgrPath);
                Texture2D iconTexture = Resources.Load<Texture2D>(ability.iconPath);

                iconController.backgroundImg.sprite = Sprite.Create(iconBckgrTexture, new Rect(0, 0, iconBckgrTexture.width, iconBckgrTexture.height), Vector2.zero);
                iconController.iconImg.sprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), Vector2.zero);
                iconController.SetAvailable(ability.isAvailable);
            }
        }




        UpdateAbilities();
    }

    private void UpdateAbilities()
    {
        for (int i = 0; i < abIconControllers.Count; i++)
        {
            IAbility ability = abilityList[i];
            AbilityIcon iconController = abIconControllers[i];

            if (ability.isOnCooldown)
            {
                float maxTime = ability.baseStats.cooldownTime;
                float currTime = ability.cooldownTime;
                iconController.SetValue(currTime, maxTime);
            }
            
            // THIS IS DONE EVERY UPDATE. NOTHING SERIOUS BUT UGLY
            bool isHighlight = ability.isCasting;
            iconController.SetHighlight(isHighlight);
            

        }
    }

    private void UpdateHealthBar()
    {
        healthBarScript.SetValue(manager.health, manager.maxHealth);
    }

    private void UpdateStaminaBar()
    {
        staminaBarScript.SetValue(manager.stamina, manager.maxStamina);
    }

    private void UpdateRatioBar()
    {
        ratioStatBarScript.SetValue(manager.humanXP, manager.dragonXP);
    }

    private void UpdateFormCooldownBar()
    {
        cooldownBarScript.SetValue(manager.formCooldownCurrTime, manager.formCooldownTime);
    }
}
