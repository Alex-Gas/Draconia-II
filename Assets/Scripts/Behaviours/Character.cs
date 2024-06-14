using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Character : IData<CharacterBehaviour>
{
    private float maxStamina;

    private float currentStamina;

    public float physicalPower;
    public float spiritPower;


    public float MaxStamina
    {
        get { return this.maxStamina; }
        set { this.maxStamina = value; }
    }

    public float Stamina
    {
        get { return this.currentStamina; }
        set { this.currentStamina = value; }
    }

    public void SetData(CharacterBehaviour behaviour)
    {
        MaxStamina = behaviour.maxStamina;
        Stamina = MaxStamina;
    }

    public void UpdateData(CharacterBehaviour behaviour)
    {

    }
}
