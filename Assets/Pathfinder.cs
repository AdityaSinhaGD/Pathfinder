using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{

    public Transform seeker, target;

    private Grid grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        //FindPathWithBFS(seeker.position, target.position);
        FindPathWithAStar(seeker.position, target.position);
    }

    private void FindPathWithBFS(Vector3 startPos,Vector3 endPos)
    {
        Node startNode = grid.GetNodeFromWorldPosition(startPos);
        Node endNode = grid.GetNodeFromWorldPosition(endPos);

        List<Node> exploredSet = new List<Node>();
        Queue<Node> queue = new Queue<Node>();

        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            Node currentNode = queue.Dequeue();

            if(currentNode == endNode)
            {
                RetracePath(startNode, endNode);
                return;
            }

            List<Node> neighbouringNodes = grid.GetNeighbouringNodes(currentNode);

            foreach(Node node in neighbouringNodes)
            {
                if (node.isWalkable && !exploredSet.Contains(node))
                {
                    exploredSet.Add(node);
                    node.parentNode = currentNode;
                    queue.Enqueue(node);
                }
            }
        }
    }

    private void FindPathWithDFS(Vector3 startPos,Vector3 endPos)
    {
        Node startNode = grid.GetNodeFromWorldPosition(startPos);
        Node endNode = grid.GetNodeFromWorldPosition(endPos);

        List<Node> exploredSet = new List<Node>();
        Stack<Node> stack = new Stack<Node>();

        stack.Push(startNode);

        while (stack.Count > 0)
        {
            Node currentNode = stack.Pop();

            if (currentNode == endNode)
            {
                RetracePath(startNode, endNode);
                return;
            }

            List<Node> neighbouringNodes = grid.GetNeighbouringNodes(currentNode);

            foreach(Node node in neighbouringNodes)
            {
                if (node.isWalkable && !exploredSet.Contains(node))
                {
                    exploredSet.Add(node);
                    node.parentNode = currentNode;
                    stack.Push(node);
                }
            }
        }
    }

    private void FindPathWithAStar(Vector3 startPos,Vector3 endPos)
    {
        Node startNode = grid.GetNodeFromWorldPosition(startPos);
        Node endNode = grid.GetNodeFromWorldPosition(endPos);

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        openList.Add(startNode);

        grid.SetInitialGCostForGrid();

        startNode.gCost = 0;

        while (openList.Count > 0)
        {
            Node currentNode = GetNodeWithMinimumFCost(openList);

            if(currentNode == endNode)
            {
                RetracePath(startNode, endNode);
                return;
            }

            openList.Remove(currentNode);

            List<Node> neighbouringNodes = grid.GetNeighbouringNodes(currentNode);

            foreach(Node node in neighbouringNodes)
            {
                if (!node.isWalkable || closedList.Contains(node))
                {
                    continue;
                }

                closedList.Add(currentNode);

                int tentativeGCost = currentNode.gCost + CalculateDistanceBetweenNodes(currentNode, node);

                if (node.gCost > tentativeGCost)
                {
                    node.gCost = tentativeGCost;
                    node.parentNode = currentNode;
                    openList.Add(node);
                }
            }
        }
    }

    private Node GetNodeWithMinimumFCost(List<Node> nodes)
    {
        Node lowestFCostNode = nodes[0];

        for(int i = 1; i < nodes.Count; i++)
        {
            if(lowestFCostNode.fCost > nodes[i].fCost)
            {
                lowestFCostNode = nodes[i];
            }
        }

        return lowestFCostNode;
    }

    private int CalculateDistanceBetweenNodes(Node nodeA, Node nodeB)
    {
        int xDiff = Mathf.Abs(nodeA.gridXIndex - nodeB.gridXIndex);
        int yDiff = Mathf.Abs(nodeA.gridYIndex - nodeB.gridYIndex);

        int remaining = Mathf.Abs(xDiff - yDiff);

        return Mathf.Min(xDiff, yDiff) * 14 + remaining * 10;
    }

    private void RetracePath(Node startNode,Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        path.Reverse();
        grid.debugPath = path;
    }

}
