using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : BaseTank {

    #region Fields

    GameObject player;
    Vector3 toPlayer;
    bool bAimingAtPlayer = false; // Used to check if the enemy aims rightfully at the player, to check if shooting would make sense
    float timeWhenPlayerLeftSight; // The time, the player left the enemy sight

    [SerializeField] float patrolRadius = 10f; // The radius, the enemy will search a new position to walk towards in
    [Range(0, 1), SerializeField] float patrolSpeedMultiplier = 0.3f; // The speed the enemy walks when only patroling

    Vector3 targetPosition; // The position the enemy will walk towards
    bool hasTarget = false; // stores if the enemy has to find a new target position

    Vector3 pointWherePlayerLastSpotted; // The position the player was last spotted to be in the sight of the enemy

    [SerializeField] float searchPlayerDuration = 3f; // How long the enemy will search for the player after he lost sight

    [SerializeField] float sightReach; // How far the enemy can see the player
    [SerializeField] float aimingTolerance = 0.1f; // The difference between the actual aiming rotation and where the player is, to check if shooting makes sense

    // Attributes for any enemy tank
    [Header("Offensive Attributes"), SerializeField] int baseAttack = 1;
    [SerializeField] float basefireRate = 1f;
    [SerializeField] float baseReloadSpeed = 1f;
    [SerializeField] float baseShootKnockback = 1f;
    [SerializeField] float baseShootKnockbackDuration = 1f;

    [Header("Defensive Attributes"), SerializeField] int baseHealth = 1;
    [SerializeField] int baseDefense = 1;

    [Header("Agility Attributes"), SerializeField] float baseTopSpeed = 1f;
    [SerializeField] float baseAcceleration = 1f;
    [SerializeField] float baseRotationSpeed = 1f;

    [Header("Overall Attributes"), SerializeField] float baseMass = 1f;

    // The possible states and the field to store the current state for the enemy AI State Machine
    enum EnemyState
    {
        patroling,
        playerSpotted,
        searchingForPlayer
    };
    EnemyState state = EnemyState.patroling;

    #endregion

    #region Unity Messages

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");

        attack = baseAttack;
        fireRate = basefireRate;
        reloadSpeed = baseReloadSpeed;
        shootKnockback = baseShootKnockback;
        shootKnockbackDuration = baseShootKnockbackDuration;

        health = baseHealth;
        defense = baseDefense;

        topSpeed = baseTopSpeed;
        acceleration = baseAcceleration;
        rotationSpeed = baseRotationSpeed;

        mass = baseMass;
    }

    protected override void FixedUpdate()
    {
        toPlayer = player.transform.position - transform.position;
        if(isDead)
        {
            // TODO Let the player control this value in options menu
            if(toPlayer.magnitude > 150f)
            {
                Destroy(gameObject);
            }
            return;
        }
        if (state == EnemyState.patroling)
        {
            acceleration = baseAcceleration * patrolSpeedMultiplier;
            if (toPlayer.magnitude < sightReach && !player.GetComponent<PlayerController>().IsDead)
            {
                state = EnemyState.playerSpotted;
            }
            if(!hasTarget)
            {
                Vector2 relativePoint = Random.insideUnitCircle * patrolRadius;
                targetPosition = transform.position + new Vector3(relativePoint.x, 0f, relativePoint.y);
                hasTarget = true;
            }
            MoveTo(targetPosition);
            if(V3Equal(transform.position, targetPosition))
            {
                hasTarget = false;
            }
        }
        else if(state == EnemyState.playerSpotted)
        {
            acceleration = baseAcceleration;
            if(player.GetComponent<PlayerController>().IsDead)
            {
                state = EnemyState.patroling;
            }
            if(toPlayer.magnitude < sightReach - 5f)
            {
                Attack();
            }
            else if(toPlayer.magnitude < sightReach + 5f)
            {
                MoveTo(player.transform.position);
            }
            else
            {
                state = EnemyState.searchingForPlayer;
                pointWherePlayerLastSpotted = player.transform.position;
                timeWhenPlayerLeftSight = Time.realtimeSinceStartup;
            }
        }
        else if(state == EnemyState.searchingForPlayer)
        {
            acceleration = baseAcceleration;
            MoveTo(pointWherePlayerLastSpotted);
            if(toPlayer.magnitude < sightReach && !player.GetComponent<PlayerController>().IsDead)
            {
                state = EnemyState.playerSpotted;
            }
            if(Time.realtimeSinceStartup > timeWhenPlayerLeftSight + searchPlayerDuration)
            {
                state = EnemyState.patroling;
            }
        }
        base.FixedUpdate();
    }

    #endregion

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        pointWherePlayerLastSpotted = player.transform.position;
        timeWhenPlayerLeftSight = Time.realtimeSinceStartup;
        state = EnemyState.searchingForPlayer;
    }

    private void Attack()
    {
        aimDirection = toPlayer.normalized;
        RotateTurret();
        if (V3Equal(cockPit.transform.forward.normalized, aimDirection.normalized))
        {
            bAimingAtPlayer = true;
        }
        else
        {
            bAimingAtPlayer = false;
        }
        if (Time.realtimeSinceStartup > shootTime + fireRate && bAimingAtPlayer)
        {
            Shoot();
        }
    }

    void MoveTo(Vector3 point)
    {
        // Move to the point
        Vector3 toPoint = point - transform.position;
        aimDirection = toPoint.normalized;
        moveDirection = toPoint.normalized;
        RotateBody();
        CalculateVelocity();
    }

    bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < aimingTolerance;
    }
}
