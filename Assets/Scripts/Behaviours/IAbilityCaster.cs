using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;



public interface IAbilityCaster
{
    public List<IAbility> abilityList { get; set; }
    //public List<IAbility> selectedAbility { get; set; }
    public GameObject entityGameObject { get; set; }
    public GameObject hitBoxObject { get; set; }
    public Vector2? aimTarget { get; set; }
    public bool isAbilityInterrupt { get; set; }

    public bool isMoveAllowed { get; set; }

    public void SelfEffect(AbilityStatForm abilityStatForm);

    public void CallAbility<T>(T ability) where T : IAbility;

    public void UpdateCasterStatForm();

    public void SetupAbilities();

    public void SetAimTarget(Vector2? point);

    public void UpdateAbilities();

    public bool CheckIfEnoughStamina(float value);

    public bool CheckIfEnoughHealth(float value);

    public void ModifyXp(int xp);
}