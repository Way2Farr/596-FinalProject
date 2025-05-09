using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.AnimatedValues;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;



public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    private List<ScriptableUnit> _units;

    public BasePlayer Player;
    public BasePlayer SelectedHero;
    public BaseEnemy Enemy;

    [SerializeField]
    public float MoveSpeed = 0.6f;

    // Set tile 
    [SerializeField]
    public Tile _startingTile, _endTile;
    public bool _startMoving;
    public bool hasAttacked = false;
    public bool hasMoved = false;
    public bool endedTurn = false;
    private void Awake()
    {
        Instance = this;

        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }

    public void Update()
    {
        MoveTile();
        MenuManager.Instance.UpdateCount();
        MenuManager.Instance.UnitStats();
    }
    public void SpawnHeroes()
    {


        // Set to spawn one random player for now
        var heroCount = 1;

        for (int i = 0; i < heroCount; i++)
        {
            var randomPrefab = GetRandomUnit<BasePlayer>(Faction.Hero);
            var spawnedHero = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetHeroSpawnTile();

            spawnedHero.OccupiedTile = randomSpawnTile;
            randomSpawnTile.SetUnit(spawnedHero);
            Player = spawnedHero;
        }


    }

    public void SpawnEnemies()
    {
    
        // Set to spawn one random enemy for now
        var enemyCount = 1;

        for (int i = 0; i < enemyCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
            var spawnedEnemy = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetEnemySpawnTile();

            spawnedEnemy.OccupiedTile = randomSpawnTile;
            randomSpawnTile.SetUnit(spawnedEnemy);

            Enemy = spawnedEnemy;
        }
    }

    private T GetRandomUnit<T>(Faction Faction) where T : BaseUnit 
    {
        return (T)_units.Where(u => u.Faction == Faction).OrderBy(o => Random.value).First().UnitPrefab;
    }


    // Set the selected player
    public void SetSelectedHero (BasePlayer player)
    {
        SelectedHero = player;
        
    }

    public void ShowMovementOverlay()
    {

        ClearMovementOverlay();
        UnitManager.Instance.SetSelectedHero(Player);
        //float tempRange = (float)Player.getMovementRange();
        //List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => Vector2.Distance(Player.transform.position, t.transform.position) <= tempRange).ToList();
        foreach (Tile tile in Player.getMovementTiles())
        {
            tile.RangeActive();
        }
    }

    public void ClearMovementOverlay()
    {
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.ToList();
        foreach (Tile tile in GridManager.Instance._tilesList)
        {
            tile.RangeInactive();
        }
    }

    public void ShowAttackOverlay()
    {

        ClearAttackOverlay();
        UnitManager.Instance.SetSelectedHero(Player);
        //float tempRange = (float)Player.getAttackRange();
        //List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => Vector2.Distance(Player.transform.position, t.transform.position) <= tempRange).ToList();
        foreach (Tile tile in Player.getMovementTiles())
        {
            tile.RangeActive();
        }
    }

    public void ClearAttackOverlay()
    {
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.ToList();
        foreach (Tile tile in GridManager.Instance._tilesList)
        {
            tile.RangeInactive();
        }
       
    }

    public void ShowEnemyMovementOverlay()
    {

        ClearEnemyMovementOverlay();
        //UnitManager.Instance.SetSelectedHero(Player);
        //float tempRange = (float)Player.getMovementRange();
        //List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => Vector2.Distance(Player.transform.position, t.transform.position) <= tempRange).ToList();
        foreach (Tile tile in Enemy.getMovementTiles())
        {
            tile.RangeActive();
        }
    }

    public void ClearEnemyMovementOverlay()
    {
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.ToList();
        foreach (Tile tile in GridManager.Instance._tilesList)
        {
            tile.RangeInactive();
        }
    }

    public void ShowEnemyAttackOverlay()
    {

        ClearEnemyAttackOverlay();
        //UnitManager.Instance.SetSelectedHero(Player);
        //float tempRange = (float)Player.getAttackRange();
        //List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => Vector2.Distance(Player.transform.position, t.transform.position) <= tempRange).ToList();
        foreach (Tile tile in Enemy.getMovementTiles())
        {
            tile.RangeActive();
        }
    }

    public void ClearEnemyAttackOverlay()
    {
        //List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.ToList();
        foreach (Tile tile in GridManager.Instance._tilesList)
        {
            tile.RangeInactive();
        }
    }

    public void MoveTile()
    {
        bool movementFlag = false;
        if(_startMoving && GameManager.Instance.State == GameManager.GameState.PlayerMove && _startingTile != null && _endTile != null)
        {
            
            Player.transform.position = Vector3.MoveTowards(Player.transform.position, new Vector3(_endTile.transform.position.x, _endTile.transform.position.y, Player.transform.position.z), MoveSpeed);
            movementFlag = true;
            
        }
        
        if (movementFlag)
        {
            //Debug.Log(Vector3.Distance(Player.transform.position, _endTile.transform.position));
            if (Vector3.Distance(Player.transform.position, _endTile.transform.position) <= 9f)
            {
                
                GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
                _startMoving = false;
                MovementFlag();
                
                
            }
            
        }
        
    }

//--------------------------------------------------------------------
    public void HandleAttack(BasePlayer Selected, BaseEnemy Enemy) {

        int setDamage = Selected._attack - (Enemy._defense / 3);
        setDamage = Mathf.Max(setDamage,0); // If it goes negative set it to zero
        Enemy._maxHealth -= setDamage;

        Debug.Log("Enemy Health is at: " + Enemy._maxHealth + " Damage is at: " + setDamage);

        if (Enemy._maxHealth <= 0) {
            Destroy(Enemy.gameObject);
        }

        SetSelectedHero(null);
        AttackFlag();

    }

    public void AttackFlag() {

        hasAttacked = true;
        ClearAttackOverlay();
        Debug.Log("attacked has been flagged!");
        TurnCheck();
    }

    public void MovementFlag() {
        hasMoved = true;
        ClearMovementOverlay();
        Debug.Log("Movement has been flagged!");
        TurnCheck();
    }

    public void TurnCheck() {

    if(endedTurn) {
        Debug.Log("Manually ended turn");
        TurnReset();
        return;
    }

    if(hasAttacked && hasMoved) { // Complete Turn
        
        TurnReset(); 
        GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
        return;
    }

    }
    

    public void TurnReset() {
        hasAttacked = false;
        hasMoved = false;
        endedTurn = false;
        GameManager.Instance.TurnManager.Tick();

    }


    
}
