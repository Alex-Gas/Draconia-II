using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MartialChainAttack : Ability, IAbility 
{
    private GameObject prefab;

    public MartialChainAttack()
    {
        UpdateBaseStats();
        prefab = Resources.Load<GameObject>("Prefabs/AbilityEffects/HitScan");
        iconPath = "Textures/UI/martial_attack_icon";
        iconBckgrPath = "Textures/UI/AbilityIconBckgrDragon";
        abAnimID = 4;
    }



    public void UpdateAbility()
    {
        CooldownTimer();
        CastTimer();
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
            stamina = -3,
            staminaCost = -3,
            stunDuration = 1,
            isDisarming = true,

            selfKnockbackStr = 10f,
            selfKnockbackDur = 0.05f,

            cooldownTime = 0.4f,
            castTime = 0.1f,

            levelRequirement = 1,

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

        GameObject hitScanObj = MonoBehaviour.Instantiate(this.prefab, GetCastOrigin(), Quaternion.identity);

        HitScanEffect hitScanEffect = hitScanObj.GetComponent<HitScanEffect>();

        hitScanEffect.Prepare(forwardForm, this, aimDirection);
    }

    private AbilityStatForm CalculateAbilityStats()
    {
        return new AbilityStatForm
        {
            Stamina = baseStats.stamina - casterStatForm.spiritPower,
            StunDuration = baseStats.stunDuration,
            IsDisarming = baseStats.isDisarming,
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

}
