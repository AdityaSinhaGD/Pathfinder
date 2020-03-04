using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Vector2 gridWorldSize;
    public float nodeRadius = 0.5f;
    public Node[,] grid;

    private int gridSizeX;
    private int gridSizeY;

    public int gridMaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    private float nodeDiameter;

    public LayerMask nonWalkableLayerMask;

    public List<Node> debugPath;//todo Remove later. Only for debugging purposes currently

    private void Awake()
    {
        nodeDiameter = 2 * nodeRadius;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        grid = new Node[gridSizeX, gridSizeY];

        CreateGrid();
        GameManager.Instance.gameState = GameManager.GameState.isPlaying;
    }

    private void CreateGrid()
    {
        Vector3 gridWorldBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2 + Vector3.forward * gridWorldSize.y / 2);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPosition = gridWorldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool isWalkable = !Physics.CheckSphere(worldPosition, nodeRadius, nonWalkableLayerMask);
                grid[x, y] = new Node(isWalkable, worldPosition, x, y);
            }
        }
    }

    public Node GetNodeFromWorldPosition(Vector3 worldPosition)
    {
        float xPercent = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float yPercent = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        xPercent = Mathf.Clamp01(xPercent);
        yPercent = Mathf.Clamp01(yPercent);

        int xIndex = Mathf.RoundToInt(xPercent * (gridSizeX - 1));
        int yIndex = Mathf.RoundToInt(yPercent * (gridSizeY - 1));

        return grid[xIndex, yIndex];
    }

    public List<Node> GetNeighbouringNodes(Node node)
    {
        List<Node> neighbouringNodes = new List<Node>();
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridXIndex + x;
                int checkY = node.gridYIndex + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbouringNodes.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbouringNodes;
    }

    #region A-star
    public void SetInitialGCostForGrid()
    {
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                grid[x, y].parentNode = null;
                grid[x, y].gCost = int.MaxValue;
            }
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = node.isWalkable ? Color.cyan : Color.red;

                if (debugPath != null)
                {
                    if (debugPath.Contains(node))
                    {
                        Gizmos.color = Color.black;
                    }
                }

                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}
