using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {

    #region Properties

    public Vector3 CockPitForward
    {
        get
        {
            return cockPit.transform.forward;
        }
    }

    public float CockPitRotationSpeed
    {
        get
        {
            return cockPitRotationSpeed;
        }
        set
        {
            cockPitRotationSpeed = value;
        }
    }

    #endregion

    #region Private Fields

    PlayerInput input;
    Animator anim;
    CameraShake camShake;
    GameObject cameraArm;

    [Header("Movement"), SerializeField] float acceleration = 1f;
    [SerializeField] float topSpeed = 3f;
    [SerializeField] float fixedTankRotationSpeed = 1f;
    float rotationSpeed;
    [Range(0f, 1f), SerializeField] float backwardsRotationSpeedMultiplier = 0.25f;
    [SerializeField] float cockPitRotationSpeed = 5f;
    Vector3 moveDirection;
    Vector3 velocity;

    Vector3 aimDirection;

    bool bIsGrounded = false;

    [Header("Shooting"), SerializeField] float shootDelay = 1f;
    float shootTime;
    [SerializeField] GameObject shootOrigin;

    [Header("Physics"), SerializeField] float drag = 1f;
    [SerializeField] float shootKnockback = 1f;
    [SerializeField] float shootKnockbackDuration = 0.5f;
    [SerializeField] float mass = 10f;

    [Header("Tank Components"), SerializeField] GameObject cockPit;

    [Header("SoundS"), SerializeField] AudioSource shotSound;
    [SerializeField] AudioSource engineSound;
    [Range(0f, 0.1f), SerializeField] float engineSoundGain = 0.01f;
    [Range(0f, 1f), SerializeField] float idleEngineVolume = 0.01f;

    [Header("Camera Shake"), SerializeField] float shootCameraShakeAmount = 1f;
    [SerializeField] float shootCameraShakeDuration = 1f;

    //LayerMask terrainLayer;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        camShake = Camera.main.GetComponent<CameraShake>();
        shootTime = Time.realtimeSinceStartup;
        cameraArm = Camera.main.transform.parent.gameObject;

        // Create the terrain layer mask
        //int layer = LayerMask.NameToLayer("Terrain");
        //terrainLayer = 1 << layer;
    }

    private void FixedUpdate()
    {
        GetInput();
        RotatePlayer();
        //UpdateIsGrounded();
        CalculateVelocity();
        if (EventSystem.current)
        {
            if (input.Shoot && Time.realtimeSinceStartup > shootTime + shootDelay && !EventSystem.current.IsPointerOverGameObject())
            {
                Shoot();
            }
        }
        else
        {
            if (input.Shoot && Time.realtimeSinceStartup > shootTime + shootDelay)
            {
                Shoot();
            }
        }
        if (engineSound)
        {
            PlayEngineSound();
        }
        // Apply the gravity
        velocity += (Vector3.down * (-Physics.gravity.y * mass)) * Time.fixedDeltaTime;
        transform.position += velocity * Time.fixedDeltaTime;
    }

    #endregion

    #region Private Methods

    void GetInput()
    {
        // Calculate the offset angle on the y axis from the Camera's forward to the global forward
        float yAngle = Vector3.Angle(Vector3.forward, cameraArm.transform.forward);
        // Calculate the real 360 degree angle for when the rotation is anti clockwise
        if (cameraArm.transform.forward.x < 0f)
        {
            yAngle = 360 - yAngle;
        }
        //Debug.LogFormat("X: {0} | Y: {1} | Z: {2}", cameraArm.transform.forward.x, cameraArm.transform.forward.y, cameraArm.transform.forward.z);
        moveDirection.x = input.Horizontal;
        moveDirection.z = input.Vertical;
        // Apply the offset angle on the y axis to the moveDirection vector --> Camera Relative Controls
        moveDirection = Quaternion.AngleAxis(yAngle, Vector3.up) * moveDirection;

        if (GameManager.Instance.isControllerInput)
        {
            aimDirection.x = input.R_Horizontal;
            aimDirection.z = input.R_Vertical;
            // Apply the offset angle on the y axis to the aimDirection vector --> Camera Relative Controls
            aimDirection = Quaternion.AngleAxis(yAngle, Vector3.up) * aimDirection;
        }
        else
        {
            if (EventSystem.current)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    Physics.Raycast(ray, out hit, 500f);
                    Vector3 mousePosInWorld = hit.point;
                    Vector3 targetAim = mousePosInWorld - transform.position;
                    aimDirection.x = targetAim.normalized.x;
                    aimDirection.z = targetAim.normalized.z;
                }
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, 500f);
                Vector3 mousePosInWorld = hit.point;
                Vector3 targetAim = mousePosInWorld - transform.position;
                aimDirection.x = targetAim.normalized.x;
                aimDirection.z = targetAim.normalized.z;
            }
        }
    }

    void RotatePlayer()
    {
        // Rotate the player smoothly, depending on the velocity
        if (input.Horizontal != 0 || input.Vertical > 0)
        {
            if (input.Vertical > -0.5f)
            {
                rotationSpeed = fixedTankRotationSpeed;
            }
            else
            {
                rotationSpeed = fixedTankRotationSpeed * backwardsRotationSpeedMultiplier;
            }

            // Set the rotationspeed to 2/3 if on keyboard, because it was feeling different for KBM and Controller as the cam rotated with the tank itself and not the aim Direction
            //if(!GameManager.Instance.isControllerInput)
            //{
            //    rotationSpeed *= 0.66666666f;
            //}

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
    void UpdateIsGrounded()
    {
        //if (Physics.Raycast(transform.position, Vector3.down, 3f, terrainLayer))
        //{
        //    bIsGrounded = true;
        //}
        //else
        //{
        //    bIsGrounded = false;
        //}
    }

    private void Shoot()
    {
        if (shotSound)
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

    private void PlayEngineSound()
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
