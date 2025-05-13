using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using TMPro;

public class BaseUnit : MonoBehaviour
{
    
    public Tile OccupiedTile;
    public Faction Faction;
    public Animator _unitAnimator;
    public SpriteRenderer _spriteRenderer;
    static readonly int IsIdle = Animator.StringToHash("IsIdle");
    static readonly int IsMoving = Animator.StringToHash("IsMoving");
    static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    static readonly int IsDamaged = Animator.StringToHash("IsDamaged");

    [Header("UI Elements")]
    public Canvas healthbarCanvas;


    [SerializeField]
    public int _maxHealth, _attack, _defense, _movementRange, _attackRange;
    public int _currentHealth;

    [SerializeField]
    private string _name;
    public void Awake()
    {
        _unitAnimator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
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
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => Vector2.Distance(this.transform.position, t.transform.position) <= tempRange && !t.OccupiedUnit).ToList();

        return _inRangeTiles;
    }

    public virtual List<Tile> getAttackTiles()
    {
        float tempRange = this.getAttackRange();
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => Vector2.Distance(this.transform.position, t.transform.position) <= tempRange && t._position != OccupiedTile._position).ToList();

        return _inRangeTiles;
    }

    public void startMoving ()
    {
        _unitAnimator.SetBool(IsIdle, false);
        _unitAnimator.SetBool(IsMoving, true);
    }

    public void stopMoving ()
    {
        _unitAnimator.SetBool(IsIdle, true);
        _unitAnimator.SetBool(IsMoving, false);
    }


}
