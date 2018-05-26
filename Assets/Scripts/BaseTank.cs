using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script from which everything, controlling a tank inherits, to gain access to methods and variables, making it easy to controll and configure the tank
/// </summary>
public class BaseTank : MonoBehaviour {

    protected Vector3 aimDirection;
    [SerializeField] protected float cockPitRotationSpeed;
    protected float rotationSpeed;
    protected Vector3 moveDirection;
    protected Vector3 velocity;

    [SerializeField] protected GameObject cockPit;

    protected void RotateTankFixed()
    {
        // Rotate the player smoothly, depending on the velocity
        if (moveDirection.x != 0 || moveDirection.y != 0)
        {

            Quaternion targetRotation = new Quaternion();
            targetRotation.SetLookRotation(moveDirection);

            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        // Rotate the guns on the ship depending on the input of the right stick
        if (aimDirection.magnitude > 0.1f && cockPit)
        {
            //gunObject.transform.forward = shootDirection;
            Quaternion targetRotation = new Quaternion();
            targetRotation.SetLookRotation(aimDirection);

            cockPit.transform.rotation = Quaternion.Lerp(cockPit.transform.rotation, targetRotation, cockPitRotationSpeed * Time.fixedDeltaTime);
        }
    }

    protected void RotateTank()
    {
        // Rotate the player smoothly, depending on the velocity
        if (moveDirection.x != 0 || moveDirection.y != 0)
        {

            Quaternion targetRotation = new Quaternion();
            targetRotation.SetLookRotation(moveDirection);

            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Rotate the guns on the ship depending on the input of the right stick
        if (aimDirection.magnitude > 0.1f && cockPit)
        {
            //gunObject.transform.forward = shootDirection;
            Quaternion targetRotation = new Quaternion();
            targetRotation.SetLookRotation(aimDirection);

            cockPit.transform.rotation = Quaternion.Lerp(cockPit.transform.rotation, targetRotation, cockPitRotationSpeed * Time.deltaTime);
        }
    }

}
