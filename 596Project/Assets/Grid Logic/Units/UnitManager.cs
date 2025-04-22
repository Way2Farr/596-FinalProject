using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEngine;



public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    private List<ScriptableUnit> _units;

    public BasePlayer Player;
    public BasePlayer SelectedHero;
    private void Awake()
    {
        Instance = this;

        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
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

            randomSpawnTile.SetUnit(spawnedEnemy);
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
        float tempRange = (float)Player.getMovementRange();
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => Vector2.Distance(Player.transform.position, t.transform.position) <= tempRange).ToList();
        foreach (Tile tile in _inRangeTiles)
        {
            tile.RangeActive();
        }
    }

    public void ClearMovementOverlay()
    {
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.ToList();
        foreach (Tile tile in _inRangeTiles)
        {
            tile.RangeInactive();
        }
    }

    public void ShowAttackOverlay()
    {

        ClearAttackOverlay();
        UnitManager.Instance.SetSelectedHero(Player);
        float tempRange = (float)Player.getAttackRange();
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => Vector2.Distance(Player.transform.position, t.transform.position) <= tempRange).ToList();
        foreach (Tile tile in _inRangeTiles)
        {
            tile.RangeActive();
        }
    }

    public void ClearAttackOverlay()
    {
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.ToList();
        foreach (Tile tile in _inRangeTiles)
        {
            tile.RangeInactive();
        }
    }
}
