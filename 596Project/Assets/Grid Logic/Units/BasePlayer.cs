using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasePlayer : BaseUnit
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Rook behavior (can use later)
    /*public override List<Tile> getMovementTiles()
    {
        float tempRange = this.getMovementRange();
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => t._position.x == this.OccupiedTile._position.x || t._position.y == this.OccupiedTile._position.y).ToList();

        return _inRangeTiles;

        //return base.getMovementTiles();
    }
*/
    // Movement behavior
    public override List<Tile> getMovementTiles()
    {
        float tempRange = this.getMovementRange() / 10;
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => Mathf.Abs(t._position.x - this.OccupiedTile._position.x) <= tempRange && Mathf.Abs(t._position.y - this.OccupiedTile._position.y) <= tempRange && !(t.Walkable)).ToList();

        return _inRangeTiles;
    }
}
