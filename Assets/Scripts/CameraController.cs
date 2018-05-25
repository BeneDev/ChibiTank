using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    #region Properties

    public Vector3 CamResetRotation
    {
        get
        {
            return playerAimDirectionForCamReset;
        }
        set
        {
            playerAimDirectionForCamReset = value;
        }
    }

    #endregion

    GameObject player;

    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSpeed = 3f;
    [SerializeField] float controllerRotationSpeed = 4f;
    
    Vector3 playerAimDirectionForCamReset;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position, speed);
        
        // Rotate based on the aim Direction, the player set the cam to be in
        if (GameManager.Instance.IsControllerInput && playerAimDirectionForCamReset != null)
        {
            transform.forward = Vector3.Lerp(transform.forward, playerAimDirectionForCamReset, controllerRotationSpeed * Time.deltaTime);
        }
        else if(playerAimDirectionForCamReset != null)
        {
            transform.forward = Vector3.Lerp(transform.forward, playerAimDirectionForCamReset, rotationSpeed * Time.deltaTime);
        }
    }
}
