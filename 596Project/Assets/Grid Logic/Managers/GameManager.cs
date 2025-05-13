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
    [SerializeField]

    AudioClip _playerMoveSound, _menuMoveSound, _menuFightSound, _menuEndSound, _victorySound, _defeatSound;

    [SerializeField]

    float clipVolume = 0.1f;

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

        if (StatManager.Instance != null)
        {
            if (StatManager.Instance._currentRound != 1)
            {
                UnitManager.Instance.Player._attack = StatManager.Instance._attack;
                UnitManager.Instance.Player._maxHealth = StatManager.Instance._maxHealth;
                UnitManager.Instance.Player._defense = StatManager.Instance._defense;
                UnitManager.Instance.Player._attackRange = StatManager.Instance._attackRange;
                UnitManager.Instance.Player._movementRange = StatManager.Instance._movementRange;
                UnitManager.Instance.Player._attackRange = StatManager.Instance._attackRange;
                UnitManager.Instance.Player._manaPoint = StatManager.Instance._manaPoint;
                Debug.Log("Manapoint is at" + UnitManager.Instance.Player._manaPoint );

            }
        }
    }

    public void UpdateGameState(GameState newState)
    {

        if (GameManager.Instance.State == GameManager.GameState.Victory || GameManager.Instance.State == GameManager.GameState.Lose)
        {
            return;
        }
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

            SoundFXManager.Instance.PlayClip(_menuMoveSound, this.transform, clipVolume);

            if (UnitManager.Instance.hasMoved) {
                MenuManager.Instance.EventMessages("You already moved!");
                UnitManager.Instance.ClearMovementOverlay();
                return;
            }

            else{
                
                UnitManager.Instance.ShowMovementOverlay();
                UnitManager.Instance.Player.CloseAbilitiesMenu();
            }
                break;

            case GameState.PlayerAttack:

            SoundFXManager.Instance.PlayClip(_menuFightSound, this.transform, clipVolume);

            if(UnitManager.Instance.Player != null) {
                UnitManager.Instance.Player.CloseAbilitiesMenu();
            }
            
            if (UnitManager.Instance.hasPerformedAction) {
                    MenuManager.Instance.EventMessages("You already attacked!");
                    UnitManager.Instance.ClearAttackOverlay();
                    return;
                }
                else{
                    
                    UnitManager.Instance.ShowAttackOverlay();
                }    
                break;

            case GameState.EnemyChoose:
                if(UnitManager.Instance.Enemy.isStunned) {
                    Debug.Log("Enemy is stunned!");
                    StartCoroutine(HandleStunned());
                  }
                  else
                UnitManager.Instance.EnemyChoose();
                break;

            case GameState.EnemyMove:
                UnitManager.Instance.ShowEnemyMovementOverlay();
                break;
            case GameState.EnemyAttack:
                StartCoroutine(UnitManager.Instance.HandleEnemyAttack(1.0f));
                break;
            case GameState.Victory:
                SoundFXManager.Instance.PlayClip(_victorySound, this.transform, clipVolume);
                VictoryScreen.Instance.StartVictoryScreen();
                break;
            case GameState.Lose:
                SoundFXManager.Instance.PlayClip(_defeatSound, this.transform, clipVolume);
                VictoryScreen.Instance.StartLossScreen();
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

                SoundFXManager.Instance.PlayClip(_menuMoveSound, this.transform, clipVolume);

                if (!UnitManager.Instance.hasPerformedAction) {
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
                SoundFXManager.Instance.PlayClip(_menuEndSound, this.transform, clipVolume);
                //SceneManager.LoadScene("Shop (Nick)");
                UnitManager.Instance.endedTurn = true;
                UnitManager.Instance.TurnCheck();
                break;

            case 99:
                SoundFXManager.Instance.PlayClip(_menuEndSound, this.transform, clipVolume);
                SceneManager.LoadScene("Shop (Nick)");
                UnitManager.Instance.endedTurn = true;
                UnitManager.Instance.TurnCheck();
                break;
            default:
                break;

        }
    }

    private IEnumerator HandleStunned() {
        yield return new WaitForSeconds(1.0f);
        UpdateGameState(GameState.ChooseOption);
    }

    private void SpawnPlayerUnits()
    {
        
        GameManager.Instance.UpdateGameState(GameState.ChooseOption);
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
    Bane,
    Stun
}
}