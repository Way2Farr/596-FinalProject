using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BossHard : BaseEnemy
{

    [SerializeField] int _visionRange;
    public int shieldHealth, _chargeRound = 0;

    public bool hasHealed, hasShielded, isShielded, hasCharged = false;
    //isShielded for player attack
    public void BossStages()
    {
        if (GameManager.Instance.State == GameManager.GameState.EnemyChoose)
        {
            //Hidden Tile set for vision, if they're x amount of tiles away pursue, if they're in range choose an attack
            CheckVision();
            var player = UnitManager.Instance.Player;
            var attackTiles = getAttackTiles();

            if (!attackTiles.Any(t => t.OccupiedUnit == player)) // Get Move towards player
            {
                Debug.Log("Boss Moving Towards Player!");

                var movementTiles = getMovementTiles();
                Tile playerTile = player.OccupiedTile;
                Tile chosenTile = movementTiles.
                OrderBy(t => Vector2.Distance(t.transform.position, playerTile.transform.position))
                .FirstOrDefault();

                if (chosenTile != null)
                {
                    UnitManager.Instance._startingTile = this.OccupiedTile;
                    UnitManager.Instance._endTile = chosenTile;
                    UnitManager.Instance._startMoving = true;

                    GameManager.Instance.UpdateGameState(GameManager.GameState.EnemyMove);
                }
            }

            else if (attackTiles.Any(t => t.OccupiedUnit == player))
            {
                Debug.Log("Boss is deciding on a stage!");
                //Check stage conditions, if none apply beat em up
                if (hasHealed == false && (_currentHealth <= (_maxHealth * 0.75f))) // If they haven't healed, and reach 75% of their health drink a potion
                {
                    BossHeal();
                }
                if (hasShielded == false && (_currentHealth <= (_maxHealth * 0.50f)))
                // If they haven't healed, and reach 50% raise a shield. Shield should tank 1 hit
                {
                    BossShield();
                }

                if (hasCharged == false && (_currentHealth <= (_maxHealth * 0.25f))) // blow up at 25% health
                {
                    BossChargeUp();
                }

                GameManager.Instance.UpdateGameState(GameManager.GameState.EnemyAttack);
            }
            GameManager.Instance.UpdateGameState(GameManager.GameState.ChooseOption);
        }
    }

    //_____________________________________Healed
    void BossHeal() // heal that mf
    {
        hasHealed = true;
        _currentHealth += (int)(_maxHealth * 0.1);
        Debug.Log("BossHealed");
    }

    //_____________________________________Shield
    void BossShield() // raise an instance of shield, this is an excuse for me to use that particle system
    {
        SpawnShieldParticle();
        shieldHealth = UnitManager.Instance.Player._attack;
        isShielded = true;
        Debug.Log("BossShielded");

    }

    void OnShieldBroken()
    {
        InflictStun(2);
        isShielded = false;
    }

    [SerializeField] private ParticleSystem ShieldParticle;
    public ParticleSystem shieldParticleInstance;
    public void SpawnShieldParticle()
    {

        shieldParticleInstance = Instantiate(ShieldParticle, transform.position, Quaternion.identity, transform);
        shieldParticleInstance.transform.rotation = Quaternion.Euler(-90, 0, 0);
        shieldParticleInstance.Play();
    }

    //_____________________________________Charge
    void BossChargeUp()
    {
        hasCharged = true;
        _chargeRound = 2;
        SpawnChargeUpParticles();
        Debug.Log("Boss is gonna explode!");
    }

        public void ChargeDuration() {
        if(_chargeRound > 0) {
            _chargeRound--;

            if (_chargeRound == 0)
            {
                hasCharged = false;
                chargeParticleInstance.Stop();
                OnChargeHit();
            }
        }
    }

    public void OnChargeHit()
    {
        int dmgSent = Mathf.Max(_attack * (_defense / 3), 100);
        UnitManager.Instance.Player.OnHurt(dmgSent);
    }

    [SerializeField] private ParticleSystem ChargeParticle;
    public ParticleSystem chargeParticleInstance;
    public void SpawnChargeUpParticles()
    {

        chargeParticleInstance = Instantiate(ChargeParticle, transform.position, Quaternion.identity, transform);
        chargeParticleInstance.transform.rotation = Quaternion.Euler(-90, 0, 0);
        chargeParticleInstance.Play();
    }


    // Update is called once per frame
    private void Update()
    {
        if (_healthbar != null) _healthbar.value = (float)_currentHealth / (float)_maxHealth;
    }

    public override List<Tile> getMovementTiles()
    {
        float tempRange = this.getMovementRange();
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(t =>
            t._position.x == this.OccupiedTile._position.x ||
            t._position.y == this.OccupiedTile._position.y ||
            Mathf.Abs(t._position.x - this.OccupiedTile._position.x) == Mathf.Abs(t._position.y - this.OccupiedTile._position.y)


            ).ToList();

        return _inRangeTiles;
    }

    public override List<Tile> getAttackTiles()
    {

        float tempRange = this.getAttackRange();
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(
            t => Vector2.Distance(this.transform.position, t.transform.position) <= tempRange && t._position !=
            OccupiedTile._position).ToList();

        return _inRangeTiles;

    }

    public List<Tile> CheckVision() //Check if a player is within range
    {
        float tempRange = this.getVisionRange();
        List<Tile> _inRangeTiles = GridManager.Instance._tiles.Values.Where(
            t => Vector2.Distance(this.transform.position, t.transform.position) <= tempRange && t._position !=
            OccupiedTile._position).ToList();

        return _inRangeTiles;
    }

    public int getVisionRange()
    {
        return _visionRange;
    }



    public virtual void BossOnHurt(int attackDamage) {


            int dmgTaken = Mathf.Max(attackDamage - (_defense / 3), 0);

        if (isShielded)
        {
            shieldHealth -= dmgTaken;

            if (shieldHealth <= 0)
            {
                OnShieldBroken();
            }
        }

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

            if (_currentHealth <= 0)
            {
                IsDead();
            
        }
    }
}
