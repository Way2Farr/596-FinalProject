using System;
using System.Linq;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnStateChange;

//--------------
    public TurnManager TurnManager {get; private set;}

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.ChooseOption);
//------------
        TurnManager = new TurnManager();

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
                UnitManager.Instance.ShowMovementOverlay();
                //UnitManager.Instance.ShowEnemyMovementOverlay();
                //HandlePlayerMove();  
                break;
            case GameState.PlayerAttack:
                //UnitManager.Instance.ShowEnemyAttackOverlay();
                UnitManager.Instance.ShowAttackOverlay();
                break;
            case GameState.EnemyChoose:
                UnitManager.Instance.EnemyChoose();
                break;
            case GameState.EnemyMove:
                UnitManager.Instance.ShowEnemyMovementOverlay();
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
                SceneManager.LoadScene("Shop (Nick)");
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
    EnemyChoose,
    EnemyMove,
    EnemyAttack,
    Victory,
    Lose,
    Flee
}
}