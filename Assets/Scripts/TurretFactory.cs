using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFactory : Singleton<TurretFactory>
{

    [SerializeField] private GameObject turretPrefab;

    private Queue<GameObject> turretPool = new Queue<GameObject>();
    [SerializeField] private int noOfAllowedTurrets = 3;
    private int noOfTurrets;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.isPlaying)
        {
            if (Input.GetMouseButtonDown(0))
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Node node = Grid.Instance.GetNodeFromWorldPosition(hit.point);

                    if (node != null && node.isWalkable)
                    {
                        if (noOfTurrets < noOfAllowedTurrets)
                        {
                            GameObject turret = Instantiate(turretPrefab, node.worldPosition, Quaternion.identity);
                            noOfTurrets++;
                            node.isWalkable = false;
                            turretPool.Enqueue(turret);
                        }
                        else
                        {
                            GameObject turret = turretPool.Dequeue();
                            Node lastInhabitedNode = Grid.Instance.GetNodeFromWorldPosition(turret.transform.position);
                            lastInhabitedNode.isWalkable = true;
                            turret.transform.position = node.worldPosition;
                            node.isWalkable = false;
                            turretPool.Enqueue(turret);
                        }
                    }
                }
            }
        }
    }
}
