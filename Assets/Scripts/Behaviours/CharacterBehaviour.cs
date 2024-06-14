using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterBehaviour : AnimateBehaviour, ISaveable<Character>
{
    protected Character character = new();

    public List<ItemData> itemsInPosession = new();

    protected GameObject staminaBarObj;
    private StaminaBar staminaBar;
    private string staminaBarPrefabPath = "Prefabs/UI/UnitStaminaBar";

    public bool isStaminaBar = false;
    public float staminaBarHeight;

    public List<IItem> itemList;

    public CharacterType characterType;

    public Animator animator;
    public VisualManager visualManager;

    public int deadBodyTextureID;
    public int knockedOutBodyTextureID;
    public int frozenBodyTextureID;

    [HideInInspector] public int shards = 0;

    public enum CharacterType
    {
        Player,
        NPC,
    }

    // list of available abilities

    public float maxStamina;
    public float staminaRegenRate = 0f;
    private float fatigueThreshold = 0.1f;
    private float fatigueRecoveryThreshold = 0.2f;
    private float fatigueStaminaRegenDebuff = 0.5f;

    protected new void Start()
    {
        base.Start();
        SpawnStaminaBar();
        character.SetData(this);
        UpdateStaminaBar();

        this.visualManager = new(this); //initiate visual
    }

    protected new void Update()
    {
        base.Update();

        Timers();

        StaminaRegen();

        CheckConditionEffects();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public new Character Save()
    {
        character.UpdateData(this);
        return character;
    }

    public void Load(Character data)
    {
        character = data;
    }

    protected void SpawnStaminaBar()
    {
        if (staminaBarObj == null && isStaminaBar == true)
        {
            GameObject prefab = Resources.Load<GameObject>(staminaBarPrefabPath);
            staminaBarObj = Instantiate(prefab, this.gameObject.transform.position + new Vector3(0, staminaBarHeight, 0), Quaternion.identity, this.transform);
            staminaBar = staminaBarObj.GetComponent<StaminaBar>();
        }
    }

    protected void UpdateStaminaBar()
    {
        if (staminaBarObj != null)
        {
            staminaBar.SetValue(character.Stamina, character.MaxStamina);
        }
    }

    private void StaminaRegen()
    {
        if (character.Stamina < character.MaxStamina && !isKnockedOut && !isFrozen)
        {
            if (character.Stamina < character.MaxStamina * fatigueThreshold)
            {
                isFatigued = true;
            }

            else if (character.Stamina >= character.MaxStamina * fatigueRecoveryThreshold)
            {
                isFatigued = false;
            }

            if (isFatigued)
            {
                ModifyStamina(staminaRegenRate * Time.deltaTime * fatigueStaminaRegenDebuff);
            }

            else
            {
                ModifyStamina(staminaRegenRate * Time.deltaTime);
            }

            UpdateStaminaBar();
        }
    }

    public new void Hit(AbilityStatForm abilityStatForm, IAbilityCaster caster)
    {
        base.Hit(abilityStatForm, caster);

        ModifyStamina(abilityStatForm.Stamina);
        CheckOnHitDebuffs(abilityStatForm); // does ability have disarm effect
        UpdateStaminaBar();
    }

    public new void SelfEffect(AbilityStatForm abilityStatForm)
    {
        base.SelfEffect(abilityStatForm);

        ModifyStamina(abilityStatForm.Stamina);

        UpdateStaminaBar();
    }

    public new void ItemStatEffect(ItemStats itemStats)
    {
        base.ItemStatEffect(itemStats);

        //ItemData itemData = ItemLibrary.GetItemDataOriginalByID(itemID);
        //ItemData.ItemStats itemStats = itemData.itemStats;

        ModifyStamina(itemStats.stamina);

        character.physicalPower += itemStats.physicalPower;
        character.spiritPower += itemStats.spiritPower;
        shards += itemStats.shards;

        UpdateStaminaBar();
    }

    private void ModifyStamina(float value)
    {
        character.Stamina = Mathf.Clamp(character.Stamina + value, 0f, character.MaxStamina);
    }

    public bool CheckIfEnoughStamina(float value)
    {
        return character.Stamina + value >= 0;
    }

    private void CheckOnHitDebuffs(AbilityStatForm form)
    {
        if (form.IsDisarming && isFatigued)
        {
            isKnockedOut = true;
        }

        if (form.IsFreezing && isFatigued)
        {
            isFrozen = true;
        }

        if (form.KnockdownDuration > 0)
        {
            ApplyKnockdown(form.KnockdownDuration);
        }

        if (form.StunDuration > 0)
        {
            ApplyStun(form.StunDuration);
        }
    }

    private void CheckConditionEffects()
    {
        if (isKnockedDown || isStunned || isFrozen || isKnockedOut)
        {
            isIncapacitated = true;
        }

        else
        {
            isIncapacitated = false;
        }


        if (isFatigued)
        {
            isSlowed = true;
        }

        else
        {
            isSlowed = false;
        }
    }

    private void ApplyKnockdown(float duration)
    {
        if (duration > knockdownCurrTime)
        {
            isKnockedDown = true;
            knockdownTime = duration;
            knockdownCurrTime = 0;
        }

        isAbilityInterrupt = true;
    }

    private void ApplyStun(float duration)
    {
        if (duration > stunCurrTime)
        {
            isStunned = true;
            stunTime = duration;
            stunCurrTime = 0;
        }

        isAbilityInterrupt = true;
    }

    private void Timers()
    {
        KnockdownTimer();
        StunTimer();
    }

    private float knockdownCurrTime = 0;
    private float knockdownTime = 0;
    private void KnockdownTimer()
    {
        if (isKnockedDown)
        {
            knockdownCurrTime += Time.deltaTime;
            if (knockdownCurrTime >= knockdownTime)
            {
                knockdownCurrTime = 0;
                knockdownTime = 0;
                isKnockedDown = false;
            }
        }
    }

    private float stunCurrTime = 0;
    private float stunTime = 0;
    private void StunTimer()
    {
        if (isStunned)
        {
            stunCurrTime += Time.deltaTime;
            if (stunCurrTime >= stunTime)
            {
                stunCurrTime = 0;
                stunTime = 0;
                isStunned = false;
            }
        }
    }


    public new void UpdateCasterStatForm()
    {
        base.UpdateCasterStatForm();

        casterStatForm.Stamina = character.Stamina;

        casterStatForm.physicalPower = character.physicalPower;
        casterStatForm.spiritPower = character.spiritPower;
    }

    public new void UpdateHudStats(HudManager hudManager)
    {
        base.UpdateHudStats(hudManager);

        hudManager.stamina = character.Stamina;
        hudManager.maxStamina = character.MaxStamina;
    }

    public IAbility ObtainCurrentlyCastingAbility()
    {
        foreach (var ability in abilityList)
        {
            if (ability.isCasting)
            {
                return ability;
            }
        }
        return null;
    }

    public IAbility ObtainCurrentlyExecutingAbility()
    {
        foreach (var ability in abilityList)
        {
            if (ability.isAbilityExecuting)
            {
                return ability;
            }
        }
        return null;
    }

    public bool IsAbilityCasting()
    {
        foreach (var ability in abilityList)
        {
            if (ability.isCasting)
            {
                return true;
            }
        }
        return false;
    }

    protected void DetectOwnMovement()
    {
        visualManager.isMoving = newMovementVector != Vector2.zero ? true : false;

        if (GameMaster.isPlayerBusy)
        {
            visualManager.isMoving = false;
        }
    }

    public new void UpdateInventoryStats(InventoryManager manager)
    {
        base.UpdateInventoryStats(manager);

        manager.physicalPower = character.physicalPower;
        manager.spiritualPower = character.spiritPower;

        manager.shards = shards;
    }
}
