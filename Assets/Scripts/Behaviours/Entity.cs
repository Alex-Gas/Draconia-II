using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Entity : IData<EntityBehaviour>
{
    private float maxHealth;
    private float currentHealth;
    private Vector3 position;
    private List<IAbility> abilityList;
    private Vector2 abilityCastOrigin;

    public float armor;


    public float MaxHealth
    {
        get { return this.maxHealth; }
        set { this.maxHealth = value; }
    }

    public float Health
    {
        get { return this.currentHealth; }
        set { this.currentHealth = value; }
    }

    public Vector3 Position
    {
        get { return this.position; }
        set { this.position = value; }
    }

    public List<IAbility> AbilityList
    {
        get { return this.abilityList; }
        set { this.abilityList = value; }
    }

    public Vector2 AbilityCastOrigin
    {
        get { return this.abilityCastOrigin; }
        set { this.abilityCastOrigin = value; }
    }

    public void SetData(EntityBehaviour behaviour)
    {
        // only for initial values and reference types
        MaxHealth = behaviour.maxHealth;
        Health = MaxHealth;
        Position = behaviour.transform.position;
        AbilityList = behaviour.abilityList;
        AbilityCastOrigin = behaviour.abilityCastOrigin;
    }

    public void UpdateData(EntityBehaviour behaviour)
    {

    }
}
