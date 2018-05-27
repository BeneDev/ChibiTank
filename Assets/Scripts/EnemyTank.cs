using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : BaseTank {

    GameObject player;
    Vector3 toPlayer;
    bool bAimingAtPlayer = false;
    bool isTimeWhenPlayerLeftSightSaved = false;
    float timeWhenPlayerLeftSight;

    [SerializeField] float lookForPlayerDuration = 3f;

    [SerializeField] float sightReach;
    [SerializeField] float aimingTolerance = 0.1f;

    [Header("Attributes"), SerializeField] float baseShootKnockback = 1f;
    [SerializeField] float baseShootKnockbackDuration = 1f;
    [SerializeField] float baseFireRate = 1f;
    [SerializeField] int baseAttack = 1;

    [SerializeField] int baseDefense = 1;
    [SerializeField] int baseHealth = 1;

    [SerializeField] float baseAcceleration = 1f;
    [SerializeField] float baseTopSpeed = 1f;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");

        fireRate = baseFireRate;
        shootKnockback = baseShootKnockback;
        shootKnockbackDuration = baseShootKnockbackDuration;
        acceleration = baseAcceleration;
        topSpeed = baseTopSpeed;
        attack = baseAttack;
        health = baseHealth;
        defense = baseDefense;
    }

    protected override void FixedUpdate()
    {
        AttackPlayerIfClose();
        Move();
        base.FixedUpdate();
    }

    private void Attack()
    {
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

    private void Move()
    {
        if(toPlayer.magnitude < sightReach)
        {
            // TODO move sidewards to evade player bullets
            isTimeWhenPlayerLeftSightSaved = false;
        }
        else
        {
            if(!isTimeWhenPlayerLeftSightSaved)
            {
                timeWhenPlayerLeftSight = Time.realtimeSinceStartup;
                isTimeWhenPlayerLeftSightSaved = true;
            }
            if(Time.realtimeSinceStartup < timeWhenPlayerLeftSight + lookForPlayerDuration)
            {
                moveDirection = aimDirection;
                CalculateVelocity();
            }
        }
    }

    bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < aimingTolerance;
    }

    void AttackPlayerIfClose()
    {
        toPlayer = player.transform.position - transform.position;
        if(toPlayer.magnitude < sightReach)
        {
            aimDirection = toPlayer;
            RotateTank();
            Attack();
        }
    }

}
