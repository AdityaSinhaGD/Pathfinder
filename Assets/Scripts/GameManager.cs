using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameState { startingUp, isPlaying, isOver }
    public GameState gameState = GameState.startingUp;

    [SerializeField] private Pathfinder pathfinder;
    [SerializeField] private GameObject turretPrefab;

    private Grid grid;

    private Queue<GameObject> turretPool = new Queue<GameObject>();
    [SerializeField] private int noOfAllowedTurrets = 3;
    private int noOfTurrets;

    public override void Awake()
    {
        base.Awake();
        pathfinder = Instantiate(pathfinder, Vector3.zero, Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {
        grid = pathfinder.GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.isPlaying)
        {
            /**/
            if (Input.GetMouseButtonDown(0))
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Node node = grid.GetNodeFromWorldPosition(hit.point);

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
                            Node lastInhabitedNode = grid.GetNodeFromWorldPosition(turret.transform.position);
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
