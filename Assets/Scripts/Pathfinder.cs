using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

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
        if (Input.GetMouseButtonDown(0))
        {
            FindPathWithAStarHeap(seeker.position, target.position);
        }
        if (Input.GetMouseButtonDown(1))
        {
            FindPathWithAStar(seeker.position, target.position);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            FindPathWithBFS(seeker.position, target.position);
        }

    }

    private void FindPathWithBFS(Vector3 startPos, Vector3 endPos)
    {
        Node startNode = grid.GetNodeFromWorldPosition(startPos);
        Node endNode = grid.GetNodeFromWorldPosition(endPos);

        HashSet<Node> exploredSet = new HashSet<Node>();
        Queue<Node> queue = new Queue<Node>();

        queue.Enqueue(startNode);
        exploredSet.Add(startNode);

        while (queue.Count > 0)
        {
            Node currentNode = queue.Dequeue();

            if (currentNode == endNode)
            {
                RetracePath(startNode, endNode);
                return;
            }

            List<Node> neighbouringNodes = grid.GetNeighbouringNodes(currentNode);

            foreach (Node node in neighbouringNodes)
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

    private void FindPathWithDFS(Vector3 startPos, Vector3 endPos)
    {
        Node startNode = grid.GetNodeFromWorldPosition(startPos);
        Node endNode = grid.GetNodeFromWorldPosition(endPos);

        HashSet<Node> exploredSet = new HashSet<Node>();
        Stack<Node> stack = new Stack<Node>();

        stack.Push(startNode);
        exploredSet.Add(startNode);

        while (stack.Count > 0)
        {
            Node currentNode = stack.Pop();

            if (currentNode == endNode)
            {
                RetracePath(startNode, endNode);
                return;
            }

            List<Node> neighbouringNodes = grid.GetNeighbouringNodes(currentNode);

            foreach (Node node in neighbouringNodes)
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

    private void FindPathWithAStar(Vector3 startPos, Vector3 endPos)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        Node startNode = grid.GetNodeFromWorldPosition(startPos);
        Node endNode = grid.GetNodeFromWorldPosition(endPos);

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        grid.SetInitialGCostForGrid();

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceBetweenNodes(startNode, endNode);

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = GetNodeWithMinimumFCost(openList);
            openList.Remove(currentNode);

            if (currentNode == endNode)
            {
                stopwatch.Stop();
                print("Path Found:" + stopwatch.ElapsedMilliseconds + "ms");
                RetracePath(startNode, endNode);
                return;
            }

            List<Node> neighbouringNodes = grid.GetNeighbouringNodes(currentNode);

            foreach (Node node in neighbouringNodes)
            {
                if (!node.isWalkable || closedList.Contains(node))
                {
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceBetweenNodes(currentNode, node);

                if (node.gCost > tentativeGCost)
                {
                    node.gCost = tentativeGCost;
                    node.hCost = CalculateDistanceBetweenNodes(node, endNode);
                    node.parentNode = currentNode;

                    if (!openList.Contains(node))
                    {
                        openList.Add(node);
                    }

                }
            }
            closedList.Add(currentNode);
        }
    }

    private void FindPathWithAStarHeap(Vector3 startPos,Vector3 endPos)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        Node startNode = grid.GetNodeFromWorldPosition(startPos);
        Node endNode = grid.GetNodeFromWorldPosition(endPos);

        Heap openList = new Heap(grid.gridMaxSize);
        List<Node> closedList = new List<Node>();

        grid.SetInitialGCostForGrid();

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceBetweenNodes(startNode, endNode);

        openList.Add(startNode);

        while (openList.noOfElementsInHeap > 0)
        {
            Node currentNode = openList.Remove();

            if (currentNode == endNode)
            {
                stopwatch.Stop();
                print("Path Found(Heap):" + stopwatch.ElapsedMilliseconds + "ms");
                RetracePath(startNode, endNode);
                return;
            }

            List<Node> neighbouringNodes = grid.GetNeighbouringNodes(currentNode);

            foreach(Node node in neighbouringNodes)
            {
                if (!node.isWalkable || closedList.Contains(node))
                {
                    continue;
                }

                int tentativeGCost = CalculateDistanceBetweenNodes(currentNode, node);

                if (node.gCost > tentativeGCost)
                {
                    node.gCost = tentativeGCost;
                    node.hCost = CalculateDistanceBetweenNodes(node, endNode);
                    node.parentNode = currentNode;

                    if (!openList.Contains(node))
                    {
                        openList.Add(node);
                    }
                }
            }
            closedList.Add(currentNode);
        }

    }

    private Node GetNodeWithMinimumFCost(List<Node> nodes)
    {
        Node lowestFCostNode = nodes[0];

        for (int i = 1; i < nodes.Count; i++)
        {
            if (lowestFCostNode.fCost > nodes[i].fCost)
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

    private void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        path.Reverse();
        print("path length"+path.Count);
        grid.debugPath = path;
    }

}
