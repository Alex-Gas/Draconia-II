using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameMaster : MonoBehaviour
{
    private UserInput userInput;

    public static float horizontalInput;
    public static float verticalInput;
    public static bool mouseLeftDown;
    public static bool mouseRightDown;
    public static Vector2 mousePosition;
    public static Vector2 mouseScreenToWorld;
    public static bool ability1Input;
    public static bool ability2Input;
    public static bool ability3Input;
    public static bool inventoryInput;
    public static bool formSwitchInput;
    public static bool interactInput;

    public static List<Node> pathfindingNodes;

    public static bool isGamePaused = false;
    public static bool isPlayerBusy = false;

    public static bool talkedToKharim = false;


    private static MenuManager menuManager;

    public static bool isMenuOpen = false;
    public static bool isInventoryOpen = false;
    public static bool isDialogOpen = false;


    private void Awake()
    {
        ItemLibrary.Create();
        DialogLibrary.Create();
        EntityRemainsLibrary.Create();
        ContainerLibrary.Create();
    }

    private void Start()
    {
        userInput = new UserInput();
        menuManager = new MenuManager();

        StartScreen();
    }

    private void Update()
    {
        UserInput();

        CheckUIStatus();

    }

    private void CheckUIStatus()
    {
        isGamePaused = isMenuOpen ? true : false;

        isPlayerBusy = isInventoryOpen || isDialogOpen ? true : false;
    }



    private void UserInput()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseScreen();
        }

        horizontalInput = userInput.GetHorizontalVal(); 
        verticalInput = userInput.GetVerticalVal();
        mouseLeftDown = userInput.GetMouseLeftDown();
        mouseRightDown = userInput.GetMouseRightDown();
        mousePosition = userInput.GetMousePosition();
        mouseScreenToWorld = GetMouseWorldPosition(mousePosition);
        ability1Input = userInput.GetAbility1Input();
        ability2Input = userInput.GetAbility2Input();
        ability3Input = userInput.GetAbility3Input();
        inventoryInput = userInput.GetInventoryInput();
        formSwitchInput = userInput.GetFormSwitchInput();
        interactInput = userInput.GetInteractInput();
    }
    public Vector2 GetMouseWorldPosition(Vector2 mousePos)
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(new Vector2(mousePos.x, mousePos.y));
        return worldPoint;
    }


    public static void CreatePathfindingNodeMap()
    {
        Tilemap map = StaticEnvironment.blockedTilemap;
        map.CompressBounds();
        pathfindingNodes = GetNodes(map);
    }


    private static List<Node> GetNodes(Tilemap map)
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

    private static void AssignNeighbour(Node node, List<Node> nodes, Vector2 offset)
    {
        Vector2 nOffset = node.location + offset;
        Node neighbour = nodes.FirstOrDefault(obj => obj.location == nOffset) ?? null;
        if (neighbour != null) node.neighbours.Add(neighbour);
    }



    public static void StartScreen()
    {
        menuManager.ToggleMenu(MenuManager.MenuMode.Start);
    }

    public static void TogglePauseScreen()
    {
        menuManager.ToggleMenu(MenuManager.MenuMode.Pause);
    }

    public static void EndScreen()
    {
        menuManager.ToggleMenu(MenuManager.MenuMode.End);
    }

    public static void EndGame()
    {
        EndScreen();
    }

}
