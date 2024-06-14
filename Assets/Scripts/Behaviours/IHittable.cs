using System;
using UnityEngine;


public interface IHittable
{
    public GameObject entityGameObject { get; set; }

    public void Hit(AbilityStatForm abilityStatForm, IAbilityCaster caster);

    public bool IsEnemyFaction(EntityBehaviour.Faction faction);

    public void SendXp(IAbilityCaster caster);
}
