using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap
{
    Node[] heap;
    public int noOfElementsInHeap;

    public Heap(int size)
    {
        heap = new Node[size];
    }

    private int GetLeftChildIndex(int elementIndex)
    {
        return 2 * elementIndex + 1;
    }

    private int GetRightChildIndex(int elementIndex)
    {
        return 2 * elementIndex + 2;
    }

    private int GetParentIndex(int elementIndex)
    {
        return (elementIndex - 1) / 2;
    }

    private Node GetLeftChildNode(int elementIndex)
    {
        return heap[GetLeftChildIndex(elementIndex)];
    }

    private Node GetRightChildNode(int elementIndex)
    {
        return heap[GetRightChildIndex(elementIndex)];
    }

    private Node GetParentNode(int elementIndex)
    {
        return heap[GetParentIndex(elementIndex)];
    }

    private bool HasLeftChildNode(int elementIndex)
    {
        return GetLeftChildIndex(elementIndex) < noOfElementsInHeap;
    }

    private bool HasRightChildNode(int elementIndex)
    {
        return GetRightChildIndex(elementIndex) < noOfElementsInHeap;
    }

    public void Add(Node node)
    {
        heap[noOfElementsInHeap] = node;
        noOfElementsInHeap++;
        SortUp();
    }

    public Node Remove()
    {
        Node nodeOut = heap[0];
        heap[0] = heap[noOfElementsInHeap - 1];
        noOfElementsInHeap--;
        SortDown();
        return nodeOut;
    }

    private void SortDown()
    {
        int currentIndex = 0;
        while (HasLeftChildNode(currentIndex))
        {
            int indexToSwap = GetLeftChildIndex(currentIndex);
            if (HasRightChildNode(currentIndex) && GetLeftChildNode(currentIndex).fCost > GetRightChildNode(currentIndex).fCost)
            {
                indexToSwap = GetRightChildIndex(currentIndex);
            }

            if (heap[currentIndex].fCost <= heap[indexToSwap].fCost)
            {
                break;
            }

            Swap(currentIndex, indexToSwap);
            currentIndex = indexToSwap;
        }
    }

    private void SortUp()
    {
        int currentIndex = noOfElementsInHeap - 1;

        while (currentIndex != 0)
        {
            if (heap[currentIndex].fCost < GetParentNode(currentIndex).fCost)
            {
                Swap(currentIndex, GetParentIndex(currentIndex));
                currentIndex = GetParentIndex(currentIndex);
            }
            else
            {
                break;
            }
        }
    }

    public bool Contains(Node n)
    {
        return Equals(heap[n.heapIndex], n);
    }

    private void Swap(int indexNodeA,int indexNodeB)
    {
        Node temp = heap[indexNodeA];
        heap[indexNodeA] = heap[indexNodeB];
        heap[indexNodeB] = temp;
    }
}
