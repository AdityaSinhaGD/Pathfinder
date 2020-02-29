using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool isWalkable;
    public Vector3 worldPosition;
    public int gridXIndex;
    public int gridYIndex;

    public Node parentNode;

    public Node(bool isWalkable, Vector3 worldPosition, int gridXIndex, int gridYIndex)
    {
        this.isWalkable = isWalkable;
        this.worldPosition = worldPosition;
        this.gridXIndex = gridXIndex;
        this.gridYIndex = gridYIndex;
    }

    #region A-Star
    public int gCost;
    public int hCost;
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    #endregion
}
