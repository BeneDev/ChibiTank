using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : BaseTank {

    GameObject player;
    Vector3 toPlayer;
    bool bAimingAtPlayer = false;
    float timeWhenPlayerLeftSight;

    Vector3 pointWherePlayerLastSpotted;

    [SerializeField] float searchPlayerDuration = 3f;

    [SerializeField] float sightReach;
    [SerializeField] float aimingTolerance = 0.1f;

    // Attributes
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

    enum EnemyState
    {
        idle,
        playerSpotted,
        searchingForPlayer
    };
    EnemyState state = EnemyState.idle;

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
        if (state == EnemyState.idle)
        {
            if(toPlayer.magnitude < sightReach)
            {
                state = EnemyState.playerSpotted;
            }
        }
        else if(state == EnemyState.playerSpotted)
        {
            if(toPlayer.magnitude < sightReach + 5f)
            {
                Attack();
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
            MoveTo(pointWherePlayerLastSpotted);
            if(toPlayer.magnitude < sightReach)
            {
                state = EnemyState.playerSpotted;
            }
            if(Time.realtimeSinceStartup > timeWhenPlayerLeftSight + searchPlayerDuration)
            {
                state = EnemyState.idle;
            }
        }
        base.FixedUpdate();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        pointWherePlayerLastSpotted = player.transform.position;
        timeWhenPlayerLeftSight = Time.realtimeSinceStartup;
        state = EnemyState.searchingForPlayer;
    }

    private void Attack()
    {
        aimDirection = toPlayer;
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
        // TODO fix bug where the enemy tank rotates weirdly when going back into idle state
        // Move to the point
        Vector3 toPoint = point - transform.position;
        aimDirection = toPoint;
        moveDirection = toPoint;
        RotateBody();
        CalculateVelocity();
    }

    bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < aimingTolerance;
    }
}
