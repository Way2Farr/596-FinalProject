using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor.AnimatedValues;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;



public class UnitManager : MonoBehaviour
{

    public static UnitManager Instance;
    private List<ScriptableUnit> _units;

    [SerializeField]
    public BaseUnit[] _setUnits;

    public BasePlayer Player;
    public BasePlayer SelectedHero;
    public BaseEnemy Enemy;

    [SerializeField]
    public float MoveSpeed = 0.07f;

    // Set tile 
    [SerializeField]
    public Tile _startingTile, _endTile;
    public bool _startMoving;
    public bool hasPerformedAction = false;
    public bool hasMoved = false;
    public bool hasHealed = false;
    public bool hasMagic = false;
    public bool endedTurn = false;
    

    private void Awake()
    {
        Instance = this;

        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }

    public void Update()
    {
        MoveTile();
        EnemyMoveTile();
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
        //var enemyCount = 1;

        if (StatManager.Instance == null || StatManager.Instance._currentRound == 1)
        {
            var randomPrefab = GetSpecificUnit<BaseEnemy>(Faction.Enemy, RoundNumber.Round1);
            var spawnedEnemy = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetEnemySpawnTile();

            spawnedEnemy.OccupiedTile = randomSpawnTile;
            randomSpawnTile.SetUnit(spawnedEnemy);

            Enemy = spawnedEnemy;
        }
        else if (StatManager.Instance._currentRound == 2)
        {
            var randomPrefab = GetSpecificUnit<BaseEnemy>(Faction.Enemy, RoundNumber.Round2);
            var spawnedEnemy = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetEnemySpawnTile();

            spawnedEnemy.OccupiedTile = randomSpawnTile;
            randomSpawnTile.SetUnit(spawnedEnemy);

            Enemy = spawnedEnemy;
        }
        else if (StatManager.Instance._currentRound == 3)
        {
            var randomPrefab = GetSpecificUnit<BaseEnemy>(Faction.Enemy, RoundNumber.Round3);
            var spawnedEnemy = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetEnemySpawnTile();

            spawnedEnemy.OccupiedTile = randomSpawnTile;
            randomSpawnTile.SetUnit(spawnedEnemy);

            Enemy = spawnedEnemy;
        }
        /*for (int i = 0; i < enemyCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
            var spawnedEnemy = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetEnemySpawnTile();

            spawnedEnemy.OccupiedTile = randomSpawnTile;
            randomSpawnTile.SetUnit(spawnedEnemy);

            Enemy = spawnedEnemy;
        }*/
    }

    private T GetRandomUnit<T>(Faction Faction) where T : BaseUnit 
    {
        return (T)_units.Where(u => u.Faction == Faction).OrderBy(o => Random.value).First().UnitPrefab;
    }

    private T GetSpecificUnit<T>(Faction Faction, RoundNumber RoundNumber) where T : BaseUnit
    {
        return (T)_units.Where(u => u.RoundNumber == RoundNumber).First().UnitPrefab;
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
        Instance.SetSelectedHero(Player);
        //float tempRange = (float)Player.getAttackRange();
        //List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => Vector2.Distance(Player.transform.position, t.transform.position) <= tempRange).ToList();
        foreach (Tile tile in Player.getAttackTiles())
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

        StartCoroutine(EnemyPause()); // wait one second before moving
    }

    IEnumerator EnemyPause()
    {
        yield return new WaitForSeconds(0.1f);

        foreach (Tile tile in Enemy.getMovementTiles())
        {
            tile.RangeActive();
        }

        yield return new WaitForSeconds(0.9f);
        
        // if state = EnemyMove
        List<Tile> EnemyRange = Enemy.getMovementTiles();
        Tile enemyTile = EnemyRange[Random.Range(0, EnemyRange.Count)];
        enemyTile.SetUnit(Enemy);
        _startMoving = true;
        ClearEnemyMovementOverlay();

        // if state = EnemyAttack call other function
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
        Debug.Log("Display enemy attack overlay");
        //UnitManager.Instance.SetSelectedHero(Player);
        //float tempRange = (float)Player.getAttackRange();
        //List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => Vector2.Distance(Player.transform.position, t.transform.position) <= tempRange).ToList();
        foreach (Tile tile in Enemy.getAttackTiles())
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
            Player.startMoving();

            //Debug.Log(Player.transform.position.x - _endTile.transform.position.x);

            // TODO: Problem for later; offset isn't programmed correctly :(
            if (Player.transform.position.x - _endTile.transform.position.x <= 0)
            {
/*                if (Player._childTransform.position.x != -101)
                {
                    Player._childTransform.position = new Vector3(-101, Player._childTransform.position.y, Player._childTransform.position.z);
                }*/
                Player._spriteRenderer.flipX = false;
                //Player._doOffset = false;
            }
            else
            {
                /*                if (Player._childTransform.position.x == -101)
                                {
                                    Player._childTransform.position = new Vector3(Player._childTransform.position.x + Player._animOffset, Player._childTransform.position.y, Player._childTransform.position.z);
                                }*/

                //Player._doOffset = true;
                Player._spriteRenderer.flipX = true;
            }
            
        }


        if (movementFlag && _startMoving)
        {
            //Debug.Log(Vector3.Distance(Player.transform.position, _endTile.transform.position));
            if (Vector3.Distance(Player.transform.position, _endTile.transform.position) <= 9f)
            {
                Player.stopMoving();
                
                //GameManager.Instance.UpdateGameState(GameManager.GameState.EnemyChoose);
                _startMoving = false;
                MovementFlag();
                
                _startingTile = null;
                _endTile = null;

                movementFlag = false;

                Debug.ClearDeveloperConsole();
                Debug.Log("Call choose option - Player movement");
                if (GameManager.Instance.State == GameManager.GameState.PlayerMove)
                {
                    GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
                }
                
            }
            
        }
    }

    public void EnemyMoveTile()
    {


        bool movementFlag = false;
        if (_startMoving && GameManager.Instance.State == GameManager.GameState.EnemyMove && _startingTile != null && _endTile != null)
        {
            Enemy.transform.position = Vector3.MoveTowards(Enemy.transform.position, new Vector3(_endTile.transform.position.x, _endTile.transform.position.y, Enemy.transform.position.z), MoveSpeed);
            movementFlag = true;
            Enemy.startMoving();

            // FLIP ENEMY

            if (Enemy.transform.position.x - _endTile.transform.position.x < 0)
            {
                Enemy._spriteRenderer.flipX = true;
            }
            else
            {
                Enemy._spriteRenderer.flipX = false;
            }

        }

        if (movementFlag)
        {
            //Debug.Log(Vector3.Distance(Player.transform.position, _endTile.transform.position));
            if (Enemy.transform.position.x == _endTile.transform.position.x && Enemy.transform.position.y == _endTile.transform.position.y)
            {

                _startingTile = null;
                _endTile = null;

                
                _startMoving = false;

                if (GameManager.Instance.State == GameManager.GameState.EnemyMove)
                {
                    Enemy.stopMoving();
                    GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
                }
                


            }

        }
    }
    // ---------------- Animation handlers

    //*

    public IEnumerator PlayAttackAnimation(BaseUnit attacker)
    {
        attacker.startAttacking();
        Instance.Player.SpawnSwingParticles();
        Instance.Player.swingParticlesInstance.Play();
        float attackLength = 2.0f;


        // get clip lengths
        AnimationClip[] clips = attacker._unitAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "PlayerAttack":
                    attackLength = clip.length;
                    break;
                case "LightAttack":
                    attackLength = clip.length;
                    break;
                default:
                    attackLength = 2.0f;
                    break;
            }
        }

        StartCoroutine(DelaySoundFX(attacker, attackLength - attackLength * 5.0f/6.0f));
        // wait
        yield return new WaitForSeconds(attackLength);
        Instance.Player.swingParticlesInstance.Stop();
        attacker.stopAttacking();
        // switch states
        if (GameManager.Instance.State == GameManager.GameState.PlayerAttack)
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
            TurnCheck();
        }

        if (GameManager.Instance.State == GameManager.GameState.EnemyAttack)
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
            ClearEnemyAttackOverlay();
        }
        

    }

    public IEnumerator DelaySoundFX (BaseUnit unit, float delay)
    {
        yield return new WaitForSeconds(delay);

        SoundFXManager.Instance.PlayClip(unit._attackFX, this.transform, 0.1f);
    }
    public IEnumerator PlayDamagedAnimation(BaseUnit defender)
    {
        defender.startDamaging();

        float attackLength = 0.5f;


        // get clip lengths
        AnimationClip[] clips = defender._unitAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "PlayerHurt":
                    //attackLength = clip.length;
                    break;
                case "LightHit":
                    //attackLength = clip.length;
                    break;
                default:
                    attackLength = 0.5f;
                    break;
            }
        }

        // wait
        yield return new WaitForSeconds(attackLength);

        defender.stopDamaging();
        // switch states
        if (GameManager.Instance.State == GameManager.GameState.PlayerAttack)
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
            TurnCheck();
        }

        if (GameManager.Instance.State == GameManager.GameState.EnemyAttack)
        {
            //GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
        }

        
    }

    
    //--------------------------------------------------------------------
    public void HandleAttack(BasePlayer Selected, BaseEnemy Enemy) {


        
        Enemy.OnHurt(Selected._attack);

        SetSelectedHero(null);
        AttackFlag();
    }

    public void AttackFlag() {

        hasPerformedAction = true;
        ClearAttackOverlay();
        Debug.Log("attacked has been flagged!");
        //TurnCheck();
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

    if(hasPerformedAction && hasMoved) { // Complete Turn
        
        TurnReset(); 
        if (!Enemy._defeated)
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.EnemyChoose);
        }
        return;
    }

    }
    

    public void TurnReset() {
        hasPerformedAction = false;
        hasMoved = false;
        endedTurn = false;
        Instance.Player.CloseAbilitiesMenu();
        Instance.Player.CloseMagicMenu();
        CheckMagic();
        GameManager.Instance.TurnManager.Tick();

        if (!Enemy._defeated)
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.EnemyChoose);
        }
        

    }


    public void CheckMagic() {
        Player.WindedDuration();
        Enemy.BaneDuration();
        Enemy.StunDuration();
    }

    public void EnemyChoose() {

        // if enemy in range then EnemyAttack
        if (Enemy.PlayerInAttackRange())
        {
            Debug.Log(Enemy.getAttackTiles().Count);
            

            GameManager.Instance.UpdateGameState(GameManager.GameState.EnemyAttack);
        }
        // else EnemyMove
        else
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.EnemyMove);
        }
        
    }

    //----------------ENEMY LOGIC------------------------

    public IEnumerator HandleEnemyAttack(float delay)
    {
        ShowEnemyAttackOverlay();
        yield return new WaitForSeconds(delay);
        Debug.Log("Enemy attack!");
        
        StartCoroutine(PlayAttackAnimation(Enemy));
        
        if(Enemy.PlayerInAttackRange())
        {
            StartCoroutine(PlayDamagedAnimation(Player));
            Player.OnHurt(Enemy._attack);
        }
    }
}
