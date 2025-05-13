using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Linq;

public class BaseUnit : MonoBehaviour
{
    
    public Tile OccupiedTile;
    public Faction Faction;
    public Animator _unitAnimator;
    public SpriteRenderer _spriteRenderer;
    public Transform _childTransform;
    static readonly int IsIdle = Animator.StringToHash("IsIdle");
    static readonly int IsMoving = Animator.StringToHash("IsMoving");
    static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    static readonly int IsDamaged = Animator.StringToHash("IsDamaged");

    [SerializeField]
    public int _animOffset = 0;
    public bool _doOffset = false;
    public float _originalChildX;
    [Header("UI Elements")]
    //public Canvas healthbarCanvas;

    [SerializeField]
    private Slider healthbar;

    [SerializeField]
    public int _maxHealth, _attack, _defense, _movementRange, _attackRange;
    public int _currentHealth;

    [SerializeField]
    private string _name;
    public void Awake()
    {
        _unitAnimator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _childTransform = GetComponentInChildren<Transform>();
        _originalChildX = _childTransform.localPosition.x;
        _currentHealth = _maxHealth;
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

        if(_doOffset)
        {
            _childTransform.localPosition = new Vector3(_originalChildX + _animOffset, _childTransform.localPosition.y, _childTransform.localPosition.x);
        }

    }

    public void stopMoving ()
    {
        _unitAnimator.SetBool(IsIdle, true);
        _unitAnimator.SetBool(IsMoving, false);
    }

    public void startAttacking()
    {
        _unitAnimator.SetBool(IsIdle, false);
        _unitAnimator.SetBool(IsAttacking, true);

    }

    public void stopAttacking()
    {
        _unitAnimator.SetBool(IsIdle, true);
        _unitAnimator.SetBool(IsAttacking, false);
    }

    public void startDamaging()
    {
        _unitAnimator.SetBool(IsIdle, false);
        _unitAnimator.SetBool(IsDamaged, true);

    }

    public void stopDamaging()
    {
        _unitAnimator.SetBool(IsIdle, true);
        _unitAnimator.SetBool(IsDamaged, false);
    }

}
