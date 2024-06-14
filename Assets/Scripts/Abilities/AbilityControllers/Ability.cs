using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Ability
{
    public AbilityStatistics baseStats { get; set; }
    // casterStatForm is now a reference to the casters own stat form
    public IAbilityCaster caster { get; set; }
    public CasterStatForm casterStatForm { get; set; } // this is one is a reference to exisiting stat form in entityBehaviour
    public GameObject casterGameObject { get; set; } // new one is created at ability level
    public GameObject casterHitBoxObject { get; set; } // new one is created at ability level

    public bool isOnCooldown { get; set; } = false;
    public bool isCasting { get; set; } = false;
    public float cooldownTime { get; set; } = 0f;
    public float castTime { get; set; } = 0f;
    public string iconPath { get; set; }
    public string iconBckgrPath { get; set; }
    public bool isAvailable {  get; set; } = false;
    public int abAnimID { get; set; } = 0;
    public bool isAbilityExecuting { get; set; }
    public VisualManager.Orientation abilityOrientation { get; set; }
    public Vector2 aimDirection { get; set; }


    public void InitiateAbility<T>(CasterStatForm casterStatForm, T caster) where T : IAbilityCaster
    {
        this.casterStatForm = casterStatForm;
        this.caster = caster;
        this.casterGameObject = caster.entityGameObject;
        this.casterHitBoxObject = caster.hitBoxObject;


    }


    protected void CooldownTimer()
    {
        if (isOnCooldown)
        {
            cooldownTime += Time.deltaTime;
            if (cooldownTime >= baseStats.cooldownTime)
            {
                cooldownTime = 0;
                isOnCooldown = false;
                //Debug.Log("Ready");
            }
        }
    }

    public Vector3 GetCastOrigin()
    {
        return casterGameObject.transform.position + new Vector3(casterStatForm.AbilityCastOrigin.x, casterStatForm.AbilityCastOrigin.y, 0f);
    }



    public VisualManager.Orientation GetAbilityOrientation(Vector2 aimTarget)
    {
        //Vector2 direction = movementVector - (Vector2)casterGameObject.transform.position;

        if (Mathf.Abs(aimTarget.x) > Mathf.Abs(aimTarget.y))
        {
            return aimTarget.x > 0 ? VisualManager.Orientation.Right : VisualManager.Orientation.Left;
        }
        else
        {
            return aimTarget.y > 0 ? VisualManager.Orientation.Up : VisualManager.Orientation.Down;
        }
    }



}