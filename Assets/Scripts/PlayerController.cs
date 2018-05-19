using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {

    #region Properties



    #endregion

    #region Private Fields

    PlayerInput input;
    Animator anim;
    CameraShake camShake;

    [Header("Movement"), SerializeField] float acceleration = 1f;
    [SerializeField] float topSpeed = 3f;
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] float cockPitRotationSpeed = 2f;
    Vector3 moveDirection;
    Vector3 velocity;

    Vector3 aimDirection;

    [Header("Shooting"), SerializeField] float shootDelay = 1f;
    float shootTime;
    [SerializeField] GameObject shootOrigin;

    [Header("Physics"), SerializeField] float drag = 1f;
    [SerializeField] float shootKnockback = 1f;
    [SerializeField] float shootKnockbackDuration = 0.5f;

    [Header("Tank Components"), SerializeField] GameObject cockPit;

    [Header("SoundS"), SerializeField] AudioSource shotSound;
    [SerializeField] AudioSource engineSound;
    [Range(0f, 0.1f), SerializeField] float engineSoundGain = 0.01f;
    [Range(0f, 1f), SerializeField] float idleEngineVolume = 0.01f;

    [Header("Camera Shake"), SerializeField] float shootCameraShakeAmount = 1f;
    [SerializeField] float shootCameraShakeDuration = 1f;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        camShake = Camera.main.GetComponent<CameraShake>();
        shootTime = Time.realtimeSinceStartup;
    }

    private void FixedUpdate()
    {
        GetInput();
        RotatePlayer();
        CalculateVelocity();
        if(input.Shoot && Time.realtimeSinceStartup > shootTime + shootDelay)
        {
            if(shotSound)
            {
                shotSound.Play();
            }
            shootTime = Time.realtimeSinceStartup;
            GameObject currentBall = GameManager.Instance.GetCannonBall(shootOrigin.transform.position, cockPit.transform.forward);
            camShake.shakeAmount = shootCameraShakeAmount;
            camShake.shakeDuration = shootCameraShakeDuration;
            anim.SetTrigger("Shoot");
            StartCoroutine(ShotKnockBack(shootKnockbackDuration));
        }
        if(engineSound)
        {
            if (velocity.magnitude * engineSoundGain > idleEngineVolume)
            {
                engineSound.volume = velocity.magnitude * engineSoundGain;
            }
            else
            {
                engineSound.volume = idleEngineVolume;
            }
        }
        transform.position += velocity * Time.fixedDeltaTime;
    }

    #endregion

    #region Private Methods

    void GetInput()
    {
        moveDirection.x = input.Horizontal;
        moveDirection.z = input.Vertical;

        aimDirection.x = input.R_Horizontal;
        aimDirection.z = input.R_Vertical;
    }

    void RotatePlayer()
    {
        // Rotate the player smoothly, depending on the velocity
        if (input.Horizontal != 0 || input.Vertical != 0)
        {
            Quaternion targetRotation = new Quaternion();
            targetRotation.SetLookRotation(moveDirection);

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
        if(velocity.magnitude < topSpeed)
        {
            velocity += moveDirection * acceleration;
        }
        velocity = velocity * (1 - Time.fixedDeltaTime * drag);
    }

    IEnumerator ShotKnockBack(float duration)
    {
        for(float t = 0; t < duration; t += Time.fixedDeltaTime)
        {
            // Apply knockback in the -aimDirection
            velocity = -cockPit.transform.forward * shootKnockback * (duration - t);
            yield return new WaitForEndOfFrame();
        }
    }

    #endregion
}
