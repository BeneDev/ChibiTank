using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : BaseTank {

    GameObject player;
    Vector3 toPlayer;
    bool bAimingAtPlayer = false;
    float timeWhenPlayerLeftSight;

    [SerializeField] float patrolRadius = 10f;
    
    Vector3 initialPos;

    Vector3 targetPosition;
    bool hasTarget = false;

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
        patroling,
        playerSpotted,
        searchingForPlayer
    };
    EnemyState state = EnemyState.patroling;

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

        initialPos = transform.position;
    }

    protected override void FixedUpdate()
    {
        toPlayer = player.transform.position - transform.position;
        if (state == EnemyState.patroling)
        {
            acceleration = baseAcceleration / 2;
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
