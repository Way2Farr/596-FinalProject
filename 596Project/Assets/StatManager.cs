using Unity.VisualScripting;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance;
    public int _currentRound = 1;

    [SerializeField]
    public int _maxHealth, _attack, _defense, _movementRange, _attackRange = 0;
    public int _currentHealth;

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
            
            _maxHealth = _playerPrefab._maxHealth;
            _attack = _playerPrefab._attack;
            _defense = _playerPrefab._defense;
            _movementRange = _playerPrefab._movementRange;
            _attackRange = _playerPrefab._attackRange;

        }

    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    public void IncreaseStats (int health, int attack, int defense, int movementRange, int attackRange)
    {
        _maxHealth += health;
        _attack += attack;
        _defense += defense;
        _movementRange += movementRange;
        _attackRange += attackRange;
    }


}
