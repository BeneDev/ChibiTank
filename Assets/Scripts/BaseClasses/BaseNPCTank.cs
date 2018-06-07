using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script every NPC Tank inherits from, as this makes the basic Attributes of any Tank serializable in the inspector. Also this Script makes sure, that NPCs do not die and are interactable for the player.
/// </summary>
public class BaseNPCTank : BaseTank {

    #region Fields

    GameObject player;
    Vector3 toPlayer; // The Vector to the player to calculate the turret direction, making the NPC look at the player
    [SerializeField] float attentionDistance; // The distance in which this NPC will spot the player

    // TODO make an array of sentences only for greetings and then pick one out of that array randomly everytime for greeting the player
    [Header("Dialogue"), SerializeField] protected string[] sentencesToTalk; // The sentences, the NPC will talk when the player talks to him
    // Special Assignments: 
    // "SAVE" -> Open the Save menu
    // ...

    // The serialize fields to initialise the common Attributes any Tank has
    [Header("Offensive Attributes"), SerializeField] int baseAttack = 1;
    [SerializeField] float basefireRate = 1f;
    [SerializeField] float baseReloadSpeed = 1f;
    [SerializeField] int baseMagazineSize = 5;
    [SerializeField] float baseShootKnockback = 1f;
    [SerializeField] float baseShootKnockbackDuration = 1f;

    [Header("Defensive Attributes"), SerializeField] int baseHealth = 1;
    [SerializeField] int baseDefense = 1;

    [Header("Agility Attributes"), SerializeField] float baseTopSpeed = 1f;
    [SerializeField] float baseAcceleration = 1f;
    [SerializeField] float baseRotationSpeed = 1f;

    [Header("Overall Attributes"), SerializeField] float baseMass = 1f;

    #endregion

    #region Unity Messages

    // Get the player and initialise the Tank Attributes
    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
        attack = baseAttack;
        fireRate = basefireRate;
        reloadSpeed = baseReloadSpeed;
        magazineSize = baseMagazineSize;
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

    // Make the NPC look towards the player, when he enters the attention zone of the NPC
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

    #endregion

    #region Helper Methods

    // Override the TakeDamage function of BaseTank to make the NPCs take no damage, making sure they cant die
    public override void TakeDamage(int damage)
    {
        // NPCs should not take damage, as they should not be able to die
    }

    // This function opens the dialogue window with all the texts in the sentences to talk field
    protected void OpenDialogue(string[] text)
    {
        DialogueMenu dialogueMenu = DialogueMenu.Show();
        dialogueMenu.SetDialogueActions(text);
    }

    #endregion

}
