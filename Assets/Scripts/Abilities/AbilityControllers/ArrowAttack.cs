using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAttack : Ability, IAbility
{
    private GameObject prefab;

    public ArrowAttack()
    {
        UpdateBaseStats();
        prefab = Resources.Load<GameObject>("Prefabs/AbilityEffects/Arrow");
        iconPath = "Textures/UI/AbilityIcon2";
        iconBckgrPath = "Textures/UI/AbilityIconBckgrHuman";
        abAnimID = 8;
    }


    // Carrying over Update() for counters
    public void UpdateAbility()
    {
        CooldownTimer();
        CastTimer();
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
            health = -10,
            castTime = 0.8f,
            stunDuration = 0.1f,

            attackRange = 6f,

            staminaCost = -5,
            cooldownTime = 2,

        };
    }


    // this is where ability gets executed
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
        AbilityStatForm returnForm = CalculateSelfEffect();
        caster.SelfEffect(returnForm);

        // using prefab because it would be hard to fabricate a bullet here
        // IMPORTANT when spawning the abiliy effect the ability is asking the caster for its position. Consider doing vice versa
        GameObject projObj = MonoBehaviour.Instantiate(this.prefab, GetCastOrigin(), Quaternion.identity);

        ProjectileEffect projectileEffect = projObj.GetComponent<ProjectileEffect>();

        projectileEffect.Prepare(forwardForm, this, aimDirection);

    }


    // If this is called at any other time than when the ability gets cast then add checks for null
    private AbilityStatForm CalculateAbilityStats()
    {
        return new AbilityStatForm
        {
            Health = baseStats.health - casterStatForm.physicalPower,
        };
    }

    // If this is called at any other time than when the ability gets cast then add checks for null
    private AbilityStatForm CalculateSelfEffect()
    {
        // make list of stat forms if there are multiple different effects happening at different times 

        return new AbilityStatForm
        {
            Stamina = baseStats.staminaCost,
        };
    }
}
