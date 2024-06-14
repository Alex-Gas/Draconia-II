using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EntityRemainsLibrary
{
    private static Dictionary<int, string> dictionaryOfBodies;

    public static void Create()
    {
        CreateBodies();   
    }

    public static string GetBodyPath(int ID)
    {
        return dictionaryOfBodies[ID];
    }

    private static void CreateBodies()
    {
        dictionaryOfBodies = new()
        {
            { 1, "KnightBodyDead" },
            { 2, "KnightBodyKnockOut" },
            { 3, "KnightBodyFrozen" },
            { 4, "ArcherBodyDead" },
            { 5, "ArcherBodyKnockOut" },
            { 6, "ArcherBodyFrozen" },
        };
    }
}
