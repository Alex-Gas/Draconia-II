using System.Collections.Generic;
using UnityEngine;


public class EntityBehaviour : MonoBehaviour, ISaveable<Entity>
{
    protected Entity entity = new();

    public GameObject entityGameObject { get; set; }
    public GameObject hitBoxObject { get; set; }
    public GameObject terrainBoxObject { get; set; }
    public GameObject modelObject { get; set; }

    public string entityDisplayName;
    private GameObject namePlatePrefab;
    protected GameObject namePlate;
    public float namePlateHeight;
    public bool isNamePlateAllowed;

    protected CasterStatForm casterStatForm = new();

    public List<IAbility> abilityList { get; set; } = new();
    public IAbility selectedAbility { get; set; }

    public Vector2? aimTarget { get; set; } = null;

    protected bool isHealthZero;
    public bool isAbilityInterrupt { get; set; } = false;

    public int xpValue;

    protected GameObject healthBar;
    private string healthBarPrefabPath = "Prefabs/UI/UnitHealthBar";
    public bool isHealthBar = false;
    public float healthBarHeight;

    public int maxHealth;
    public Faction faction;
    public Vector2 abilityCastOrigin;
    public bool isGod = false;

    public int conversationID;
    protected Conversation conversation;

    public bool isMoveAllowed { get; set; } = true;

    protected bool isFatigued;
    protected bool isKnockedOut;
    protected bool isKnockedDown;
    protected bool isSlowed;
    protected bool isStunned;
    protected bool isFrozen;
    protected bool isIncapacitated;

    protected List<ItemData> itemsForDrop = new(); 


    // faction that the entity belongs to
    public enum Faction
    {
        None,
        Player,
        Enemy,
        NPC,
    }

    protected void Awake()
    {
        entityGameObject = this.gameObject;
    }

    protected void Start()
    {
        SpawnHealthBar();
        GrabReferences();
        entity.SetData(this);
        
        UpdateHealthBar();

        LoadConversation();

        SpawnNamePlate();
    }

    protected void Update()
    {

    }

    protected void FixedUpdate()
    {

    }
    
    private void GrabReferences()
    {
        entityGameObject = this.gameObject;

        Transform hitBoxTransform = transform.Find("HitBox");
        if (hitBoxTransform != null)
        {
            hitBoxObject = hitBoxTransform.gameObject;
        }

        Transform terrainBoxTransform = transform.Find("TerrainBox");
        if (terrainBoxTransform != null)
        {
            terrainBoxObject = terrainBoxTransform.gameObject;
        }

        Transform modelTransform = transform.Find("Model");
        if (terrainBoxTransform != null)
        {
            terrainBoxObject = terrainBoxTransform.gameObject;
        }

        namePlatePrefab = Resources.Load<GameObject>("Prefabs/UI/NamePlate");
    }
    
    public Entity Save()
    {
        entity.UpdateData(this);
        return entity;
    }

    public void Load(Entity data)
    {
        entity = data;
    }

    protected void SpawnNamePlate()
    {
        if (isNamePlateAllowed)
        {
            namePlate = Instantiate(namePlatePrefab, transform.position + new Vector3(0, namePlateHeight, 0), Quaternion.identity, transform);
            NamePlate script = namePlate.GetComponent<NamePlate>();
            script.entityName = entityDisplayName;
        }
    }

    protected void SpawnHealthBar()
    {
        if (healthBar == null && isHealthBar == true)
        {
            GameObject prefab = Resources.Load<GameObject>(healthBarPrefabPath);
            healthBar = Instantiate(prefab, this.gameObject.transform.position + new Vector3(0, healthBarHeight, 0), Quaternion.identity, this.transform);
        }
    }

    protected void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            HealthBar healthBarScript = healthBar.GetComponent<HealthBar>();
            healthBarScript.SetValue(entity.Health, entity.MaxHealth);
        }
    }

    public void Hit(AbilityStatForm abilityStatForm, IAbilityCaster caster)
    {
        float health = 0;
        if (abilityStatForm.Health < 0)
        {
            health = Mathf.Min(abilityStatForm.Health + entity.armor, -1);
        }
        else
        {
            health = abilityStatForm.Health;
        }


        ModifyHealth(health, 0f);
        CheckIfHealthZero();

        UpdateHealthBar();

        Debug.Log(this.name + " hit for: " + health);
    }

    public void SelfEffect(AbilityStatForm abilityStatForm)
    {
        ModifyHealth(abilityStatForm.Health, 0.1f);

        UpdateHealthBar();
    }

    public void ItemStatEffect(ItemStats itemStats)
    {
        ModifyHealth(itemStats.health, 0.1f);

        entity.armor += itemStats.armor;

        UpdateHealthBar();
    }

    protected void ModifyHealth(float value, float minHealth)
    {
        if (!isGod)
        {
            entity.Health = Mathf.Clamp(entity.Health + value, minHealth, entity.MaxHealth);
        }
    }

    public bool CheckIfEnoughHealth (float value)
    {
        return entity.Health + value > 0;
    }

    private void CheckIfHealthZero()
    {
        if (entity.Health <= 0)
        {
            isHealthZero = true;
        }

        else
        {
            isHealthZero = false;
        }
    }

    public void UpdateCasterStatForm()
    {
        casterStatForm.Health = entity.Health;
        casterStatForm.Position = entity.Position;
        casterStatForm.Faction = this.faction;
        casterStatForm.AimTarget = this.aimTarget;
        casterStatForm.AbilityCastOrigin = this.abilityCastOrigin;

        casterStatForm.armor = entity.armor;
    }

    public void UpdateHudStats(HudManager hudManager)
    {
        hudManager.health = entity.Health;
        hudManager.maxHealth = entity.MaxHealth;
    }

    public void LoadConversation()
    {
        if (conversationID > 0)
        {
            conversation = DialogLibrary.GetConversationByID(conversationID);
        }
    }

    protected void DropItems()
    {
        foreach (ItemData itemData in itemsForDrop)
        {
            GameObject prefab = Resources.Load<GameObject>(itemData.prefabPath);
            GameObject droppedItem = MonoBehaviour.Instantiate(prefab, this.transform.position, Quaternion.identity);
            ItemBehaviour script = droppedItem.GetComponent<ItemBehaviour>();
            script.TransfuseItemData(itemData);
            script.OnDrop();

        }

        itemsForDrop.Clear();
    }

    public void SendXp(IAbilityCaster caster)
    {
        caster.ModifyXp(xpValue);

        if (xpValue > 0)
        {
            SpawnXpPopup(caster, xpValue.ToString());
        }

        xpValue = 0;
    }

    private void SpawnXpPopup(IAbilityCaster caster, string xpValue)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/XpPopup");
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
        XpPopup script = obj.GetComponent<XpPopup>();
        script.SetText(xpValue);
    }

    public void ToggleNamePlateVisible()
    {
        if (namePlate != null)
        {
            namePlate.GetComponent<NamePlate>().isMakeVisible = true;
        }
    }

    public void HighlightNamePlate()
    {
        if (namePlate != null)
        {
            namePlate.GetComponent<NamePlate>().isHighlight = true;
        }
    }
    
    public void DialogInteract()
    {

    }

    protected void SpawnEntityBody(string path)
    {
        if (path != null)
        {
            GameObject bodyPrefab = Resources.Load<GameObject>("Prefabs/EntityRemains/" + path);
            Instantiate(bodyPrefab, new Vector3(transform.position.x, transform.position.y, -0.5f), Quaternion.identity);
        }
        else{
            Debug.LogError("Body prefab not found.");
        }
    }

    public void UpdateInventoryStats(InventoryManager manager)
    {
        manager.armor = entity.armor;
    }
}
