using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : BaseTank {

    GameObject player;
    Vector3 toPlayer;
    bool bAimingAtPlayer = false;
    [SerializeField] float sightReach;
    [SerializeField] float aimingTolerance = 0.1f;

    [Header("Attributes"), SerializeField] float baseShootKnockback = 1f;
    [SerializeField] float baseShootKnockbackDuration = 1f;
    [SerializeField] float baseFireRate = 1f;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
        fireRate = baseFireRate;
        shootKnockback = baseShootKnockback;
        shootKnockbackDuration = baseShootKnockbackDuration;
    }

    protected override void FixedUpdate()
    {
        AttackPlayerIfClose();
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
