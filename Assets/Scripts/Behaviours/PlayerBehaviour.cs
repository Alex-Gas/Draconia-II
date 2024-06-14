using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : CharacterBehaviour, ISaveable<Player>, IHittable, IAbilityCaster, ITalkable
{
    protected Player player = new();

    protected HudManager hudManager;
    protected DialogManager dialogManager;
    protected InventoryManager inventoryManager;

    protected List<IAbility> humanAbilityList { get; set; } = new();
    protected List<IAbility> dragonAbilityList { get; set; } = new();

    private List<int> startingItemsIDs = new List<int>() { };

    private List<GameObject> allInteractables = new();
    private List<GameObject> accessibleInteractables = new();
    private GameObject selectedInteractable;

    private float interactableDetectRange = 8f;
    private float interactRange = 2f;

    [SerializeField] private int totalXP;
    private int humanXP;
    private int dragonXP;

    [SerializeField] private int level = 1;
    [SerializeField] private int humanLevel = 1;
    [SerializeField] private int dragonLevel = 1;
    private int nextLevelXpGoal = 800;

    public PlayerForm playerForm = PlayerForm.Human;

    private Vector2 defaultRespawnLocation = new Vector2(0,0);
    public GameObject checkpointObj = null;

    //private bool isSoulSatisfied = true;

    public enum PlayerForm
    {
        Human,
        Dragon,
    }

    protected new void Start()
    {
        base.Start();
        player.SetData(this);

        // initiate hud must be after every other intiialization
        InitiateHudManager();
        InitiateDialogManager();
        this.inventoryManager = new(this); //initiate inventory

        SetupAbilities();
        SelectAbilitySet();

        UpdateHealthBar();

        AddItemsToInventory(startingItemsIDs);
        AddCurrency(400);

    }

    protected new void Update()
    {
        if (!GameMaster.isGamePaused)
        {
            base.Update();

            FormChangeCooldown();

            UpdateAbilities();


            ScanForInteractables();

            CheckInputs();

            visualManager.ApplyFormVisual(playerForm);
            visualManager.ApplyOrientationVisual(aimTarget, newMovementVector);
            visualManager.ApplyAbilityVisual();

            UpdateHudManager();

        }
    }

    protected new void FixedUpdate()
    {
        if (!GameMaster.isGamePaused)
        {
            base.FixedUpdate();
        }
        if (GameMaster.isGamePaused || GameMaster.isPlayerBusy)
        {
            animateRigidbody.velocity = Vector2.zero;
            
        }
    }

    public new Player Save()
    {
        player.UpdateData(this);
        return player;
    }

    public void Load(Player data)
    {
        player = data;
    }


    // ----- INPUTS
    private void CheckInputs()
    {
        CheckMoveInput();
        CheckAbilityInput();
        CheckMiscInput();

        DetectOwnMovement();
    }


    private void CheckMoveInput()
    {
        if (isInputAllowed && !isIncapacitated && !GameMaster.isPlayerBusy)
        {
            newMovementVector = GetMovementVelocityVector();

            if (isSlowed)
            {
                newMovementVector *= slowdownValue;
            }
        }
    }

    private void CheckAbilityInput()
    {
        SetAimTarget(GameMaster.mouseScreenToWorld);

        if (!isIncapacitated && !GameMaster.isPlayerBusy)
        {
            if (GameMaster.ability1Input)
            {
                CallAbility(abilityList[0]);
            }

            else if (GameMaster.ability2Input)
            {
                CallAbility(abilityList[1]);
            }

            else if (GameMaster.ability3Input)
            {
                CallAbility(abilityList[2]);
            }
        }
    }

    private void CheckMiscInput()
    {
        if (GameMaster.inventoryInput)
        {
            inventoryManager.ToggleInventory();
        }

        if (!GameMaster.isPlayerBusy)
        {
            if (GameMaster.interactInput)
            {
                PerformInteraction();
            }

            if (GameMaster.formSwitchInput)
            {
                TogglePlayerForm();
            }
        }
    }

    private Vector2 GetMovementVelocityVector()
    {
        if (Input.anyKey)
        {
            float horizontalValue = GameMaster.horizontalInput;
            float verticalValue = GameMaster.verticalInput * player.verticalMultiplier;

            Vector2 normalizedVector = new Vector2(horizontalValue, verticalValue).normalized;

            Vector2 velocityVector = normalizedVector * animate.SpeedMultiplier;

            return velocityVector;
        }
        else
        {
            return new Vector2(0, 0);
        }
    }

    public void AddItemsToInventory(List<int> itemsIDs)
    {
        inventoryManager.AddItemsToInventoryByID(itemsIDs);
    }

    public bool CheckIfItemInInventory(int id)
    {
        return inventoryManager.CheckIfItemInInventory(id);
    }

    public void AddCurrency(int amount)
    {
        shards += amount;
    }

    public void SetupAbilities()
    {
        {
            SwordChainAttack swordChainAttack = new();
            KickDash kickDash = new();
            FuryBlastAttack furyBlastAttack = new();

            humanAbilityList = new List<IAbility> { swordChainAttack, kickDash, furyBlastAttack };
            foreach (var ability in humanAbilityList)
            {
                ability.InitiateAbility(casterStatForm, this);
            }
        }
        {
            MartialChainAttack martialChainAttack = new();
            DashAbility dashAbility = new();
            FreezeBlastAttack freezeBlastAttack = new();

            dragonAbilityList = new List<IAbility> { martialChainAttack, dashAbility, freezeBlastAttack };
            foreach (var ability in dragonAbilityList)
            {
                ability.InitiateAbility(casterStatForm, this);
            }
        }


        CheckIfAbilityAvailable();
    }

    private void SelectAbilitySet()
    {
        switch (playerForm)
        {
            case PlayerForm.Human:
                abilityList.Clear();
                foreach (var ability in humanAbilityList)
                {
                    abilityList.Add(ability);
                }
                break;
            case PlayerForm.Dragon:
                abilityList.Clear();
                foreach (var ability in dragonAbilityList)
                {
                    abilityList.Add(ability);
                }
                break;
        }

        hudManager.RedrawAbilityIcons();
    }

    private void TogglePlayerForm()
    {
        if (!IsAbilityCasting())
        {
            switch (playerForm)
            {
                case PlayerForm.Human:
                    if (!isOnFormCooldown)
                    {
                        playerForm = PlayerForm.Dragon;
                        isOnFormCooldown = true;
                        formCooldownCurrTime = 0;
                    }
                    break;

                case PlayerForm.Dragon:
                    if (!isOnFormCooldown)
                    {
                        playerForm = PlayerForm.Human;
                        isOnFormCooldown = true;
                        formCooldownCurrTime = 0;
                    }
                    break;
            }

            SelectAbilitySet();
        }
    }

    private bool isOnFormCooldown = false;
    private float formCooldownCurrTime = 10f;
    public float formCooldownTime = 10f;
    private void FormChangeCooldown()
    {
        if (isOnFormCooldown)
        {
            formCooldownCurrTime += Time.deltaTime;
            if (formCooldownCurrTime >= formCooldownTime)
            {
                isOnFormCooldown = false;
            }
        }
    }

    private void InitiateHudManager()
    {
        this.hudManager = new(this);
    }

    private void InitiateDialogManager()
    {
        this.dialogManager = new(this);
    }

    // this must be called before any target usage
    public void SetAimTarget(Vector2? point)
    {
        aimTarget = point;
    }

    private void ScanForInteractables()
    {
        allInteractables.Clear();
        accessibleInteractables.Clear();
        selectedInteractable = null;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, interactableDetectRange);
        if (colliders.Length > 0)
        {
            foreach (Collider2D collider in colliders)
            {
                IInteractable interactableScript = collider.gameObject.GetComponentInParent<IInteractable>();
                if (collider.gameObject.layer == LayerMask.NameToLayer("InteractBox") && interactableScript != null)
                {
                    allInteractables.Add(collider.gameObject);

                    bool isVisible = true;
                    RaycastHit2D[] hits = CheckIfInteractableAvailable(collider.gameObject);

                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                        {
                            isVisible = false;
                            break;
                        }
                    }

                    if (isVisible)
                    {
                        interactableScript.ToggleNamePlateVisible();

                        if (Vector2.Distance(collider.gameObject.transform.position, transform.position) < interactRange)
                        {
                            accessibleInteractables.Add(collider.gameObject);
                        }
                    }
                }
            }
        }

        if (accessibleInteractables.Count > 0)
        {
            selectedInteractable = accessibleInteractables[0];

            for (int i = 1; i < accessibleInteractables.Count; i++)
            {
                if (Vector2.Distance(accessibleInteractables[i].transform.position, transform.position) < Vector2.Distance(selectedInteractable.transform.position, transform.position))
                {
                    selectedInteractable = accessibleInteractables[i];
                }
            }
        }
        
        if (selectedInteractable != null)
        {
            IInteractable script = selectedInteractable.GetComponentInParent<IInteractable>();
            script.HighlightNamePlate();
        }

        //Debug.Log("Colliders detected: " + colliders.Length);
        //Debug.Log("All interactables count: " + allInteractables.Count);
        //Debug.Log("Accessible interactable count: " + accessibleInteractables.Count);
    }

    private RaycastHit2D[] CheckIfInteractableAvailable(GameObject target)
    {
        Vector2 startPoint = gameObject.transform.position;
        Vector2 endPoint = target.transform.position;
        Vector2 direction = endPoint - startPoint;
        RaycastHit2D[] hits = Physics2D.RaycastAll(startPoint, direction, direction.magnitude);
        return hits;
    }

    private void PerformInteraction()
    {
        if (selectedInteractable != null)
        {
            IInteractable script = selectedInteractable.GetComponentInParent<IInteractable>();
            script.OnInteract(this);
        }
    }

    // a bit manual but will do for now
    public bool IsEnemyFaction(EntityBehaviour.Faction casterFaction)
    {
        if (casterFaction == Faction.Enemy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // THIS IS NOW SUPPOSED TO BE LOCATED AT THE END OF A TREE ONLY.
    // Instead the UpdateCasterStatForm is to be the function that goes down and collects relevant data from Entity, Animate etc
    // Don't pass native casterStatForm here. Do it at the creation of abilities.
    public void CallAbility<T>(T ability) where T : IAbility
    {
        if (ability.isAvailable && !IsAbilityCasting())
        {
            ability.ExecuteAbility();
        }
    }

    public new void Hit(AbilityStatForm abilityStatForm, IAbilityCaster caster)
    {
        base.Hit(abilityStatForm, caster);

        if (isHealthZero)
        {
            ReturnToCheckpoint();
        }

        else if (isKnockedOut)
        {
            ReturnToCheckpoint();
        }
        //UpdateHudManagerEvent();
    }

    // this function applies effects inflicted on itself 
    public new void SelfEffect(AbilityStatForm abilityStatForm)
    {
        base.SelfEffect(abilityStatForm);

        //UpdateHudManagerEvent();
    }

    // This is to update every ability effect with newest version of casterStatForm.
    // This way if this object gets destroyed all effects retain the last existing version of casterStatForm
    public void UpdateAbilities()
    {
        UpdateCasterStatForm();

        foreach (var ability in humanAbilityList)
        {
            ability.UpdateAbility();
        }
        foreach (var ability in dragonAbilityList)
        {
            ability.UpdateAbility();
        }

        isAbilityInterrupt = false;
    }

    // IMPORTANT
    // PROTOTYPE. This method could be called either every frame or anytime an ability needs the latest caster stats
    public new void UpdateCasterStatForm()
    {
        base.UpdateCasterStatForm();
    }


    public void UpdateHudManager()
    {
        UpdateHudStats(this.hudManager);     // this here if I want hud manager stats like current health to be updated every frame instead of whenever changes occur
        this.hudManager.UpdateHud();
    }

    public new void UpdateHudStats(HudManager hudManager)
    {
        base.UpdateHudStats(hudManager);
        hudManager.humanXP = humanXP;
        hudManager.dragonXP = dragonXP;
        hudManager.formCooldownCurrTime = formCooldownCurrTime;
    }

    public bool PickupItem(ItemData item)
    {
        if (itemsInPosession.Count < 18 )
        {
            inventoryManager.AddItemToInventory(item);
            return true;
        }
        return false;       
    }

    public void RequestConversation(Conversation conversation, ITalkable talker)
    {
        dialogManager.InitiateDialog(conversation, talker);
    }

    public void ModifyXp(int xp)
    {
        switch (playerForm)
        {
            case PlayerForm.Human:
                humanXP += xp;
                totalXP += xp;
                //Debug.Log("human xp modified by: " + xp);
                break;
            case PlayerForm.Dragon:
                dragonXP += xp;
                totalXP += xp;
                //Debug.Log("dragon xp modified by: " + xp);
                break;
        }

        if (totalXP >= nextLevelXpGoal)
        {
            level++;
            if (humanXP > dragonXP)
            {
                humanLevel++;
            }
            else if (dragonXP > humanXP)
            {
                dragonLevel++;
            }
            else
            {
                //Debug.Log("xp of human and dragon are even. Increasing human level by default.");
                humanLevel++;
            }

            dragonXP = 0;
            humanXP = 0;
            nextLevelXpGoal += 800;
        }



        CheckIfAbilityAvailable();
    }

    private void CheckIfAbilityAvailable()
    {
        foreach (var ability in humanAbilityList)
        {
            if (ability.baseStats.levelRequirement <= humanLevel)
            {
                ability.isAvailable = true;
            }
            else { ability.isAvailable = false; }
        }

        foreach (var ability in dragonAbilityList)
        {
            if (ability.baseStats.levelRequirement <= dragonLevel)
            {
                ability.isAvailable = true;
            }
            else { ability.isAvailable = false; }
        }

        hudManager.RedrawAbilityIcons();
    }

    public void SetCheckpointLocation(GameObject checkpointObj)
    {
        if (checkpointObj != this.checkpointObj)
        {
            this.checkpointObj = checkpointObj;
            Debug.Log("checkpoint set at: " + checkpointObj.transform.position);
        }
    }

    public void ReturnToCheckpoint()
    {
        if (checkpointObj != null)
        {
            transform.position = new Vector3(checkpointObj.transform.position.x, checkpointObj.transform.position.y, -5);
        }
        else
        {
            transform.position = new Vector3(defaultRespawnLocation.x, defaultRespawnLocation.y, -5);
        }
        ModifyHealth(10f, 0f);
        Debug.Log("returning to checkpoint");
    }

    public new void UpdateInventoryStats(InventoryManager manager)
    {
        base.UpdateInventoryStats(manager);
        manager.totalXP = totalXP;
        manager.humanXP = humanXP;
        manager.dragonXP = dragonXP;
        manager.nextLevelXpGoal = nextLevelXpGoal;

        manager.level = level;
        manager.humanLevel = humanLevel;
        manager.dragonLevel = dragonLevel;
    }

    public void SetupQuest()
    {
        GameMaster.talkedToKharim = true;
        hudManager.SetQuestMessage();
    }

}
