using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : Ability, IAbility
{
    private GameObject prefab;


    public DashAbility()
    {
        UpdateBaseStats();
        iconPath = "Textures/UI/dash_icon";
        iconBckgrPath = "Textures/UI/AbilityIconBckgrDragon";
        abAnimID = 5;
    }


    // Carrying over Update() for counters
    public void UpdateAbility()
    {
        CooldownTimer();
        CastTimer();
        VisualTimer();
    }

    // Cast also needs to check if caster hasn't been affected by interrupt actions like stun or freeze.
    protected void CastTimer()
    {
        if (isCasting)
        {
            if (caster.isAbilityInterrupt)
            {
                castTime = 0;
                isCasting = false;
                //Debug.Log("Projectile cast interrupted!");
            }
            else
            {
                castTime += Time.deltaTime;
                if (castTime >= baseStats.castTime)
                {
                    castTime = 0;
                    isCasting = false;
                    isOnCooldown = true;
                    //Debug.Log("Projectile Fire! Projectile on cooldown...");
                    CastAbility();
                }
            }
        }

        else
        {
            castTime = 0;
        }
    }

    // this is where base stats of abilty are set/updated
    private void UpdateBaseStats()
    {
        baseStats = new()
        {
            staminaCost = -7f,
            cooldownTime = 1f,
            castTime = 0f,
            levelRequirement = 2,

            selfKnockbackStr = 15f,
            selfKnockbackDur = 0.3f,

            effectVisualLingerDuration = 0.3f,
        };
    }


    // this is where ability gets executed
    public void ExecuteAbility()
    {
        if (!isOnCooldown && 
            !isCasting && 
            casterStatForm.AimTarget != null && 
            caster.CheckIfEnoughStamina(GetStaminaCost()))
        {
            isCasting = true;
            isAbilityExecuting = true;
            //Debug.Log("Casting Projectile...");
            CastTimer();
        }
    }


    public void TakeAim()
    {
        Vector2? worldPoint = casterStatForm.AimTarget;
        Vector2 direction;

        if (worldPoint.HasValue)
        {
            direction = worldPoint.Value - (Vector2)casterGameObject.transform.position;
        }

        else
        {
            direction = Vector2.zero;
        }

        aimDirection = direction.normalized;
        abilityOrientation = GetAbilityOrientation(aimDirection);
    }


    private void CastAbility()
    {

        TakeAim();
        isVisual = true;
        visCurrTime = 0;

        AbilityStatForm returnForm = CalculateSelfEffect();

        returnForm.KnockbackDir = ((Vector2)casterStatForm.AimTarget - (Vector2)casterGameObject.transform.position).normalized;

        caster.SelfEffect(returnForm);
    }


    // If this is called at any other time than when the ability gets cast then add checks for null
    private AbilityStatForm CalculateAbilityStats()
    {
        return new AbilityStatForm
        {

        };
    }

    // If this is called at any other time than when the ability gets cast then add checks for null
    private AbilityStatForm CalculateSelfEffect()
    {
        // make list of stat forms if there are multiple different effects happening at different times 

        return new AbilityStatForm
        {
            Stamina = GetStaminaCost(),

            // FOR TESTING
            KnockbackDur = baseStats.selfKnockbackDur,
            KnockbackStr = baseStats.selfKnockbackStr,
        };
    }

    private float GetStaminaCost()
    {
        return Mathf.Min(baseStats.staminaCost + casterStatForm.spiritPower / 2, -1);
    }

    private bool isVisual = false;
    private float visCurrTime = 0;
    private void VisualTimer()
    {
        if (isVisual)
        {
            isAbilityExecuting = true;
            visCurrTime += Time.deltaTime;
            if (visCurrTime >= baseStats.effectVisualLingerDuration)
            {
                isAbilityExecuting = false;
                visCurrTime = 0;
                isVisual = false;
            }
        }
    }

}
