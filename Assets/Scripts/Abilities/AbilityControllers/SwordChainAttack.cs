using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordChainAttack : Ability, IAbility
{
    private GameObject prefab;

    public SwordChainAttack()
    {
        UpdateBaseStats();
        prefab = Resources.Load<GameObject>("Prefabs/AbilityEffects/HitScan");
        iconPath = "Textures/UI/sword_swing_icon";
        iconBckgrPath = "Textures/UI/AbilityIconBckgrHuman";
        abAnimID = 1;
    }


    public void UpdateAbility()
    {
        CooldownTimer();
        CastTimer();

        ExecutingDurationActions();
    }

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
            health = -6,
            staminaCost = -5,
            stunDuration = 0.5f,

            selfKnockbackStr = 6f,
            selfKnockbackDur = 0.2f,


            knockbackStr = 9f,
            knockbackDur = 0.05f,

            cooldownTime = 0.8f,
            castTime = 0.2f,

            effectVisualLingerDuration = 0.2f,

            levelRequirement = 1,
        };

    }

    public void ExecuteAbility()
    {
        if (!isOnCooldown && !isCasting && casterStatForm.AimTarget != null && caster.CheckIfEnoughStamina(baseStats.staminaCost))
        {
            isCasting = true;

            //Debug.Log("Casting Projectile...");
            CastTimer();

            AbilityStatForm returnForm = CalculateSelfEffect();
            returnForm.KnockbackDir = ((Vector2)casterStatForm.AimTarget - (Vector2)casterGameObject.transform.position).normalized;
            caster.SelfEffect(returnForm);

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

        AbilityStatForm forwardForm = CalculateAbilityStats();
        forwardForm.KnockbackDir = ((Vector2)casterStatForm.AimTarget - (Vector2)casterGameObject.transform.position).normalized;

        GameObject hitScanObj = MonoBehaviour.Instantiate(this.prefab, GetCastOrigin(), Quaternion.identity);

        HitScanEffect hitScanEffect = hitScanObj.GetComponent<HitScanEffect>();

        hitScanEffect.Prepare(forwardForm, this, aimDirection);
    }



    private AbilityStatForm CalculateAbilityStats()
    {
        return new AbilityStatForm
        {
            Health = baseStats.health - casterStatForm.physicalPower,
            StunDuration = baseStats.stunDuration,

            KnockbackDur = baseStats.knockbackDur,
            KnockbackStr = baseStats.knockbackStr,
        };
    }

    private AbilityStatForm CalculateSelfEffect()
    {
        return new AbilityStatForm
        {
            Stamina = baseStats.staminaCost,

            KnockbackDur = baseStats.selfKnockbackDur,
            KnockbackStr = baseStats.selfKnockbackStr,
        };
    }

    private void ExecutingDurationActions()
    {
        caster.isMoveAllowed = !isAbilityExecuting;
    }
}
