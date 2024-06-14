using System;
using UnityEngine;

public interface IEffect
{
    public IAbility ability { get; set; }
    public AbilityStatForm abilityStatForm { get; set; }

    public CasterStatForm casterStatForm { get; set; }

    public void Prepare(AbilityStatForm abilityStatForm, IAbility ability, Vector2 aimDirection);
}
