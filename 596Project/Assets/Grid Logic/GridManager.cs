using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] 
    private int _width, _height;

    [SerializeField] private Transform _gridMother;
    [SerializeField] 
    private Tile _tilePrefab, _obstacleTilePrefab;

    [SerializeField]
    private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var randomTile = Random.Range(0, 6) == 4 ? _tilePrefab : _tilePrefab;
                var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity, _gridMother);
                spawnedTile.name = $"Tile {x} {y}";

                
                spawnedTile.Init(x, y);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
        _cam.transform.position = _cam.transform.position + new Vector3(0, -1);
    }

    public Tile GetTileAtPoint(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }
}
