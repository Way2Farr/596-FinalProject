using System;
using Unity.VisualScripting;
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
        UpdateGameState(GameState.ChooseOption);
    }
    public void UpdateGameState(GameState newState)
    {
        State = newState;
        Debug.Log(newState.ToString());

        // Call function for a given state
        switch (newState)
        {
            case GameState.SpawnUnits:
                SpawnPlayerUnits();
                break;
            case GameState.ChooseOption:
                break;
            case GameState.PlayerMove:
                //HandlePlayerMove();  
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
            case GameState.Flee:
                break;
        }

        OnStateChange?.Invoke(newState);
    }

    public void ButtonSetState(int setState)
    {
        switch (setState)
        {
            case 0:
                UpdateGameState(GameState.PlayerAttack);
                break;
            case 1:
                UpdateGameState(GameState.PlayerMove);
                break;
            case 2:
                UpdateGameState(GameState.Flee);
                break;
            default:
                break;

        }
    }

    public void MoveLogic()
    {

    }

    private void SpawnPlayerUnits()
    {
        
        GameManager.Instance.UpdateGameState(GameState.ChooseOption);
    }
    private void HandlePlayerMove()
    {
        throw new NotImplementedException();
    }

    public enum GameState
{
    SpawnUnits,
    ChooseOption,
    PlayerMove,
    PlayerAttack,
    EnemyMove,
    EnemyAttack,
    Victory,
    Lose,
    Flee
}
}