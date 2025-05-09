using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    protected Color _baseColor, _offsetColor;
    [SerializeField]
    protected SpriteRenderer _renderer;
    [SerializeField]
    protected GameObject _highlight;

    [SerializeField]
    protected GameObject _rangeIndicator;

    [SerializeField]
    protected GameObject _attackRangeIndicator;

    [SerializeField] private bool _isWalkable = true;

    [SerializeField]
    public Vector2 _position;

    [SerializeField]
    public bool _inMovementRange, _inAttackRange = false;

    public BaseUnit OccupiedUnit;
    public bool Walkable => _isWalkable && OccupiedUnit != null;

    public virtual void Init(int x, int y)
    {
        //_renderer.color = isOffset ? _baseColor : _offsetColor;
    }

    private void OnMouseEnter()
    {
        _highlight.SetActive(true);
        //Debug.Log("Hovering square.");
    }

    private void OnMouseExit()
    {
        _highlight.SetActive(false);
        //Debug.Log("Not hovering square.");
    }

    private void OnMouseDown()
    {

        // TODO: Make into separate functions
        // MOUSE DOWN LOGIC IF PLAYER ATTACK
        if (GameManager.Instance.State == GameManager.GameState.PlayerAttack)
        {
            if (OccupiedUnit != null)
            {
                if (_inAttackRange)
                {
                    if (UnitManager.Instance.SelectedHero != null)
                    {
                        var enemy = (BaseEnemy)OccupiedUnit;

                        Destroy(enemy.gameObject);
                        UnitManager.Instance.SetSelectedHero(null);
                        UnitManager.Instance.ClearAttackOverlay();
                        GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
                    }
                }
                

            }
            else if (_inAttackRange)
            {
                UnitManager.Instance.ClearAttackOverlay();
                GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
            }
        }

        // MOUSE DOWN LOGIC IF PLAYER MOVE
        else if (GameManager.Instance.State == GameManager.GameState.PlayerMove)
        {
            if (UnitManager.Instance.SelectedHero != null && _inMovementRange)
            {
                SetUnit(UnitManager.Instance.SelectedHero);
                UnitManager.Instance.ShowMovementOverlay();
                UnitManager.Instance.SetSelectedHero(null);

                UnitManager.Instance._startMoving = true;
                UnitManager.Instance.ClearMovementOverlay();
                //GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);

            }
        }
        

        
    }

    public void RangeActive()
    {
        if (GameManager.Instance.State == GameManager.GameState.PlayerMove)
        {
            _inMovementRange = true;
            _rangeIndicator.SetActive(true);
        }
        else
        {
            _inAttackRange = true;
            _attackRangeIndicator.SetActive(true);
        }
        
    }

    public void RangeInactive()
    {
        if (GameManager.Instance.State == GameManager.GameState.PlayerMove)
        {
            _inMovementRange = false;
            _rangeIndicator.SetActive(false);
        }
        else
        {
            _inAttackRange = false;
            _attackRangeIndicator.SetActive(false);
        }
    }


    public void SetUnit(BaseUnit unit)
    {

        if ((GameManager.Instance.State == GameManager.GameState.PlayerMove) || (GameManager.Instance.State == GameManager.GameState.EnemyMove))
        {
            // Set original tile
            UnitManager.Instance._startingTile = unit.OccupiedTile;
        }
        else
        {
            unit.transform.position = new Vector3(transform.position.x, transform.position.y, -9);
        }


        

        if (unit.OccupiedTile != null)
        {
            unit.OccupiedTile.OccupiedUnit = null;
        }



        if ((GameManager.Instance.State == GameManager.GameState.PlayerMove) || (GameManager.Instance.State == GameManager.GameState.EnemyMove))
        {
            // Set ending tile
            UnitManager.Instance._endTile = this;
        }
        
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }
}


