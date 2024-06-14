using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStatForm
{
    private float health;
    private float stamina;

    private float? knockbackStr;
    private Vector2? knockbackDir;
    private float? knockbackDur;

    private bool isDisarming;
    private float stunDuration;
    private bool isFreezing;
    private float knockdownDuration;
    
    public AbilityStatForm() 
    {
        health = 0;
        stamina = 0;
        knockbackStr = null;
        knockbackDir = null;
        knockbackDur = null;
        isDisarming = false;
        isFreezing = false;

        stunDuration = 0;
        knockdownDuration = 0;
    }


    public float Health
    {
        get { return this.health; }
        set { this.health = value; }
    }

    public float Stamina
    {
        get { return this.stamina;}
        set { this.stamina = value;}
    }

    public float? KnockbackStr
    {
        get { return this.knockbackStr; }
        set { this.knockbackStr = value;}
    }

    public Vector2? KnockbackDir
    {
        get { return this.knockbackDir; }
        set { this.knockbackDir = value; }
    }

    public float? KnockbackDur
    {
        get { return this.knockbackDur; }
        set { this.knockbackDur = value; }
    }

    public bool IsDisarming
    {
        get { return this.isDisarming; }
        set { this.isDisarming = value; }
    }

    public float StunDuration
    {
        get { return this.stunDuration; }
        set { this.stunDuration = value; }
    }

    public bool IsFreezing
    {
        get { return this.isFreezing; }
        set { this.isFreezing = value; }
    }

    public float KnockdownDuration
    {
        get { return this.knockdownDuration;}
        set { this.knockdownDuration = value; }
    }
}
