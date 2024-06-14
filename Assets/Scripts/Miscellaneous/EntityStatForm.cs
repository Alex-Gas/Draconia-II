using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatForm
{
    private float health;
    private float stamina;
    private float physicalPower;
    private float magicalPower;

    public EntityStatForm()
    {
        health = 0;
        stamina = 0;
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
}
