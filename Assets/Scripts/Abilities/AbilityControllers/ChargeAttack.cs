using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : Ability, IAbility
{
    private GameObject prefab;

    public ChargeAttack()
    {
        UpdateBaseStats();
        prefab = Resources.Load<GameObject>("Prefabs/AbilityEffects/HitScan");
        iconPath = "Textures/UI/AbilityIcon3";
        iconBckgrPath = "Textures/UI/AbilityIconBckgrHuman";
        abAnimID = 7;
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
                //Debug.Log("charge attack interrupt");
            }
            else
            {
                castTime += Time.deltaTime;
                if (castTime >= baseStats.castTime)
                {
                    castTime = 0;
                    isCasting = false;
                    isOnCooldown = true;
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
            health = -15,
            staminaCost = -7,
            stunDuration = 0.2f,
            attackRange = 2f,

            selfKnockbackStr = 10f,
            selfKnockbackDur = 0.1f,

            knockbackStr = 8f,
            knockbackDur = 0.05f,

            cooldownTime = 2f,
            castTime = 0.4f,

            effectVisualLingerDuration = 0.2f,
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
            Health = baseStats.health,
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
