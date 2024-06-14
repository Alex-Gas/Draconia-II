using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject archer, knight;

    private List<GameObject> list;

    private bool isCharged = true;
    private bool isSpawning = false;


    private void Awake()
    {
        list = new() { archer, knight };
    }


    private void Update()
    {
        if (!GameMaster.isGamePaused)
        {
            SpawnTimer();
            SpawnInterval();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCharged && other.gameObject.layer == LayerMask.NameToLayer("TerrainCollider"))
        {
            PlayerBehaviour script = other.gameObject.GetComponentInParent<PlayerBehaviour>();

            if (script != null && script.characterType == CharacterBehaviour.CharacterType.Player)
            {
                isCharged = false;
                isSpawning = true;
                Debug.Log("spawner triggered");
            }
        }
    }

    private void SpawnEntity()
    {
        int rand = Random.Range(0, 2);

        Instantiate(list[rand], new Vector3(transform.position.x, transform.position.y, -5), Quaternion.identity);

        Debug.Log("entity spawned!");
    }



    private float TotalTime = 16;
    private float CurrTotalTime = 0;
    private void SpawnTimer()
    {
        if (isSpawning)
        {
            CurrTotalTime += Time.deltaTime;
            if (CurrTotalTime >= TotalTime)
            {
                CurrTotalTime = 0;
                isSpawning = false;
            }
        }
    }

    private float IntervalTime = 4;
    private float CurrIntervalTime = 0;

    private void SpawnInterval()
    {
        if (isSpawning)
        {
            CurrIntervalTime += Time.deltaTime;
            if (CurrIntervalTime >= IntervalTime)
            {
                CurrIntervalTime = 0;
                SpawnEntity();
                
            }
        }
    }



}
