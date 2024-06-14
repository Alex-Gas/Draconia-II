using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileEffect : Effect, IEffect
{
    public GameObject colliderObj, colliderPivot, visual, visualPivot;

    private Rigidbody2D body;

    private Collider2D casterHitBox;

    private float projectileSpeed = 10f;
    private float projectileLifetime = 100f;

    public void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }


    public void Start()
    {
        casterHitBox = ability.casterHitBoxObject.GetComponent<Collider2D>();

        ReadAim();
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
            Move();
        }
        if (GameMaster.isGamePaused)
        {
            body.velocity = Vector2.zero;
        }
    }

    private void ReadAim()
    {
        float rotationAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        colliderPivot.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
        visualPivot.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
    }


    private void Move()
    {
        if (projectileLifetime > 0)
        {
            projectileLifetime--;
            body.velocity = aimDirection * projectileSpeed;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        IHittable hittableScript = collider.gameObject.GetComponentInParent<IHittable>();

        // MAKE CONDITIONS INTO FUNCTIONS FOR BETTER READABILITY
        // MAKE PARENT CLASS ABILITYEFFECT

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
                        Destroy(gameObject);
                    }
                    // if friendly faction - ignore
                }
                // if doesn't - destroy
                else
                {
                    Destroy(gameObject);
                }
            }
            // if the same - ignore
        }
        // if no hitbox - ignore
    }
}

