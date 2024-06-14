using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateBehaviour : EntityBehaviour, ISaveable<Animate>
{
    protected Vector2 newMovementVector;

    [HideInInspector] public Rigidbody2D animateRigidbody = null;

    protected Animate animate = new();

    public float speedMultiplier;

    public bool isInputAllowed { get; set; } = true;

    private Vector2? forcedFlashMoveVector;
    private float? forcedFlashMoveTime;

    private bool isFlashInputTimer = false;
    private float flashInputCurrTime = 0;

    protected float slowdownValue = 0.6f;

    public bool isMovementForbidden;

    protected new void Start()
    {
        base.Start();
        animateRigidbody = entityGameObject.GetComponent<Rigidbody2D>();
        animate.SetData(this);

    }

    protected new void Update()
    {
        base.Update();
        FlashMoveTimer();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        AnimateMove();
    }

    public new Animate Save()
    {
        animate.UpdateData(this);
        return animate;
    }

    public void Load(Animate data)
    {
        animate = data;
    }

    // method responsible for executing a movement vector of any object that can move
    // only animate objects can move
    private void AnimateMove()
    {
        Vector2 movementVector;

        if (isMoveAllowed && !isMovementForbidden)
        {
            movementVector = this.newMovementVector;
        }
        else {
            movementVector = Vector2.zero;
        }
        animateRigidbody.velocity = Vector2.zero + movementVector;
    }

    private void ApplyFlashMove(AbilityStatForm abilityStatForm)
    {
        if (!isMovementForbidden)
        {
            isInputAllowed = false;
            forcedFlashMoveVector = abilityStatForm.KnockbackDir * abilityStatForm.KnockbackStr;
            forcedFlashMoveTime = abilityStatForm.KnockbackDur;

            flashInputCurrTime = 0f;
            isFlashInputTimer = true;
        }
    }

    public new void Hit(AbilityStatForm abilityStatForm, IAbilityCaster caster)
    {
        base.Hit(abilityStatForm, caster);

        if (abilityStatForm.KnockbackDir != null && abilityStatForm.KnockbackStr != null && abilityStatForm.KnockbackDur != null)
        {
            //Debug.Log("target forced move");
            ApplyFlashMove(abilityStatForm);
        }
    }

    public new void SelfEffect(AbilityStatForm abilityStatForm)
    {
        base.SelfEffect(abilityStatForm);

        if (abilityStatForm.KnockbackDir != null && abilityStatForm.KnockbackStr != null && abilityStatForm.KnockbackDur != null)
        {
            //Debug.Log("self forced move");
            ApplyFlashMove(abilityStatForm);
        }
    }

    public new void ItemStatEffect(ItemStats itemStats)
    {
        base.ItemStatEffect(itemStats);
    }

    private void FlashMoveTimer()
    {
        if (isFlashInputTimer)
        {
            flashInputCurrTime += Time.deltaTime;
            if (flashInputCurrTime >= forcedFlashMoveTime)
            {
                flashInputCurrTime = 0f;
                isFlashInputTimer = false;
                forcedFlashMoveVector = null;
                forcedFlashMoveTime = null;
                isInputAllowed = true;
                newMovementVector = Vector2.zero;
            }

            else
            {
                newMovementVector = (Vector2)forcedFlashMoveVector;
            }
        }

        else
        {
            flashInputCurrTime = 0f;
            isFlashInputTimer = false;
            forcedFlashMoveVector = null;
            forcedFlashMoveTime = null;
        }
    }



    public new void UpdateCasterStatForm()
    {
        base.UpdateCasterStatForm();
    }

    public new void UpdateHudStats(HudManager hudManager)
    {
        base.UpdateHudStats(hudManager);
    }

    public new void UpdateInventoryStats(InventoryManager manager)
    {
        base.UpdateInventoryStats(manager);
    }
}
