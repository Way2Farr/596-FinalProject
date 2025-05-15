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

    public bool MenuShow = false;

    [SerializeField]
    AudioClip _healClip, _stunClip, _baneClip, _windedClip;
    void Start()
    {
     _currentHealth = _maxHealth;
     ManaPoints.SetText("MP: " + _manaPoint);
    }

    void Update()
    {
        ManaPoints.SetText("MP: " + _manaPoint);
        if (_healthbar != null) _healthbar.value = (float)_currentHealth / (float)_maxHealth;
    }
    //___________________________________________________________________________________\\
    // DAMAGE FUNCTION
    public virtual void OnHurt(int attackDamage) {

        int dmgTaken = Mathf.Max(attackDamage - (_defense / 3),0);

        _currentHealth -= dmgTaken;
        SpawnHitParticles();
        hitParticlesInstance.Play();


        if(DamageTextPrefab) {
            ShowDmgTxt(dmgTaken);
        }
        else {
            Debug.Log("Error at Player.ShowDmgtxt");
        }

        if(_currentHealth <= 0) {
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
        hitParticlesInstance.Stop();
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

        if( healthFlask > 0 && !UnitManager.Instance.hasPerformedAction) {
            _currentHealth = Mathf.Min(_currentHealth + healAmount, _maxHealth);
            healthFlask =- 1;
            CloseAbilitiesMenu();
            SpawnHealParticles();
            healthParticlesInstance.Play();
            UnitManager.Instance.hasPerformedAction = true;
            ShowHealTxt();

            SoundFXManager.Instance.PlayClip(_healClip, this.transform, 0.1f);
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
        //Destroy(gameObject);
        GameManager.Instance.UpdateGameState(GameManager.GameState.Lose);
        VictoryScreen.Instance.StartLossScreen();
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

        SoundFXManager.Instance.PlayClip(GameManager.Instance._menuEndSound, this.transform, 0.1f);

        Debug.Log("WHAT STATE AM I" + GameManager.Instance.State);
        Debug.Log("Abilities array length: " + _abilities.Length);
        if(state == GameManager.GameState.ChooseOption ){

            foreach(GameObject abilityPanel in _abilities)
            {
                abilityPanel.SetActive(true);
                MenuShow = true;
                Debug.Log("Opening Ability Panel!");
            } 
        }
        else
        {
            //Debug.Log("Deactivate menu.");
            foreach (GameObject abilityPanel in _abilities)
            {
                abilityPanel.SetActive(false);
                MenuShow = false;
            }
        }
    }
    public void CloseAbilitiesMenu()
{
    foreach (GameObject abilityPanel in _abilities)
    {
        abilityPanel.SetActive(false);
        MenuShow = false;
    }
    Debug.Log("Abilities menu closed.");
}


    public virtual void OpenMagic() {

            if(!UnitManager.Instance.hasPerformedAction){

            CloseAbilitiesMenu();

            foreach(GameObject magicPanel in _magic)
            {
                magicPanel.SetActive(true);
                MenuShow = true;

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
        MenuShow = false;
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
                if (!UnitManager.Instance.hasPerformedAction)
                {
                    Winded();
                    SoundFXManager.Instance.PlayClip(_windedClip, this.transform, 0.1f);
                }
                else
                {
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
                    SoundFXManager.Instance.PlayClip(_baneClip, this.transform, 0.1f);
                }
                else {
                MenuManager.Instance.EventMessages("You already performed an action!");
                return;
                }
            break;

            case 8:
                CloseAbilitiesMenu();
                CloseMagicMenu();
                if(!UnitManager.Instance.hasPerformedAction){
                GameManager.Instance.UpdateGameState(GameManager.GameState.Stun);
                SoundFXManager.Instance.PlayClip(_stunClip, this.transform, 0.1f);
                Stun();
                }
                else {
                MenuManager.Instance.EventMessages("You already performed an action!");
                return;
                }
            break;
        }
    }

 //___________________________________________________________________________________\\
// WINDED BUFF
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

        if(_manaPoint > 0 ){
            if(speedDuration <= 0) {
            speedDuration = 2;
            originalSpeed = _movementRange;
            _movementRange += speedBuff;
            _manaPoint--;

            UnitManager.Instance.hasPerformedAction = true;
            WindedIcon.SetActive(true);
            ManaPoints.text = "MP: " +_manaPoint; 
            SpawnWindedParticles();

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
                windedParticlesInstance.Stop();
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

        if(_manaPoint > 0) {
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
            enemy.InflictBane(3);
            ClearBaneOverlay();;
            _manaPoint--;
            ManaPoints.text = "MP: " + _manaPoint;
            UnitManager.Instance.hasPerformedAction = true;
            GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
            BaneIcon.SetActive(true);

            }
            else {
                ClearBaneOverlay();
                GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
                
            }

        }


 //___________________________________________________________________________________\\
// Stun BUFF

    public int _stunRange;
    public GameObject StunIcon;

    void Stun() {

        CloseAbilitiesMenu();

        if( UnitManager.Instance.hasPerformedAction) {
            MenuManager.Instance.EventMessages("You already performed an action!");
            return;
        }

        if(_manaPoint > 0) {
                ShowStunOverlay();
    
        }
        else {
            MenuManager.Instance.EventMessages("Not enough mana!");
            GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
        }
  
        
    }
    public virtual List<Tile> GetStunTiles()
    {
        float tempRange = this.GetStunRange();
      List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values
        .Where(t => Vector2.Distance(this.transform.position, t.transform.position)
         <= tempRange && t._position != OccupiedTile._position).ToList();

        return _inRangeTiles;
    }

    public int GetStunRange() 
    { 
        return _stunRange;
    }

    public void ShowStunOverlay()
    {

        ClearStunOverlay();
        UnitManager.Instance.SetSelectedHero(UnitManager.Instance.Player);
        foreach (Tile tile in GetStunTiles())
        {
            tile.RangeActive();
        }
        _inStunRange = true;
    }

    public void ClearStunOverlay()
    {
        foreach (Tile tile in GridManager.Instance._tilesList)
        {
            tile.RangeInactive();
        }
       
    }

    public bool _inStunRange;

    public void HandleStun(BaseEnemy enemy) {
            if(enemy != null && _inStunRange) {
            enemy.InflictStun(3);
            ClearStunOverlay();
            _manaPoint--;
            ManaPoints.text = "MP: " + _manaPoint;
            UnitManager.Instance.hasPerformedAction = true;
            GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
            StunIcon.SetActive(true);

            }
            else {
                ClearStunOverlay();
                GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
                
            }

        }

//______________________________________Winded
// Winded Particle Systems

[SerializeField] private ParticleSystem WindedParticles;
private ParticleSystem windedParticlesInstance;

private void SpawnWindedParticles() {
    windedParticlesInstance = Instantiate(WindedParticles, transform.position, Quaternion.identity, transform);
    windedParticlesInstance.transform.rotation = Quaternion.Euler(-20,0,0);
    windedParticlesInstance.Play();
}

//______________________________________ Hit

[SerializeField] private ParticleSystem HitParticles;
private ParticleSystem hitParticlesInstance;
private void SpawnHitParticles() {
    hitParticlesInstance = Instantiate(HitParticles, transform.position, Quaternion.identity, transform);
    hitParticlesInstance.transform.rotation = Quaternion.Euler(-90,0,0);
    hitParticlesInstance.Play();
}


//______________________________________Health


[SerializeField] private ParticleSystem HealthParticles;
private ParticleSystem healthParticlesInstance;

private void SpawnHealParticles() {
    Debug.Log("Spawned Heal!");
    healthParticlesInstance = Instantiate(HealthParticles, transform.position, Quaternion.identity, transform);
    healthParticlesInstance.transform.rotation = Quaternion.Euler(-90,0,0);
    healthParticlesInstance.Play();
}


//______________________________________ Swing

[SerializeField] private ParticleSystem SwingParticles;
public  ParticleSystem swingParticlesInstance;
public void SpawnSwingParticles() {
    swingParticlesInstance = Instantiate(SwingParticles, transform.position, Quaternion.identity, transform);
    swingParticlesInstance.transform.rotation = Quaternion.Euler(-90,0,0);
    swingParticlesInstance.Play();
}

}