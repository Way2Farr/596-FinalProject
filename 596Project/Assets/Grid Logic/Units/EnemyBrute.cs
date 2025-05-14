using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBrute : BaseEnemy
{


    public override List<Tile> getMovementTiles()
    {
        float tempRange = this.getMovementRange();
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => Vector2.Distance(this.transform.position, t.transform.position) <= tempRange && t._position != OccupiedTile._position).ToList();

        return _inRangeTiles;
    }

    public override List<Tile> getAttackTiles()
    {

        float tempRange = this.getAttackRange();
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => t._position.x == this.OccupiedTile._position.x || t._position.y == this.OccupiedTile._position.y).ToList();

        return _inRangeTiles;

        
    }
}
