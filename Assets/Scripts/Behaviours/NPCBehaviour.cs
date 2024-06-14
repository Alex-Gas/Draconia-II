using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityBehaviour;
using static PlayerBehaviour;

public class NPCBehaviour : CharacterBehaviour, IHittable, IAbilityCaster, IInteractable, ITalkable
{
    protected NPC npc = new();



    protected new void Start()
    {
        base.Start();
        npc.SetData(this);
        UpdateHealthBar();
    }
    protected new void Update()
    {
        base.Update();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
    }


    public void SetupAbilities()
    {

    }

    // a bit manual but will do for now
    public bool IsEnemyFaction(EntityBehaviour.Faction casterFaction)
    {
        if (casterFaction == Faction.Player)
        {
            return false;
        }
        else if (casterFaction == Faction.Enemy)
        {
            return false;
        }
        else
        {
            return false;
        }
    }

    public void CallAbility<T>(T ability) where T : IAbility
    {
        ability.ExecuteAbility();
    }

    public new void Hit(AbilityStatForm abilityStatForm, IAbilityCaster caster)
    {
        base.Hit(abilityStatForm, caster);
    }


    // this function applies effects inflicted on itself 
    public new void SelfEffect(AbilityStatForm abilityStatForm)
    {
        base.SelfEffect(abilityStatForm);
    }

    // this must be called before any target usage
    public void SetAimTarget(Vector2? point)
    {
        aimTarget = point;
    }

    public void UpdateAbilities()
    {
        UpdateCasterStatForm();

        foreach (var ability in abilityList)
        {
            ability.UpdateAbility();
        }
    }


    public void OnInteract(PlayerBehaviour entity)
    {
        //Debug.Log("called for interaction with NPC");
        if (conversation != null)
        {
            //Debug.Log("conversation id: " + conversation.ID + ", nodes count: " + conversation.dialogNodes.Count);
            entity.RequestConversation(conversation, this);
        }
    }

    public void ModifyXp(int xp)
    {
        // npcs dont receive xp
    }

}
