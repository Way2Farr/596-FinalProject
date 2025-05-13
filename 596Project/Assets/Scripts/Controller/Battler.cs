using Unity.VisualScripting;
using UnityEngine;

public class Battler : MonoBehaviour
{
    public enum TurnState {
        Idle,
        Queued,
        ChooseAction,
        ChooseTarget,
        Action,
        Dead
    }
    

    public TurnState State {
        get;
        set;
        }

    protected float cd;
    [SerializeField] protected float maxCd = 3f;

    private void Update()
    {
    switch(State) {
        case TurnState.Idle:
            Idle();
            break;
        case TurnState.Queued:
            Queued();
            break;
        case TurnState.ChooseAction:
            ChooseAction();
            break;
        case TurnState.ChooseTarget:
            ChooseTarget();
            break;
        case TurnState.Action:
            ChooseAction();
            break;
        case TurnState.Dead:
            Dead();
            break;
        }
    }
    protected virtual void Idle() {
        cd += Time.deltaTime;
        if (cd >= maxCd) {
            BattleManager.Instance.Enqueue(this);
            State = TurnState.Queued;
        }
    }
    protected virtual void Queued() {

    }
    protected virtual void ChooseAction() {

    }
    protected virtual void ChooseTarget() {

    }
    protected virtual void Action() {

    }
    protected virtual void Dead() {

    }
}

