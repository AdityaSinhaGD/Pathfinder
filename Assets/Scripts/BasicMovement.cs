using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 10f;

    private List<Node> path = new List<Node>();

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            path = Pathfinder.Instance.FindPathWithAStarHeap(transform.position, target.position);
            if (path != null)
            {
                StartCoroutine(TraversePath(path));
            }
        }
    }

    private IEnumerator TraversePath(List<Node> path)
    {
        foreach(Node node in path)
        {
            while (Vector3.Distance(transform.position, node.worldPosition) > 0.2f)
            {
                transform.position = Vector3.MoveTowards(transform.position, node.worldPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
