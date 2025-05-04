using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy1 : BaseEnemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override List<Tile> getMovementTiles()
    {
        float tempRange = this.getMovementRange();
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => t._position.x == this.OccupiedTile._position.x).ToList();

        return _inRangeTiles;
    }
}
