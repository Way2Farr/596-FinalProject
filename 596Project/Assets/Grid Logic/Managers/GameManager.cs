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
            if (UnitManager.Instance.hasMoved) {
                MenuManager.Instance.EventMessages("You already moved!");
                UnitManager.Instance.ClearMovementOverlay();
                return;
        }
            else{
                UnitManager.Instance.ShowMovementOverlay();
            }
                break;
            
            case GameState.PlayerAttack:

            if(UnitManager.Instance.Player != null) {
                UnitManager.Instance.Player.CloseAbilitiesMenu();
            }
            
            if (UnitManager.Instance.hasPerformedAction) {
                    MenuManager.Instance.EventMessages("You already attacked!");
                    UnitManager.Instance.ClearAttackOverlay();
                    return;
            }
            else{    
                StartCoroutine(DelayedShowAttackOverlay());
                Debug.Log("Attack overlay shown.");
                }    
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
                if(!UnitManager.Instance.hasPerformedAction) {
                UnitManager.Instance.Player.OpenAbilities(GameState.ChooseOption);
                }
                else {
                    MenuManager.Instance.EventMessages("You performed an action already!");
                    return;
                }

                break;
            case 1:
                UpdateGameState(GameState.PlayerMove);
                break;
            case 2:
                UpdateGameState(GameState.Flee);
                //SceneManager.LoadScene("Shop (Nick)");
                UnitManager.Instance.endedTurn = true;
                UnitManager.Instance.TurnCheck();
                UpdateGameState(GameState.EnemyChoose);
                break;
            default:
                break;

        }
    }

    private IEnumerator DelayedShowAttackOverlay() {
        yield return new WaitForEndOfFrame(); // Wait for the current frame to finish
        UnitManager.Instance.ShowAttackOverlay();
        Debug.Log("Attack overlay shown after delay.");
    }
    public void MoveLogic()
    {

    }

    private void SpawnPlayerUnits()
    {
        
    Instance.UpdateGameState(GameState.ChooseOption);
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
    Flee,
    Heal,
    Bane
}
}