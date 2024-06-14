using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ObjectFade : MonoBehaviour
{
    public GameObject parent;
    private BoxCollider2D detectionCollider;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        detectionCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = parent.GetComponent<SpriteRenderer>();
    }


    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("TerrainCollider"))
        {
            CharacterBehaviour script = other.gameObject.GetComponentInParent<CharacterBehaviour>();

            if (script != null && script.characterType == CharacterBehaviour.CharacterType.Player || script.characterType == CharacterBehaviour.CharacterType.NPC && other.gameObject.transform.position.y > gameObject.transform.position.y)
            {
                ApplyFade(true);
            }
            else
            {
                ApplyFade(false);
            }
        }
        else
        {
            ApplyFade(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ApplyFade(false);
    }


    private void ApplyFade(bool fade)
    {
        if (fade)
        {
            spriteRenderer.color = new UnityEngine.Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
        }
        else
        {
            spriteRenderer.color = new UnityEngine.Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        }
    }

}
