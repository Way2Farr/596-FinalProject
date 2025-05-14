using Unity.VisualScripting;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance;
    public int _currentRound = 1;

    [SerializeField]
    public int _maxHealth, _attack, _defense, _movementRange, _attackRange,_manaPoint = 0;
    public int _currentHealth;
    public int _origMana;

    [SerializeField]
    public bool _inBattlePhase, _inShopPhase;

    [SerializeField]
    Player1 _playerPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        _inBattlePhase = GameObject.Find("GameManager") != null;
        _inShopPhase = !(_inBattlePhase);

        Debug.Log(_inBattlePhase);
        // instantiate stats if first round
        if (_inBattlePhase && _currentRound == 1)
        {
            
            _maxHealth = UnitManager.Instance.Player._maxHealth;
            _attack = UnitManager.Instance.Player._attack;
            _defense = UnitManager.Instance.Player._defense;
            _movementRange = UnitManager.Instance.Player._movementRange;
            _attackRange = UnitManager.Instance.Player._attackRange;
            _manaPoint = UnitManager.Instance.Player._manaPoint;

        }

    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    public void IncreaseStats (int health, int attack, int defense, int movementRange, int attackRange, int manaPoint)
    {
        _currentRound++;
        _maxHealth = health;
        _attack = attack;
        _defense = defense;
        _movementRange = movementRange;
        _attackRange = attackRange;
        _manaPoint = manaPoint;
    }


}
