using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : BaseTank {

    GameObject player;
    Vector3 toPlayer;
    [SerializeField] float sightReach;

    [Header("Attributes"), SerializeField] float baseFireRate = 1f;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
        fireRate = baseFireRate;
    }

    private void Update()
    {
        AttackPlayerIfClose();
        if (Time.realtimeSinceStartup > shootTime + fireRate)
        {
            Shoot();
        }
    }

    void AttackPlayerIfClose()
    {
        toPlayer = player.transform.position - transform.position;
        if(toPlayer.magnitude < sightReach)
        {
            aimDirection = toPlayer;
            RotateTank();
        }
    }
}
