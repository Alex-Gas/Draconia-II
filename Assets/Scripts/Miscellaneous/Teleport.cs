using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject target;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("TerrainCollider"))
        {
            CharacterBehaviour script = other.gameObject.GetComponentInParent<CharacterBehaviour>();

            if (script != null && script.characterType == CharacterBehaviour.CharacterType.Player)
            {
                script.entityGameObject.transform.position = target.transform.position;
            }
        }
    }
}
