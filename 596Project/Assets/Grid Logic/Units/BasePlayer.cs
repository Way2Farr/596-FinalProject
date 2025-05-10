using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class BasePlayer : BaseUnit
{

    public GameObject DamageTextPrefab;

    private static readonly Vector3 damageOffsetPos = new Vector3(0,1,0);

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
            Debug.Log("Error with Player.Tmp_Text");
        }

        Destroy(damageTextPro, 1.5f);
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


    public TMP_Text abilitiesText;
    [SerializeField] private GameObject[] _abilities;

    
    public virtual void OpenAbilities(GameManager.GameState state) {

        Debug.Log("Current GameState is: " + GameManager.Instance.State);
        if(state == GameManager.GameState.ChooseAction ){
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
}
