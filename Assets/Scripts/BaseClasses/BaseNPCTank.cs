using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNPCTank : BaseTank {

    GameObject player;
    Vector3 toPlayer;
    [SerializeField] float attentionDistance;

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

        moveDirection = Vector3.zero;
    }

    public override void TakeDamage(int damage)
    {
        // NPCs should not take damage, as they should not be able to die
    }

    protected override void FixedUpdate()
    {
        toPlayer = player.transform.position - transform.position;
        if (toPlayer.magnitude < attentionDistance)
        {
            aimDirection = toPlayer;
            RotateTurret();
        }
        base.FixedUpdate();
    }

    protected void OpenSaveMenu()
    {
        SaveMenu.Show();
    }

    protected void OpenDialogue(string text)
    {
        // TODO open dialogue window with the text param shown
    }
}
