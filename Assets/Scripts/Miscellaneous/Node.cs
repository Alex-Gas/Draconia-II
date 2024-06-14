using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool blocked;
    public Vector2 location;
    public List<Node> neighbours;

    public Node(bool blocked, Vector2 location, List<Node> neighbours)
    {
        this.blocked = blocked;
        this.location = location;
        this.neighbours = neighbours;
    }
}
