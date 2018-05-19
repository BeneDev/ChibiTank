using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {

    #region Properties



    #endregion

    #region Private Fields

    PlayerInput input;

    [Header("Movement"), SerializeField] float acceleration = 1f;
    [SerializeField] float topSpeed = 3f;
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] float cockPitRotationSpeed = 2f;
    Vector3 direction;
    float velocity;

    Vector3 aimDirection;

    [Header("Physics"), SerializeField] float drag = 1f;
    [SerializeField] float shootKnockback = 1f;

    [Header("Tank Components"), SerializeField] GameObject cockPit;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        GetInput();
        RotatePlayer();
        CalculateVelocity();
        if(input.Shoot)
        {
            Shoot();
        }
        transform.position += transform.forward * velocity * Time.fixedDeltaTime;
    }

    #endregion

    #region Private Methods

    void GetInput()
    {
        direction.x = input.Horizontal;
        direction.z = input.Vertical;

        aimDirection.x = -input.R_Horizontal;
        aimDirection.z = input.R_Vertical;
    }

    void RotatePlayer()
    {
        // Rotate the player smoothly, depending on the velocity
        if (input.Horizontal != 0 || input.Vertical != 0)
        {
            Quaternion targetRotation = new Quaternion();
            targetRotation.SetLookRotation(direction);

            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        // Rotate the guns on the ship depending on the input of the right stick
        if (input.R_Horizontal != 0 || input.R_Vertical != 0 && cockPit)
        {
            //gunObject.transform.forward = shootDirection;
            Quaternion targetRotation = new Quaternion();
            targetRotation.SetLookRotation(aimDirection);

            cockPit.transform.rotation = Quaternion.Lerp(cockPit.transform.rotation, targetRotation, cockPitRotationSpeed * Time.fixedDeltaTime);
        }
    }

    void CalculateVelocity()
    {
        if(velocity < topSpeed)
        {
            velocity += acceleration * direction.magnitude;
        }
        velocity = velocity * (1 - Time.fixedDeltaTime * drag);
    }

    void Shoot()
    {
        // Apply knockback in the -aimDirection
    }

    #endregion
}
