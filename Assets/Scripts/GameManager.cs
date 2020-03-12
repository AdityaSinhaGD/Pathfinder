using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameState { startingUp, isPlaying, isOver }
    public GameState gameState = GameState.startingUp;

    [SerializeField] private Pathfinder pathfinder;

    public override void Awake()
    {
        base.Awake();
        pathfinder = Instantiate(pathfinder, Vector3.zero, Quaternion.identity);
    }

}
