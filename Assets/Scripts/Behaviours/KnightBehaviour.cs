using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBehaviour : EnemyBehaviour, IHittable, IAbilityCaster
{
    protected Knight knight = new();

    private EnemyAIAgent enemyAIAgent;


    protected new void Start()
    {
        base.Start();
        knight.SetData(this);
        SetupAbilities();
        SelectAbility();

        enemyAIAgent = new EnemyAIAgent(this);

    }
    protected new void Update()
    {
        if (!GameMaster.isGamePaused)
        {
            enemyAIAgent.AgentAIUpdate();

            base.Update();

            visualManager.ApplyOrientationVisual(aimTarget, newMovementVector);
            visualManager.ApplyAbilityVisual();
        }

    }

    protected new void FixedUpdate()
    {
        if (!GameMaster.isGamePaused)
        {
            base.FixedUpdate();
        }
        if (GameMaster.isGamePaused)
        {
            animateRigidbody.velocity = Vector2.zero;
        }
    }

    public void SetupAbilities()
    {
        ChargeAttack chargeAttack = new();
        this.abilityList = new List<IAbility> { chargeAttack };
        foreach (var ability in abilityList)
        {
            ability.InitiateAbility(casterStatForm, this);
        }
    }

    private void SelectAbility()
    {
        selectedAbility = abilityList[0];
    }

    public new void Hit(AbilityStatForm abilityStatForm, IAbilityCaster caster)
    {
        base.Hit(abilityStatForm, caster);

        if (isHealthZero)
        {
            //Debug.Log("ping");
            SendXp(caster);
            DropItems();
            SpawnEntityBody(EntityRemainsLibrary.GetBodyPath(deadBodyTextureID));
            Destroy(gameObject);
        }

        else if (isKnockedOut)
        {
            SendXp(caster);
            DropItems();
            SpawnEntityBody(EntityRemainsLibrary.GetBodyPath(knockedOutBodyTextureID));
            Destroy(gameObject);
        }

        else if (isFrozen)
        {
            SendXp(caster);
            DropItems();
            SpawnEntityBody(EntityRemainsLibrary.GetBodyPath(frozenBodyTextureID));
            Destroy(gameObject);
        }
    }


    // this function applies effects inflicted on itself 
    public new void SelfEffect(AbilityStatForm abilityStatForm)
    {
        base.SelfEffect(abilityStatForm);
    }

    public void ModifyXp(int xp)
    {
        // enemies dont receive xp
    }
}
