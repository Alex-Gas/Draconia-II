using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanEffect : Effect, IEffect
{
    public GameObject colliderObj, colliderPivot, visual, visualPivot;

    private BoxCollider2D colliderTemplate;

    private Collider2D casterHitBox;
    private Vector2 lastCasterPosition;

    public void Awake()
    {
        colliderTemplate = colliderObj.GetComponent<BoxCollider2D>();
    }
    

    public void Start()
    {
        ability.isAbilityExecuting = true;

        casterHitBox = ability.casterHitBoxObject.GetComponent<Collider2D>();

        ReadAim();

        Run();  
    }

    public new void Update()
    {
        if (!GameMaster.isGamePaused)
        {
            base.Update();
            StickToCaster();
        }

    }

    private void StickToCaster()
    {
        if (ability.casterGameObject != null)
        {
            lastCasterPosition = ability.casterGameObject.transform.position;
        }
        this.gameObject.transform.position = lastCasterPosition;
    }

    private void ReadAim()
    {
        aimDirection = ability.aimDirection;
        float rotationAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        colliderPivot.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
        visualPivot.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
    }

    private void Run()
    {

        StartCoroutine(Visual());

        Collider2D[] colliders = Physics2D.OverlapBoxAll(colliderObj.transform.position, colliderTemplate.size, 0f);

        //Debug.Log(colliderObj.transform.position + ", " + colliderTemplate.size);

        if (colliders.Length > 0)
        {
            foreach (Collider2D collider in colliders)
            {
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
                        // if doesn't - destroy
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
