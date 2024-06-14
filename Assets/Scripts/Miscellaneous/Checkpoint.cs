using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Checkpoint : MonoBehaviour
{
    public GameObject spawnLocation;
    public GameObject model;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("TerrainCollider"))
        {
            PlayerBehaviour script = other.gameObject.GetComponentInParent<PlayerBehaviour>();

            if (script != null && script.characterType == CharacterBehaviour.CharacterType.Player)
            {
                script.SetCheckpointLocation(spawnLocation);
                model.GetComponent<SpriteRenderer>().enabled = true;
            }
        }


    }
}
