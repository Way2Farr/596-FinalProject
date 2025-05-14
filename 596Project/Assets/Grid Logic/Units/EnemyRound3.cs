using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyRound3 : BaseEnemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (_healthbar != null) _healthbar.value = (float)_currentHealth / (float)_maxHealth;
    }

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
