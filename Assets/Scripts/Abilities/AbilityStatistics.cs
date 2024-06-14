using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStatistics
{
    public float health = 0;
    public float stamina = 0;


    public float healthCost = 0;
    public float staminaCost = 0;

    public float cooldownTime = 0;
    public float castTime = 0;

    public float dpsInterval;

    public float attackRange;

    public float stunDuration;
    public float knockdownDuration;

    public float levelRequirement;

    public bool isDisarming = false;
    public bool isFreezing = false;


    public float effectVisualLingerDuration;


    public float? knockbackStr = null;
    public Vector2? knockbackDir = null;
    public float? knockbackDur = null;
    
    public float? selfKnockbackStr = null;
    public Vector2? selfKnockbackDir = null;
    public float? selfKnockbackDur = null;
    

}

