using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNPCTank : BaseTank {

    GameObject player;
    Vector3 toPlayer;
    [SerializeField] float attentionDistance;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rotationSpeed = 3f;
        moveDirection = Vector3.zero;
    }

    // Update is called once per frame
    void Update () {
        toPlayer = player.transform.position - transform.position;
		if(toPlayer.magnitude < attentionDistance)
        {
            aimDirection = -toPlayer;
            RotateTank();
        }
	}

    protected void OpenSaveDialogue()
    {
        SaveMenu.Show();
    }

    protected void Talk(string text)
    {
        // TODO open dialogue window with the text param shown
    }
}
