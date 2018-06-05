using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script which controls the camera arm and ultimately the camera
/// </summary>
public class CameraController : MonoBehaviour {

    #region Properties

    // The rotation the cam is reset to when the player chooses to reangle the camera
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

    // The object, the camera faces all the time, when in focus mode
    public GameObject FocussedObject
    {
        get
        {
            return focussedObject;
        }
        set
        {
            if (value != null)
            {
                focussedObject = value;
                playerAimDirectionForCamReset = transform.position - value.transform.position;
            }
            else
            {
                focussedObject = null;
            }
        }
    }

    #endregion

    #region Fields

    GameObject player;

    [SerializeField] float speed = 5f; // The speed, the camera follows the player
    [SerializeField] float rotationSpeed = 3f; // The speed, the camera rotates with when the player uses Keyboard and mouse
    [SerializeField] float controllerRotationSpeed = 4f; // The speed, the camera rotates with when player uses a controller

    bool cameOutOfFocussedMode = false; // Stores if the camera just came out of focus mode, to reset the cam to the last rotation it was in, in focus mode. 
    // Prevents fast camera flicking after focus mode

    Vector3 playerAimDirectionForCamReset; // The player aim direction, to set the camera to the player direction
    GameObject focussedObject; // The object, the camera will focus on

    #endregion

    #region Unity Messages

    // Get the player reference
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Follows the player and rotates the camera accodingly
    private void Update()
    {
        // Follow the player position
        transform.position = Vector3.Lerp(transform.position, player.transform.position, speed);
        if(focussedObject)
        {
            FocusObject();
            cameOutOfFocussedMode = true;
        }
        else
        {
            if(cameOutOfFocussedMode)
            {
                playerAimDirectionForCamReset = transform.forward;
                cameOutOfFocussedMode = false;
            }
            RotateCamera();
        }
    }

    #endregion

    #region Helper Methods

    // Turns off the camera animator. Used to enable screenshake, even though the camera is animated
    public void TurnOffAnimator()
    {
        Camera.main.GetComponentInParent<Animator>().enabled = false;
    }

    // This will update the camera rotation when in focus mode
    private void FocusObject()
    {
        Vector3 faceDirection = focussedObject.transform.position - transform.position;
        // Rotate based to always face the focussed object
        if (GameManager.Instance.IsControllerInput && playerAimDirectionForCamReset != null)
        {
            transform.forward = Vector3.Lerp(transform.forward, faceDirection, controllerRotationSpeed * Time.deltaTime);
        }
        else if (playerAimDirectionForCamReset != null)
        {
            transform.forward = Vector3.Lerp(transform.forward, faceDirection, rotationSpeed * Time.deltaTime);
        }
    }

    //Rotates the camera
    private void RotateCamera()
    {
        // Rotate based on the aim Direction, the player set the cam to be in
        if (GameManager.Instance.IsControllerInput && playerAimDirectionForCamReset != null)
        {
            transform.forward = Vector3.Lerp(transform.forward, playerAimDirectionForCamReset, controllerRotationSpeed * Time.deltaTime);
        }
        else if (playerAimDirectionForCamReset != null)
        {
            transform.forward = Vector3.Lerp(transform.forward, playerAimDirectionForCamReset, rotationSpeed * Time.deltaTime);
        }
    }

    #endregion

}
