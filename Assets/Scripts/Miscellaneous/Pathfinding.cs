using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Pathfinding
{
    protected EnemyAIAgent enemyAIAgent;

    // list of nodes which are impassable
    private List<Node> nodes;


    public Pathfinding(EnemyAIAgent enemyAIAgent)
    {
        this.enemyAIAgent = enemyAIAgent;

        nodes = GameMaster.pathfindingNodes;

        /*

        Tilemap map = StaticEnvironment.blockedTilemap;
        //constraining tilemap bounds to two furthest corners
        map.CompressBounds();

        nodes = GetNodes(map);
        */
    }

    /*
    private List<Node> GetNodes(Tilemap map)
    {
        // calculating the offset (tiles in tilemap start in bottom left cornner)
        float cellOffset = map.cellSize.x / 2f;
        // get a list of all blocked coordinates in the grid
        List<Vector2> blockedGrid = new();
        foreach (Vector2Int boundPosition in map.cellBounds.allPositionsWithin)
        {
            Vector2 location = new Vector2(boundPosition.x + cellOffset, boundPosition.y + cellOffset);
            if (map.HasTile((Vector3Int)boundPosition)) blockedGrid.Add(location);
        }

        // future node list
        List<Node> nodesNew = new();

        // create all nodes, with no neighbours for now
        foreach (Vector2Int boundPosition in map.cellBounds.allPositionsWithin)
        {
            // create cell at current location
            Vector2 location = new Vector2(boundPosition.x + cellOffset, boundPosition.y + cellOffset);
            bool isBlocked = blockedGrid.Contains(location);
            nodesNew.Add(new Node(isBlocked, location, new()));
        }

        // assign neighbours (bit complex, might change in future)
        foreach (Node node in nodesNew)
        {
            AssignNeighbour(node, nodesNew, new Vector2(1f, 0f));
            AssignNeighbour(node, nodesNew, new Vector2(1f, 1f));
            AssignNeighbour(node, nodesNew, new Vector2(0f, 1f));
            AssignNeighbour(node, nodesNew, new Vector2(-1f, 1f));
            AssignNeighbour(node, nodesNew, new Vector2(-1f, 0f));
            AssignNeighbour(node, nodesNew, new Vector2(-1f, -1f));
            AssignNeighbour(node, nodesNew, new Vector2(0f, -1f));
            AssignNeighbour(node, nodesNew, new Vector2(1f, -1f));
        }

        return nodesNew;
    }

    // this tests to see if a node in list is a neighbour to current node
    private static void AssignNeighbour(Node node, List<Node> nodes, Vector2 offset)
    {
        Vector2 nOffset = node.location + offset;
        Node neighbour = nodes.FirstOrDefault(obj => obj.location == nOffset) ?? null;
        if (neighbour != null) node.neighbours.Add(neighbour);
    }
    */
    private List<Node> Astar(Node start, Node goal)
    {
        // list of nodes in the perimeter
        List<Node> toExplore = new() { start };

        // the cost of the cheapest path from the start to Node
        Dictionary<Node, float> gScore = new();

        // gScore + heuristic evaluation of the node
        Dictionary<Node, float> fScore = new();

        gScore[start] = 0f;
        fScore[start] = Heuristic(start, goal);

        // maps node to node preceeding it on shortest path currently known
        Dictionary<Node, Node> cameFrom = new();

        // this is to check which nodes can be skipped to get the shortest path
        Dictionary<Node, bool> hasBlockedNeighbours = new();

        // A*
        while (toExplore.Count > 0)
        {
            Node current = toExplore.OrderBy(obj => fScore.GetValueOrDefault(obj, float.MaxValue)).First();
            if (current == goal) return ReconstructPath(cameFrom, hasBlockedNeighbours, current);

            hasBlockedNeighbours[current] = false;
            toExplore.Remove(current);
            foreach (Node neighbour in current.neighbours)
            {
                float neighbourScore = neighbour.blocked ? float.MaxValue : gScore.GetValueOrDefault(current, float.MaxValue) + 1f;
                if (neighbour.blocked) hasBlockedNeighbours[current] = true;
                if (neighbourScore < gScore.GetValueOrDefault(neighbour, float.MaxValue))
                {
                    cameFrom[neighbour] = current;
                    gScore[neighbour] = neighbourScore;
                    fScore[neighbour] = neighbourScore + Heuristic(neighbour, goal);
                    if (!toExplore.Contains(neighbour)) toExplore.Add(neighbour);
                }
            }
        }
        return new();
    }

    private static float Heuristic(Node start, Node goal)
    {
        return (goal.location - start.location).magnitude;
    }

    // tracks the path back to start point from finish point
    private static List<Node> ReconstructPath(Dictionary<Node, Node> cameFrom, Dictionary<Node, bool> hasBlockedNeighbours, Node current)
    {
        List<Node> path = new() { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            if (hasBlockedNeighbours[current]) path.Add(current);
        }
        path.Reverse();
        return path;
    }

    private Node GetClosestUnblockedNode(Vector2 position)
    {
        return nodes
            .Where(node => !node.blocked)
            .OrderBy(node => (position - node.location).magnitude).First();
    }


    public Queue<Vector2> CreatePath(Vector2 goal)
    {
        Rigidbody2D body = enemyAIAgent.enemyBehaviour.animateRigidbody;
        Node start = GetClosestUnblockedNode(body.transform.position);
        Node target = GetClosestUnblockedNode(goal);

        List<Node> nodes = Astar(start, target);
        Queue<Vector2> path = new();
        // skip last (swap for final location) and first (first point is to align to the grid)
        for (int i = 1; i < nodes.Count - 1; i++) path.Enqueue(nodes[i].location);
        path.Enqueue(goal);

        return path;
    }

}
