using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBlastAttack : Ability, IAbility
{

    private GameObject prefab;
    public FreezeBlastAttack()
    {
        UpdateBaseStats();
        prefab = Resources.Load<GameObject>("Prefabs/AbilityEffects/FreezeRay");
        iconPath = "Textures/UI/freeze_breath_icon";
        iconBckgrPath = "Textures/UI/AbilityIconBckgrDragon";
        abAnimID = 6;
    }

    // Carrying over Update() for counters
    public void UpdateAbility()
    {
        CooldownTimer();
        CastTimer();
        IntervalTimer();

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

                    caster.isMoveAllowed = true;
                    //Debug.Log("Projectile Fire! Projectile on cooldown...");
                }
            }

            CastAbility();
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
            stamina = -1f,
            staminaCost = -15f,

            cooldownTime = 10f,
            castTime = 2f,

            levelRequirement = 3,

            effectVisualLingerDuration = 0.2f,

            isFreezing = true,
        };
    }


    public void ExecuteAbility()
    {
        if (!isOnCooldown && 
            !isCasting && 
            casterStatForm.AimTarget != null && 
            caster.CheckIfEnoughStamina(baseStats.staminaCost))
        {
            isCasting = true;
            //Debug.Log("Casting Projectile...");
            CastTimer();

            AbilityStatForm returnForm = CalculateSelfEffect();
            caster.SelfEffect(returnForm);
        }
    }

    private void CastAbility()
    {
        if (!isOnInterval)
        {
            TakeAim();
            isOnInterval = true;

            AbilityStatForm forwardForm = CalculateAbilityStats();

            GameObject hitScanObj = MonoBehaviour.Instantiate(this.prefab, GetCastOrigin(), Quaternion.identity);

            HitScanEffect hitScanEffect = hitScanObj.GetComponent<HitScanEffect>();

            hitScanEffect.Prepare(forwardForm, this, aimDirection);
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


    private AbilityStatForm CalculateAbilityStats()
    {
        return new AbilityStatForm
        {
            Stamina = baseStats.stamina - casterStatForm.spiritPower,
            IsFreezing = baseStats.isFreezing,
        };
    }

    private AbilityStatForm CalculateSelfEffect()
    {
        return new AbilityStatForm
        {
            Stamina = baseStats.staminaCost,
        };
    }

    private bool isOnInterval = false;
    private float intervalTime = 0.2f;
    private float intervalCurrTime = 0;
    private void IntervalTimer()
    {
        if (isOnInterval)
        {
            intervalCurrTime += Time.deltaTime;
            if (intervalCurrTime >= intervalTime)
            {
                intervalCurrTime = 0;
                isOnInterval = false;
                //Debug.Log("Ready");
            }
        }
    }



}
