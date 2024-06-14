using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickDash : Ability, IAbility
{
    private GameObject prefab;

    public KickDash()
    {
        UpdateBaseStats();
        prefab = Resources.Load<GameObject>("Prefabs/AbilityEffects/HitScan");
        iconPath = "Textures/UI/kick_dash_icon";
        iconBckgrPath = "Textures/UI/AbilityIconBckgrHuman";
        abAnimID = 2;
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
            health = -8,
            staminaCost = -6,
            stunDuration = 1,

            knockbackStr = 8f,
            knockbackDur = 0.1f,

            selfKnockbackStr = 8f,
            selfKnockbackDur = 0.3f,

            cooldownTime = 3f,
            castTime = 0.2f,

            effectVisualLingerDuration = 0.3f,

            levelRequirement = 2,
        };
    }

    public void ExecuteAbility()
    {
        if (!isOnCooldown && !isCasting && casterStatForm.AimTarget != null && caster.CheckIfEnoughStamina(baseStats.staminaCost))
        {
            isCasting = true;
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

        AbilityStatForm forwardForm = CalculateAbilityStats();
        forwardForm.KnockbackDir = ((Vector2)casterStatForm.AimTarget - (Vector2)casterGameObject.transform.position).normalized;

        AbilityStatForm returnForm = CalculateSelfEffect();
        returnForm.KnockbackDir = -((Vector2)casterStatForm.AimTarget - (Vector2)casterGameObject.transform.position).normalized;

        caster.SelfEffect(returnForm);

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
        //caster.isMoveAllowed = !isAbilityExecuting;
    }
}
