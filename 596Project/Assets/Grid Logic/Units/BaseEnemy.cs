using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;


public class BaseEnemy : BaseUnit
{
    public GameObject DamageTextPrefab;
    public GameObject BaneIcon;

    public bool _defeated = false;
    public GameObject StunIcon;
    
    private static readonly Vector3 damageOffsetPos = new Vector3(0,1,0);

    public virtual void OnHurt(int attackDamage) {

        int dmgTaken = Mathf.Max(attackDamage - (_defense / 3),0);

        _currentHealth -= dmgTaken;
        SpawnEnemyHitParticles();

        if (DamageTextPrefab)
        {
            ShowDmgTxt(dmgTaken);
        }
        else
        {
            Debug.Log("Error at ShowDmgtxt");
        }

        if(_currentHealth <= 0) {
            IsDead();
        } 
    }


  
    int enemyBaneDuration;
    int originalDefense;
    public virtual void InflictBane(int duration) {

        if (enemyBaneDuration <= duration) {
            enemyBaneDuration = 2;
            BaneIcon.SetActive(true);
            SpawnBaneParticles();
            originalDefense = _defense;
            _defense = 0;
            
        }
    }

    public void BaneDuration() {
        if(enemyBaneDuration > 0) {
            enemyBaneDuration--;
            if(enemyBaneDuration == 0) {
                _defense = originalDefense;
                BaneIcon.SetActive(false);
                 baneParticlesInstance.Stop();
                UnitManager.Instance.Player.BaneIcon.SetActive(false);
            }
        }
    }


//_________________________

    int enemyStunDuration;

    public bool isStunned = false;
    public virtual void InflictStun(int duration) {

        if (enemyStunDuration <= duration) {
            enemyStunDuration = duration;
            StunIcon.SetActive(true);
            SpawnStunParticles();
            isStunned = true;
        }
    }

    public void StunDuration() {
        if(enemyStunDuration > 0) {
            enemyStunDuration--;

            if(enemyStunDuration == 0) {
                isStunned = false;
                StunIcon.SetActive(false);
                stunParticlesInstance.Stop();
                UnitManager.Instance.Player.StunIcon.SetActive(false);
            }
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
            Debug.Log("Error with Tmp_Text");
        }

        Destroy(damageTextPro, 1.5f);
    } 

    void IsDead(){
        this._spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
        this._healthbar.gameObject.SetActive(false);
        _defeated = true;



        StartCoroutine(VictoryDelay(0.1f));
    }

    public IEnumerator VictoryDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        GameManager.Instance.UpdateGameState(GameManager.GameState.Victory);
    }

// Bane Particle Systems

[SerializeField] private ParticleSystem BaneParticles;
private ParticleSystem baneParticlesInstance;

private void SpawnBaneParticles() {
    baneParticlesInstance = Instantiate(BaneParticles, transform.position, Quaternion.identity, transform);
    baneParticlesInstance.Play();
    baneParticlesInstance.transform.rotation = Quaternion.Euler(-90,0,0);
}

//______________________________________Stun
// Stun Particle Systems

[SerializeField] private ParticleSystem StunParticles;
private ParticleSystem stunParticlesInstance;

    private void SpawnStunParticles()
    {
        stunParticlesInstance = Instantiate(StunParticles, transform.position, Quaternion.identity, transform);
        stunParticlesInstance.Play();
        stunParticlesInstance.transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    [SerializeField] private ParticleSystem enemyHitParticles;
    public ParticleSystem enemyHitParticlesInstance;
    public void SpawnEnemyHitParticles() {

        enemyHitParticlesInstance = Instantiate(enemyHitParticles, transform.position, Quaternion.identity, transform);
        enemyHitParticlesInstance.transform.rotation = Quaternion.Euler(-90,0,0);
        enemyHitParticlesInstance.Play();
}

    [SerializeField] private ParticleSystem EnemySwingParticles;
    public  ParticleSystem enemySwingParticlesInstance;
    public void SpawnEnemySwingParticles() {

        enemySwingParticlesInstance = Instantiate(EnemySwingParticles, transform.position, Quaternion.identity, transform);
        enemySwingParticlesInstance.Play();
        enemySwingParticlesInstance.transform.rotation = Quaternion.Euler(-90,0,0);
        
}

    //______________________________________Health

    // --------- HANDLE ENEMY BEHAVIOR --------------



    public bool PlayerInAttackRange()
    {
        //return true;

        List<Tile> _playerTiles = this.getAttackTiles().Where(t => t._position == UnitManager.Instance.Player.OccupiedTile._position).ToList();

        bool playerInRange = _playerTiles.Count > 0;

        return playerInRange;
    }



}