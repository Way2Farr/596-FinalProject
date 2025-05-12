using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class BasePlayer : BaseUnit
{

    public GameObject DamageTextPrefab;
    public GameObject HealTextPrefab;

    [SerializeField]public int healthFlask = 1;
    [SerializeField]public int healAmount = 5;

    private static readonly Vector3 damageOffsetPos = new Vector3(0,1,0);
    private static readonly Vector3 healOffsetPos = new Vector3(0,1,0);


    void Start()
    {
     _currentHealth = _maxHealth;
    }

    public virtual void OnHurt(int attackDamage) {

        int dmgTaken = Mathf.Max(attackDamage - (_defense / 3),0);

        _currentHealth -= dmgTaken;

        if(DamageTextPrefab) {
            ShowDmgTxt(dmgTaken);
        }
        else {
            Debug.Log("Error at Player.ShowDmgtxt");
        }

        if(_maxHealth <= 0) {
            IsDead();
        } 
    }

    void ShowDmgTxt(int damage) {
        Vector3 offsetPos = transform.position + damageOffsetPos;
        GameObject damageTextPro = Instantiate(DamageTextPrefab, offsetPos, Quaternion.identity, transform);
        damageTextPro.transform.rotation = Quaternion.Euler(-90,0,0);
        TMP_Text textC = damageTextPro.GetComponentInChildren<TMP_Text>();
        
        if (textC != null) {
            textC.text = damage.ToString();
        }
        else {
            Debug.Log("Error with PlayerDMG");
        }

        Destroy(damageTextPro, 1.5f);
    } 

    void ShowHealTxt() {
            
            Vector3 offsetPos = transform.position + healOffsetPos;
            GameObject healTextPro = Instantiate(HealTextPrefab, offsetPos, Quaternion.identity, transform);
            healTextPro.transform.rotation = Quaternion.Euler(-90,0,0);
            TMP_Text textH = healTextPro.GetComponentInChildren<TMP_Text>();
            
            if (textH != null) {
                textH.text = "" + healAmount;
            }
            else {
                Debug.Log("Error with PlayerHeal");
            }

            Destroy(healTextPro, 1.5f);
        } 

    public void PlayerHeal() {

        Debug.Log("has attacked is " + UnitManager.Instance.hasAttacked);
        if( healthFlask > 0 && !UnitManager.Instance.hasAttacked) {
            _currentHealth = Mathf.Min(_currentHealth + healAmount, _maxHealth);
            healthFlask =- 1;
            CloseAbilitiesMenu();
            UnitManager.Instance.hasHealed = true;
            ShowHealTxt();

            if (UnitManager.Instance.hasMoved){
            UnitManager.Instance.TurnCheck();
            }
            else {
                return;
            }
            
        }
        else {
            if (UnitManager.Instance.hasHealed && healthFlask > 0){
            MenuManager.Instance.EventMessages("You healed this turn already!");
            }
            else if(healthFlask <= 0) {
                MenuManager.Instance.EventMessages("You're out of health flasks!");
            return;
            }
            CloseAbilitiesMenu();
            return;
        }
    }




    void IsDead(){
        Destroy(gameObject);
    }   

    // Rook behavior (can use later)
    /*public override List<Tile> getMovementTiles()
    {
        float tempRange = this.getMovementRange();
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => t._position.x == this.OccupiedTile._position.x || t._position.y == this.OccupiedTile._position.y).ToList();

        return _inRangeTiles;

        //return base.getMovementTiles();
    }
*/
    // Movement behavior
    public override List<Tile> getMovementTiles()
    {
        float tempRange = this.getMovementRange() / 10;
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t => Mathf.Abs(t._position.x - this.OccupiedTile._position.x) <= tempRange && Mathf.Abs(t._position.y - this.OccupiedTile._position.y) <= tempRange && !(t.Walkable)).ToList();

        return _inRangeTiles;
    }

    [SerializeField] private GameObject[] _abilities;

    
    public virtual void OpenAbilities(GameManager.GameState state) {

        if(state == GameManager.GameState.ChooseOption ){
            foreach(GameObject abilityPanel in _abilities)
            {
                abilityPanel.SetActive(true);
                
            } 
        }
        else
        {
            //Debug.Log("Deactivate menu.");
            foreach (GameObject abilityPanel in _abilities)
            {
                abilityPanel.SetActive(false);
            }
        }
    }
    public void CloseAbilitiesMenu()
{
    foreach (GameObject abilityPanel in _abilities)
    {
        abilityPanel.SetActive(false);
    }
    Debug.Log("Abilities menu closed.");
}

    public void ButtonSetState(int setState)
    {
        switch (setState)
        {
            case 3:
                GameManager.Instance.UpdateGameState(GameManager.GameState.PlayerAttack);
                break;
            case 4:
                GameManager.Instance.UpdateGameState(GameManager.GameState.Heal);
                break;
            case 5:
                GameManager.Instance.UpdateGameState(GameManager.GameState.Debuff);
                break;
        }
    }



}
