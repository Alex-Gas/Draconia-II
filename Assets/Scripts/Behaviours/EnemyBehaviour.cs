using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : CharacterBehaviour
{
    protected Enemy enemy = new();

    private float goalTolerance = 0.05f;
    private Queue<Vector2> waypointQueue = new Queue<Vector2>();

    public List<IHittable> enemyTarget = new();

    [HideInInspector] public bool isAttack = false;

    List<int> itemsToDropIDs = new() { 3, };

    public float viewRange;


    protected new void Start()
    {
        base.Start();
        enemy.SetData(this);

        DefaultQueue();

        UpdateHealthBar();

        SetItemsForDrop();
    }
    protected new void Update()
    {
        base.Update();

        UpdateAbilities();

        CheckMove();
        DetectOwnMovement();
        CheckAbility();

    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
    }   

    private void CheckMove()
    {
        if (isInputAllowed && !isIncapacitated)
        {
            newMovementVector = GetMovementVelocityVector();
        }
    }

    private Vector2 GetMovementVelocityVector()
    {

        if (waypointQueue.Count > 0 && !CheckGoalTolerance())
        {
            return CreateMovementVelocityVector(waypointQueue.Peek());
        }

        else 
        {
            if (waypointQueue.Count > 1)
            {
                RemoveQueueWaypoint();
                return GetMovementVelocityVector();
            }

            else
            {
                return new Vector2(0, 0);
            }
        }
    }

    private Vector2 CreateMovementVelocityVector(Vector2 targetVector)
    {
        float horizontalValue = targetVector.x - animateRigidbody.position.x;
        float verticalValue = (targetVector.y - animateRigidbody.position.y);

        Vector2 normalizedVector = new Vector2(horizontalValue, verticalValue).normalized;

        Vector2 velocityVector = normalizedVector * animate.SpeedMultiplier;

        return velocityVector;
    }

    public void AddQueueWaypoint(Vector2 point)
    {
        waypointQueue.Enqueue(point);
    }

    public void RemoveQueueWaypoint()
    {
        waypointQueue.Dequeue();
    }

    private void ClearQueue()
    {
        this.waypointQueue = new Queue<Vector2>();
    }

    public void ReplaceQueue(Queue<Vector2> incomingQueue)
    {
        if (incomingQueue.Count > 0)
        {
            ClearQueue();
            foreach (var point in incomingQueue)
            {
                AddQueueWaypoint(point);
            }
        }

        else
        {
            DefaultQueue();
        }
    }

    // check if entity within goal
    private bool CheckGoalTolerance()
    {
        float xDiff = waypointQueue.Peek().x - animateRigidbody.position.x;
        float yDiff = waypointQueue.Peek().y - animateRigidbody.position.y;

        return xDiff >= -goalTolerance & xDiff <= goalTolerance & yDiff >= -goalTolerance & yDiff <= goalTolerance;
    }

    private void DefaultQueue()
    {
        this.waypointQueue = new Queue<Vector2>();
        waypointQueue.Enqueue(new Vector2(animateRigidbody.position.x, animateRigidbody.position.y));
    }

    // a bit manual but will do for now
    public bool IsEnemyFaction(EntityBehaviour.Faction casterFaction)
    {
        if (casterFaction == Faction.Player)
        {
            return true;
        }
        else if (casterFaction == Faction.Enemy)
        {
            return false;
        }
        else
        {
            return false;
        }

    }

    private void CheckAbility()
    {
        if (isAttack && !IsAbilityCasting() && !isIncapacitated)
        {
            CallAbility(abilityList[0]);
        }
        
        isAttack = false;
    }

    // this must be called before any target usage
    public void SetAimTarget(Vector2? point)
    {
        aimTarget = point;
    }


    public void CallAbility<T>(T ability) where T : IAbility
    {
        //Debug.Log("ExecutingAbility");
        ability.ExecuteAbility();
    }

    public void UpdateAbilities()
    {
        UpdateCasterStatForm();

        foreach (var ability in abilityList)
        {
            ability.UpdateAbility();
        }
        isAbilityInterrupt = false;
    }

    public new void UpdateCasterStatForm()
    {
        base.UpdateCasterStatForm();
    }

    public new void Hit(AbilityStatForm abilityStatForm, IAbilityCaster caster)
    {
        base.Hit(abilityStatForm, caster);
    }

    // this function applies effects inflicted on itself 
    public new void SelfEffect(AbilityStatForm abilityStatForm)
    {
        base.SelfEffect(abilityStatForm);
    }

    protected void SetItemsForDrop()
    {
        foreach (int ID in itemsToDropIDs)
        {
            ItemData itemData = ItemLibrary.LibraryItemRetriever(ID);
            itemsForDrop.Add(itemData);
        }
    }
}
