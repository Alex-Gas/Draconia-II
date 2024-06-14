using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public IAbility ability { get; set; }
    public IAbilityCaster caster { get; set; }
    public AbilityStatForm abilityStatForm { get; set; }
    public AbilityStatistics baseStats { get; set; }
    public CasterStatForm casterStatForm { get; set; }

    public Vector2 aimDirection;
    // Effect is to have its own independent casterStatForm

    public void Prepare(AbilityStatForm abilityStatForm, IAbility ability, Vector2 aimDirection)
    {
        this.abilityStatForm = abilityStatForm;
        this.ability = ability; 
        caster = ability.caster;
        baseStats = ability.baseStats;
        casterStatForm = ability.casterStatForm;
        this.aimDirection = aimDirection;
    }

    public void Update()
    {

    }
}
