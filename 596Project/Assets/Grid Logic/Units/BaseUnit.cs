using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public Tile OccupiedTile;
    public Faction Faction;

    [SerializeField]
    private int _maxHealth, _attack, _defense, _movement;
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
}
