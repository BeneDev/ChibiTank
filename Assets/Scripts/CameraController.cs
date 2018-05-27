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

    GameObject player;

    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSpeed = 3f;
    [SerializeField] float controllerRotationSpeed = 4f;

    bool cameOutOfFocussedMode = false;

    Vector3 playerAimDirectionForCamReset;
    GameObject focussedObject;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

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
}
