using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

public class BaseUnit : MonoBehaviour
{
    
    public Tile OccupiedTile;
    public Faction Faction;

    [Header("UI Elements")]
    public Canvas healthbarCanvas;


    [SerializeField]
    public int _maxHealth, _attack, _defense, _movementRange, _attackRange;
    public int _currentHealth;

    [SerializeField]
    private string _name;

    public int getMovementRange()
    {
        return _movementRange;
    }
    public void setMovementRange(int movementRange)
    {
        _movementRange = (movementRange);
    }

    public int getAttackRange() 
    { 
        return (_attackRange);
    }

    public void setAttackRange(int attackRange)
    {
        _attackRange = (attackRange);
    }

    public virtual List<Tile> getMovementTiles ()
    {
        float tempRange = this.getMovementRange();
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => Vector2.Distance(this.transform.position, t.transform.position) <= tempRange).ToList();

        return _inRangeTiles;
    }

    public virtual List<Tile> getAttackTiles()
    {
        float tempRange = this.getMovementRange();
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => Vector2.Distance(this.transform.position, t.transform.position) <= tempRange).ToList();

        return _inRangeTiles;
    }
}
