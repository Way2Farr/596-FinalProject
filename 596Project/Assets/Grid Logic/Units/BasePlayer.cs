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
     ManaPoints.text = "MP: " + manaPoint; 
    }

    //___________________________________________________________________________________\\
    // DAMAGE FUNCTION
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
 //___________________________________________________________________________________\\
 // HEAL FUNCTION
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

        Debug.Log("has attacked is " + UnitManager.Instance.hasPerformedAction);
        if( healthFlask > 0 && !UnitManager.Instance.hasPerformedAction) {
            _currentHealth = Mathf.Min(_currentHealth + healAmount, _maxHealth);
            healthFlask =- 1;
            CloseAbilitiesMenu();
            UnitManager.Instance.hasPerformedAction = true;
            ShowHealTxt();

            if (UnitManager.Instance.hasMoved){
            UnitManager.Instance.TurnCheck();
            }
            else {
                return;
            }
            
        }
        else {
            if (UnitManager.Instance.hasPerformedAction && healthFlask > 0){
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

 //___________________________________________________________________________________\\
// PLAYER UI METHODS
    [SerializeField] private GameObject[] _abilities;
    [SerializeField] private GameObject[] _magic;


    
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


    public virtual void OpenMagic() {

            if(!UnitManager.Instance.hasPerformedAction){

            CloseAbilitiesMenu();

            foreach(GameObject magicPanel in _magic)
            {
                magicPanel.SetActive(true);
                
            } 
        }
        else {
            MenuManager.Instance.EventMessages("You already performed an action!");
                return;  
        }
    }

    public void CloseMagicMenu()
{
    foreach (GameObject magicPanel in _magic)
    {
        magicPanel.SetActive(false);
    }
    Debug.Log("Magic menu closed.");
}



    public void ButtonSetState(int setState)
    {
        switch (setState)
        {
            case 3:
                CloseAbilitiesMenu();
                GameManager.Instance.UpdateGameState(GameManager.GameState.PlayerAttack);
                break;
            case 6:
                CloseAbilitiesMenu();
                CloseMagicMenu();
                if(!UnitManager.Instance.hasPerformedAction)
                Winded();
                else {
                MenuManager.Instance.EventMessages("You already performed an action!");
                return;  
                }
            break;

            case 7:
                CloseAbilitiesMenu();
                CloseMagicMenu();
                if(!UnitManager.Instance.hasPerformedAction){
                GameManager.Instance.UpdateGameState(GameManager.GameState.Bane);
                Bane();
                }
                else {
                MenuManager.Instance.EventMessages("You already performed an action!");
                return;
                }
            break;

            case 8:
                CloseAbilitiesMenu();
                CloseMagicMenu();
                Stun();
            break;
        }
    }

 //___________________________________________________________________________________\\
// WINDED BUFF
    int manaPoint = 3;
    int speedBuff = 20;

    int speedDuration;    
    int originalSpeed;

    public GameObject WindedIcon;

    public TMP_Text ManaPoints;
    void Winded() {
        
        CloseAbilitiesMenu();

        if(speedDuration > 0) {
            MenuManager.Instance.EventMessages("Winded is still active!");
            return;
        }

        if( UnitManager.Instance.hasPerformedAction) {
            MenuManager.Instance.EventMessages("You already performed an action!");
            return;
        }

        if(manaPoint > 0 ){
            if(speedDuration <= 0) {
            speedDuration = 2;
            originalSpeed = _movementRange;
            _movementRange += speedBuff;
            manaPoint--;

            UnitManager.Instance.hasPerformedAction = true;
            WindedIcon.SetActive(true);
            ManaPoints.text = "MP: " + manaPoint; 

            }
        }
        else {
            MenuManager.Instance.EventMessages("You don't have enough mana!");
        
        }
        GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
    }

    public void WindedDuration() {
        if(speedDuration > 0) {
            speedDuration--;
            if(speedDuration == 0) {
                _movementRange = originalSpeed;
                WindedIcon.SetActive(false);
                MenuManager.Instance.EventMessages("Winded has ended!");
                GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
                
            }
        }
    }

 //___________________________________________________________________________________\\

    public int _baneRange;
    public GameObject BaneIcon;

    void Bane() {

        CloseAbilitiesMenu();

        if( UnitManager.Instance.hasPerformedAction) {
            MenuManager.Instance.EventMessages("You already performed an action!");
            return;
        }

        if(manaPoint > 0) {
                ShowBaneOverlay();
        }
        else {
            MenuManager.Instance.EventMessages("Not enough mana!");
            GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
        }
  
        
    }
    public virtual List<Tile> GetBaneTiles()
    {
        float tempRange = this.GetBaneRange();
      List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values
        .Where(t => Vector2.Distance(this.transform.position, t.transform.position)
         <= tempRange && t._position != OccupiedTile._position).ToList();

        return _inRangeTiles;
    }

    public int GetBaneRange() 
    { 
        return _baneRange;
    }

    public void ShowBaneOverlay()
    {

        ClearBaneOverlay();
        UnitManager.Instance.SetSelectedHero(UnitManager.Instance.Player);
        foreach (Tile tile in GetBaneTiles())
        {
            tile.RangeActive();
        }
        _inBaneRange = true;
    }

    public void ClearBaneOverlay()
    {
        foreach (Tile tile in GridManager.Instance._tilesList)
        {
            tile.RangeInactive();
        }
       
    }

    public bool _inBaneRange;

    public void HandleBane(BaseEnemy enemy) {
            if(enemy != null && _inBaneRange) {
            enemy.InflictBane(2);
            ClearBaneOverlay();
            manaPoint--;
            ManaPoints.text = "MP: " + manaPoint;
            UnitManager.Instance.hasPerformedAction = true;
            GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);

            }
            else {
                ClearBaneOverlay();
                GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
                
            }

        }


 //___________________________________________________________________________________\\

    void Stun(){

    }


    
}
