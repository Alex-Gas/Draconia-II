using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StaticEnvironment : MonoBehaviour
{
    public GameObject blockedGridObj;
    [HideInInspector] public static GameObject blockedTileObject;
    [HideInInspector] public static Tilemap blockedTilemap;

    public void Awake()
    {
        blockedTilemap = blockedGridObj.GetComponent<Tilemap>();
        blockedTileObject = blockedGridObj;
        GameMaster.CreatePathfindingNodeMap();
    }
}
