using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script which lets the player interact with the NPC
/// </summary>
public class NPCTank : BaseNPCTank {

    // This script gets called by the player. Call the desired methods in here
	public void Interact()
    {
        OpenDialogue(sentencesToTalk);
    }

}
