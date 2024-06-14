using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuryBlastAttack : Ability, IAbility
{

    private GameObject prefab;

    public FuryBlastAttack()
    {
        UpdateBaseStats();
        prefab = Resources.Load<GameObject>("Prefabs/AbilityEffects/SplashEffect");
        iconPath = "Textures/UI/fury_blast_icon";
        iconBckgrPath = "Textures/UI/AbilityIconBckgrHuman";
        abAnimID = 3;
    }

    protected void CastTimer()
    {
        if (isCasting)
        {
            caster.isMoveAllowed = false;
            if (caster.isAbilityInterrupt)
            {
                castTime = 0;
                isCasting = false;
                caster.isMoveAllowed = true;
                //Debug.Log("ability interrupted");
            }
            else
            {
                castTime += Time.deltaTime;
                if (castTime >= baseStats.castTime)
                {
                    castTime = 0;
                    isCasting = false;
                    isOnCooldown = true;

                    caster.isMoveAllowed = true;

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
            health = -15f,
            healthCost = -10f,
            knockbackStr = 10f,
            knockbackDur = 0.3f,
            attackRange = 3f,

            knockdownDuration = 3f,
            levelRequirement = 3,

            cooldownTime = 15f,
            castTime = 0.5f,

            effectVisualLingerDuration = 0.4f,
        };

    }

    public void UpdateAbility()
    {
        CooldownTimer();
        CastTimer();

        ExecutingDurationActions();
    }

    public void ExecuteAbility()
    {
        if (!isOnCooldown && !isCasting && casterStatForm.AimTarget != null && caster.CheckIfEnoughHealth(baseStats.healthCost))
        {
            isCasting = true;
            CastTimer();
        }
    }

    private void CastAbility()
    {
        AbilityStatForm forwardForm = CalculateAbilityStats();
        AbilityStatForm returnForm = CalculateSelfEffect();

        caster.SelfEffect(returnForm);

        GameObject effectObj = MonoBehaviour.Instantiate(this.prefab, GetCastOrigin(), Quaternion.identity);

        SplashEffect script = effectObj.GetComponent<SplashEffect>();
        script.Prepare(forwardForm, this, aimDirection);

    }

    private AbilityStatForm CalculateAbilityStats()
    {
        return new AbilityStatForm
        {
            Health = baseStats.health - casterStatForm.physicalPower,
            KnockdownDuration = baseStats.knockdownDuration,

            KnockbackDur = baseStats.knockbackDur,
            KnockbackStr = baseStats.knockbackStr,
        };
    }

    private AbilityStatForm CalculateSelfEffect()
    {
        return new AbilityStatForm
        {
            Health = baseStats.healthCost,
        };
    }

    private void ExecutingDurationActions()
    {
        if (!isCasting)
        {
            caster.isMoveAllowed = !isAbilityExecuting;
        }
    }
}
