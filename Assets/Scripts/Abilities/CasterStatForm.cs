using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterStatForm
{
    private float health;
    private float stamina;
    private Vector3 position;
    private EntityBehaviour.Faction faction;
    private Vector2? aimTarget;
    private Vector2 abilityCastOrigin;

    public float armor;
    public float physicalPower;
    public float spiritPower;

    // REMEMBER TO SET DEFAULTS!
    public CasterStatForm()
    {
        health = 0;
        stamina = 0;
    }

    public EntityBehaviour.Faction Faction 
    {
        get { return this.faction; }
        set { this.faction = value; }
    }

    public float Health
    {
        get { return this.health; }
        set { this.health = value; }
    }

    public float Stamina
    {
        get { return this.stamina; }
        set { this.stamina = value; }
    }

    public Vector3 Position
    {
        get { return this.position; }
        set { this.position = value; }
    }

    public Vector2? AimTarget
    {
        get { return this.aimTarget; }
        set { this.aimTarget = value; }
    }
    public Vector2 AbilityCastOrigin
    {
        get { return this.abilityCastOrigin; }
        set { this.abilityCastOrigin = value; }
    }
}
