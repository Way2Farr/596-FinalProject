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

    //------------------------------------------------------------------------

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
        if (GameManager.Instance.State == GameManager.GameState.PlayerAttack)
        {
            HandlePlayerAttack();
        }
        else if (GameManager.Instance.State == GameManager.GameState.PlayerMove) {
            HandlePlayerMove();
        }
        else if (GameManager.Instance.State == GameManager.GameState.Bane)
            HandlePlayerBane();
        else if (GameManager.Instance.State == GameManager.GameState.Stun)
            HandlePlayerStun();
    }
        // TODO: Make into separate functions
        // MOUSE DOWN LOGIC IF PLAYER ATTACK
    private void HandlePlayerAttack() {

            if (OccupiedUnit != null && _inAttackRange)
            {
                    if (UnitManager.Instance.SelectedHero != null)
                    {
                        var enemy = OccupiedUnit as BaseEnemy;
                        Debug.Log("enemy health was at: " + enemy._maxHealth);
                        UnitManager.Instance.HandleAttack(UnitManager.Instance.SelectedHero, enemy);
    
                    }

                Debug.ClearDeveloperConsole();
                Debug.Log("Call choose option - Attack in range");

                StartCoroutine(UnitManager.Instance.PlayAttackAnimation(UnitManager.Instance.Player));
                StartCoroutine(UnitManager.Instance.PlayDamagedAnimation(UnitManager.Instance.Enemy));
                /*if (GameManager.Instance.State == GameManager.GameState.PlayerAttack)
                {
                    GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
                }*/

        }
            else if (_inAttackRange)
            {
                UnitManager.Instance.AttackFlag();
                UnitManager.Instance.ClearAttackOverlay();

                Debug.ClearDeveloperConsole();
                Debug.Log("Call choose option - Attack not in range");

                /*if (GameManager.Instance.State == GameManager.GameState.PlayerAttack)
                {
                    GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
                }*/

                StartCoroutine(UnitManager.Instance.PlayAttackAnimation(UnitManager.Instance.Player));

        }
        
    }

    private void HandlePlayerMove() {

        // MOUSE DOWN LOGIC IF PLAYER MOVE
            if (UnitManager.Instance.SelectedHero != null && _inMovementRange)
            {
                SetUnit(UnitManager.Instance.SelectedHero); 
                UnitManager.Instance.ShowMovementOverlay();
                UnitManager.Instance.SetSelectedHero(null);

                UnitManager.Instance._startMoving = true;
                UnitManager.Instance.ClearMovementOverlay();


            }
    }

    public void RangeActive()
    {
        if (GameManager.Instance.State == GameManager.GameState.PlayerMove || GameManager.Instance.State == GameManager.GameState.EnemyMove)
        {
            _inMovementRange = true;
            _rangeIndicator.SetActive(true);
        }
        else if (GameManager.Instance.State == GameManager.GameState.PlayerAttack || GameManager.Instance.State == GameManager.GameState.EnemyAttack)
        {

            _inAttackRange = true;
            _attackRangeIndicator.SetActive(true);
        }
        else if (GameManager.Instance.State == GameManager.GameState.Bane){

            UnitManager.Instance.Player._inBaneRange = true;
            _attackRangeIndicator.SetActive(true);
        }
        else if (GameManager.Instance.State == GameManager.GameState.Stun){
            UnitManager.Instance.Player._inStunRange = true;
            _attackRangeIndicator.SetActive(true);
        }
        
    }
    public void RangeInactive()
    {
        if (GameManager.Instance.State == GameManager.GameState.PlayerMove || GameManager.Instance.State == GameManager.GameState.EnemyMove)
        {
            _inMovementRange = false;
            _rangeIndicator.SetActive(false);
        }
        else
        {
            _inAttackRange = false;
            _attackRangeIndicator.SetActive(false);


            UnitManager.Instance.Player._inBaneRange = false;
            UnitManager.Instance.Player._inStunRange = false;
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


    public void HandlePlayerBane() {
        
        if (OccupiedUnit != null && UnitManager.Instance.Player.GetBaneTiles().Contains(this))
    {
        var enemy = OccupiedUnit as BaseEnemy;

        if (enemy != null){
            UnitManager.Instance.Player.HandleBane(enemy);
        }
        else {
            UnitManager.Instance.Player.ClearBaneOverlay();
            GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
        }
    }
    else if (UnitManager.Instance.Player._inBaneRange)
            {
                UnitManager.Instance.Player.ClearBaneOverlay();
            GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
        }
    }
    public void HandlePlayerStun() {
        
        if (OccupiedUnit != null && UnitManager.Instance.Player.GetBaneTiles().Contains(this))
    {
        var enemy = OccupiedUnit as BaseEnemy;

        if (enemy != null){
            UnitManager.Instance.Player.HandleStun(enemy);
        }
        else {
            UnitManager.Instance.Player.ClearStunOverlay();
            GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
        }
    }
    else if (UnitManager.Instance.Player._inStunRange)
            {
                UnitManager.Instance.Player.ClearBaneOverlay();
            GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
        }
    }
}


