using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnStateChange;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.PlayerMove);
    }
    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.PlayerMove:
                HandlePlayerMove();  
                break;
            case GameState.PlayerAttack:
                break;
            case GameState.EnemyMove:
                break;
            case GameState.EnemyAttack:
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
        }

        OnStateChange?.Invoke(newState);
    }

    private void HandlePlayerMove()
    {
        throw new NotImplementedException();
    }

    public enum GameState
{
    PlayerMove,
    PlayerAttack,
    EnemyMove,
    EnemyAttack,
    Victory,
    Lose
}
}