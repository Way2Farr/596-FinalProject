using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public enum BattleState {
        Idle, // Waiting for other actions to queue
        Perform,
        Performing // Allows animations to go through
    }

    private BattleState state;

    private Queue<Battler> turnQueue;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }

    private void Update() {
        switch(state) {

            case BattleState.Idle:
            if(turnQueue.Count > 0) {
                Battler currentBattler = turnQueue.Dequeue();
                currentBattler.State = Battler.TurnState.ChooseAction;
            }
                break;
            case BattleState.Perform:
                break;
            case BattleState.Performing:
                break;
        }
    }

    public void Enqueue(Battler battler) {
        turnQueue.Enqueue(battler);
    }
}
