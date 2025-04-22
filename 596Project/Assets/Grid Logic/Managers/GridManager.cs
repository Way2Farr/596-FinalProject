using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] 
    private int _width, _height;

    [SerializeField] private Transform _gridMother;
    [SerializeField] 
    private Tile _tilePrefab, _obstacleTilePrefab;

    [SerializeField]
    private Transform _cam;

    public Dictionary<Vector2, Tile> _tiles;

    [SerializeField]
    private Vector2 heroSpawnPosition = new Vector2(2, 2);

    [SerializeField]
    private Vector2 enemySpawnPosition = new Vector2(7, 7);

    [SerializeField]
    public int GridScale = 10;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Spawns in Grid 
    void GenerateGrid()
    {

        // Each Tile is connected to a Position vector
        _tiles = new Dictionary<Vector2, Tile>();

        _gridMother.localScale = new Vector3(GridScale, GridScale, GridScale);
        // Generate the grid according to _width and _height
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                // functionality to generate random obstacles; not implemented
                var randomTile = Random.Range(0, 6) == 4 ? _tilePrefab : _tilePrefab;

                // Instantiate tile position according to the scale of the grid. 
                // The + 4f is to offset (I don't know why)
                var spawnedTile = Instantiate(randomTile, new Vector3(x * GridScale, Mathf.RoundToInt(y * GridScale) + 4f), Quaternion.identity, _gridMother);
                spawnedTile.name = $"Tile {x} {y}";

                
                // spawn each tile
                spawnedTile.Init(x, y);

                // add Tile to dictionary
                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        // set camera to center on grid
        _cam.transform.position = GridScale * new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -9);
        _cam.transform.position = _cam.transform.position + new Vector3(0, -6f);

        // Spawn in Player and Enemy
        UnitManager.Instance.SpawnHeroes();
        UnitManager.Instance.SpawnEnemies();
    }

    public Tile GetHeroSpawnTile()
    {
        return GetTileAtPoint(heroSpawnPosition);
    }

    public Tile GetEnemySpawnTile()
    {
        return GetTileAtPoint(enemySpawnPosition);
    }


    // Get Tile at a given (x, y)
    public Tile GetTileAtPoint(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }
}
