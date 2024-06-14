using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{
    public AbilityStatistics baseStats { get; set; }
    // casterStatForm is now a reference to the casters own stat form
    public IAbilityCaster caster { get; set; }
    public CasterStatForm casterStatForm { get; set; }
    public GameObject casterGameObject { get; set; }
    public GameObject casterHitBoxObject { get; set; }

    public bool isOnCooldown { get; set; }
    public bool isCasting { get; set; }
    public float cooldownTime { get; set; }
    public float castTime { get; set; }
    public string iconPath { get; set; }
    public string iconBckgrPath { get; set; }
    public bool isAvailable { get; set; }
    public bool isAbilityExecuting { get; set; }
    public int abAnimID {  get; set; }

    public Vector2 aimDirection { get; set; }


    public VisualManager.Orientation abilityOrientation { get; set; }
    public void InitiateAbility<T>(CasterStatForm casterStatForm, T caster) where T : IAbilityCaster;

    public void ExecuteAbility();

    public void UpdateAbility();

    public VisualManager.Orientation GetAbilityOrientation(Vector2 movementVector);
}
