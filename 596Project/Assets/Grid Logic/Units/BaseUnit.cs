using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Linq;

public class BaseUnit : MonoBehaviour
{

    public int _roundNumber = 1;
    public Tile OccupiedTile;
    public Faction Faction;
    public Animator _unitAnimator;
    public SpriteRenderer _spriteRenderer;
    public Transform _childTransform;
    public Slider _healthbar;
    static readonly int IsIdle = Animator.StringToHash("IsIdle");
    static readonly int IsMoving = Animator.StringToHash("IsMoving");
    static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    static readonly int IsDamaged = Animator.StringToHash("IsDamaged");

    [SerializeField]
    public int _animOffset = 0;
    public bool _doOffset = false;
    public float _originalChildX;
    [Header("UI Elements")]

    [SerializeField]
    public int _maxHealth, _attack, _defense, _movementRange, _attackRange, _manaPoint;
    public int _currentHealth;

    [SerializeField]
    private string _name;

    [SerializeField]
    public AudioClip _attackFX, _moveFX, _damageFX;
    public void Awake()
    {
        _unitAnimator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _childTransform = GetComponentInChildren<Transform>();
        _originalChildX = _childTransform.localPosition.x;
        _healthbar = GetComponentInChildren<Slider>();
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


        if(GameObject.FindGameObjectsWithTag("Sound").Length <= 1)
        {
            SoundFXManager.Instance.PlayClip(_moveFX, this.transform, 0.1f);
        }
        
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

        if (GameManager.Instance.State == GameManager.GameState.PlayerAttack)
        {
            UnitManager.Instance.Player.SpawnSwingParticles();
        }
        if (GameManager.Instance.State == GameManager.GameState.EnemyAttack)
        {
            UnitManager.Instance.Enemy.SpawnEnemySwingParticles();
        }

        _unitAnimator.SetBool(IsIdle, false);
        _unitAnimator.SetBool(IsAttacking, true);

    }

    public void stopAttacking()
    {
        _unitAnimator.SetBool(IsIdle, true);
        _unitAnimator.SetBool(IsAttacking, false);

        if (GameManager.Instance.State == GameManager.GameState.PlayerAttack)
        {
            UnitManager.Instance.Player.swingParticlesInstance.Stop();

            if (UnitManager.Instance.Player.swingParticlesInstance != null)
            {
                Destroy(UnitManager.Instance.Player.swingParticlesInstance.gameObject);
            }
        }

        if (GameManager.Instance.State == GameManager.GameState.EnemyAttack)
        {
            UnitManager.Instance.Enemy.enemySwingParticlesInstance.Stop();
            if (UnitManager.Instance.Enemy.enemySwingParticlesInstance != null)
            {
                Destroy(UnitManager.Instance.Enemy.enemySwingParticlesInstance.gameObject);
            }
        }
    }

    public void startDamaging()
    {
        _unitAnimator.SetBool(IsIdle, false);
        _unitAnimator.SetBool(IsDamaged, true);

        if (GameManager.Instance.State == GameManager.GameState.PlayerAttack)
        {
            UnitManager.Instance.Enemy.SpawnEnemyHitParticles();
        }
        if (GameManager.Instance.State == GameManager.GameState.EnemyAttack)
        {

            if (StatManager.Instance._currentRound == 3)
            {
                UnitManager.Instance.Player.SpawnHitByBossParticles();
            }
            else 
            UnitManager.Instance.Player.SpawnHitParticles();
        }
    }

    public void stopDamaging()
    {
        _unitAnimator.SetBool(IsIdle, true);
        _unitAnimator.SetBool(IsDamaged, false);

    }

    private void Update()
    {
        if (_healthbar != null) _healthbar.value = (float)_currentHealth/(float)_maxHealth;
    }
}
