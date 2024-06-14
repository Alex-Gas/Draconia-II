using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class Animate : IData<AnimateBehaviour>
{
    private float speedMultiplier;

    public float SpeedMultiplier
    {
        get { return this.speedMultiplier; }
        set { this.speedMultiplier = value; }
    }

    public void SetData(AnimateBehaviour behaviour)
    {
        SpeedMultiplier = behaviour.speedMultiplier;
    }

    public void UpdateData(AnimateBehaviour behaviour)
    {

    }

}
