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

    #endregion

    #region Unity Messages

    // Get the player and initialise the Tank Attributes
    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
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
