using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerLibrary
{
    private static Dictionary<int, List<int>> dictionaryOfContainers;

    public static void Create()
    {
        CreateBodies();
    }

    public static List<int> GetContainerContents(int ID)
    {
        return dictionaryOfContainers[ID];
    }

    private static void CreateBodies()
    {
        dictionaryOfContainers = new()
        {
            { 0, new List<int> { }},
            { 1, 
                new List<int>(){ 9 } 
            },
            { 2,
                new List<int>(){ 12, 3, 3 }
            },
            { 3,
                new List<int>(){ 15 }
            },
            { 4,
                new List<int>(){ 16 }
            },
            { 5,
                new List<int>(){ 13, 3 }
            },
            { 6,
                new List<int>(){ 11, 3 }
            },
        };
    }
}
