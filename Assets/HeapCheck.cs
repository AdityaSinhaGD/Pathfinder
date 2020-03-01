using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeapCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BuildHeap(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BuildHeap(int n)
    {
        Heap heap = new Heap(n);
        for(int i = 0; i < n; i++)
        {
            Node node = new Node(true,Vector3.zero,0,0);
            node.gCost = Random.Range(1, 50);
            heap.Add(node);
            Debug.Log(node.gCost);
        }

        Debug.Log("-------------------------");

        while (heap.noOfElementsInHeap > 0)
        {
            Node node = heap.Remove();
            Debug.Log(node.fCost);
        }
    }
}
