using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IInput {

    bool bShootInUse = false;
    bool bCancelInUse = false;
    bool bResetCamInUse = false;

    // The input for horizontal movement
    public float Horizontal
    {
        get
        {
            return Input.GetAxis("Horizontal");
        }
    }

    // The input for vertical movement
    public float Vertical
    {
        get
        {
            return Input.GetAxis("Vertical");
        }
    }

    // The input for horizontal aiming
    public float R_Horizontal
    {
        get
        {
            return Input.GetAxis("RHorizontal");
        }
    }

    // The input for vertical aiming
    public float R_Vertical
    {
        get
        {
            return Input.GetAxis("RVertical");
        }
    }

    public bool Shoot
    {
        get
        {
            if (Input.GetAxisRaw("Shoot") != 0)
            {
                if (bShootInUse == false)
                {
                    bShootInUse = true;
                    return true;
                }
            }
            if (Input.GetAxisRaw("Shoot") == 0)
            {
                bShootInUse = false;
            }
            return false;
        }
    }

    public bool ResetCam
    {
        get
        {
            if (Input.GetAxisRaw("ResetCam") != 0)
            {
                if (bResetCamInUse == false)
                {
                    bResetCamInUse = true;
                    return true;
                }
            }
            if (Input.GetAxisRaw("ResetCam") == 0)
            {
                bResetCamInUse = false;
            }
            return false;
        }
    }

    public bool Cancel
    {
        get
        {
            if (Input.GetAxisRaw("Cancel") != 0)
            {
                if (bCancelInUse == false)
                {
                    bCancelInUse = true;
                    return true;
                }
            }
            if (Input.GetAxisRaw("Cancel") == 0)
            {
                bCancelInUse = false;
            }
            return false;
        }
    }
}
