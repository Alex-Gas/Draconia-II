using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashEffect : Effect, IEffect
{
    private Collider2D casterHitBox;

    public void Awake()
    {

    }


    public void Start()
    {
        ability.isAbilityExecuting = true;
        casterHitBox = ability.casterHitBoxObject.GetComponent<Collider2D>();
        Run();
    }


    public new void Update()
    {
        if (!GameMaster.isGamePaused)
        {
            base.Update();
        }
    }


    public void FixedUpdate()
    {
        if (!GameMaster.isGamePaused)
        {
        }
    }


    private void Run()
    {
        StartCoroutine(Visual());


        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, baseStats.attackRange);

        if (colliders.Length > 0)
        {
            foreach (Collider2D collider in colliders)
            {
                // this if only for when knockback is to be applied when hit
                abilityStatForm.KnockbackDir = ((Vector2)collider.gameObject.transform.position - (Vector2)this.gameObject.transform.position).normalized; ;

                IHittable hittableScript = collider.gameObject.GetComponentInParent<IHittable>();
                // 1. check if target has a hitbox collider (separates background environment)
                if (collider.gameObject.layer == LayerMask.NameToLayer("HitBox") || collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                {
                    // 2. check if target hitbox and caster hitbox are the same (separates caster)
                    if (casterHitBox == null || collider != casterHitBox)
                    {
                        // 3. check if target has hitable script (separates walls)
                        if (hittableScript != null)
                        {
                            // 4. call return CheckFaction() to check if target belongs to a faction considered hostile or neutral (separates friendlies)
                            if (hittableScript.IsEnemyFaction(casterStatForm.Faction))
                            {
                                // 5. call Hit() to apply effect to a target hit
                                hittableScript.Hit(abilityStatForm, caster);
                            }
                            // if friendly faction - ignore
                        }
                        // if doesn't - ignore
                    }
                    // if the same - ignore
                }
                // if no hitbox - ignore
            }
        }
    }

    IEnumerator Visual()
    {
        yield return new WaitForSeconds(baseStats.effectVisualLingerDuration);
        ability.isAbilityExecuting = false;
        Destroy(gameObject);
    }




}
