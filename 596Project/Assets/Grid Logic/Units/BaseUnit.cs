using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public Tile OccupiedTile;
    public Faction Faction;

    [SerializeField]
    private int _maxHealth, _attack, _defense, _movementRange, _attackRange;
    private int _currentHealth;

    [SerializeField]
    private string _name;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

}
