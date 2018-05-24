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

    public int Level
    {
        get
        {
            return level;
        }
    }

    public int Attack
    {
        get
        {
            return attack;
        }
    }

    public float FireRate
    {
        get
        {
            return fireRate;
        }
    }

    public float ReloadSpeed
    {
        get
        {
            return reloadSpeed;
        }
    }

    public float KnockBack
    {
        get
        {
            return shootKnockback * shootKnockbackDuration;
        }
    }

    public int Defense
    {
        get
        {
            return defense;
        }
    }

    public float TopSpeed
    {
        get
        {
            return topSpeed;
        }
    }

    public float Acceleration
    {
        get
        {
            return acceleration;
        }
    }

    public float RotationSpeed
    {
        get
        {
            return rotationSpeed;
        }
    }

    public float Mass
    {
        get
        {
            return mass;
        }
    }

    #endregion

    #region Private Fields

    PlayerInput input;
    Animator anim;
    CameraShake camShake;
    GameObject cameraArm;
    
    [SerializeField] float cockPitRotationSpeed = 5f;
    Vector3 moveDirection;
    Vector3 velocity;

    Vector3 aimDirection;

    bool bIsGrounded = false;
    
    float shootTime;
    [SerializeField] GameObject shootOrigin;

    [Header("Physics"), SerializeField] float drag = 1f;
    [SerializeField] float rayToGroundLength = 1f;
    [SerializeField] float gravityCap = 3f;

    [Header("Tank Components"), SerializeField] GameObject cockPit;

    [Header("SoundS"), SerializeField] AudioSource shotSound;
    [SerializeField] AudioSource engineSound;
    [Range(0f, 0.1f), SerializeField] float engineSoundGain = 0.01f;
    [Range(0f, 1f), SerializeField] float idleEngineVolume = 0.01f;

    [Header("Camera Shake"), SerializeField] float shootCameraShakeAmount = 1f;
    [SerializeField] float shootCameraShakeDuration = 1f;

    LayerMask terrainLayer;

    // Attributes of the player
    [Header("Offensive Attributes"), SerializeField] int baseAttack = 1;
    [SerializeField] float basefireRate = 1f;
    [SerializeField] float baseReloadSpeed = 1f;
    [SerializeField] float baseShootKnockback = 1f;
    [SerializeField] float baseShootKnockbackDuration = 1f;

    [Header("Defensive Attributes"), SerializeField] int baseHealth = 1;
    [SerializeField] int baseDefense = 1;

    [Header("Agility Attributes"), SerializeField] float baseTopSpeed = 1f;
    [SerializeField] float baseAcceleration = 1f;
    [SerializeField] float baseRotationSpeed = 1f;

    [Header("Overall Attributes"), SerializeField] float baseMass = 1f;

    int level = 1;
    int attack;
    float fireRate;
    float reloadSpeed;
    float shootKnockback;
    float shootKnockbackDuration;

    int health;
    int defense;

    float topSpeed;
    float acceleration;
    float rotationSpeed;

    float mass;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        camShake = Camera.main.GetComponent<CameraShake>();
        shootTime = Time.realtimeSinceStartup;
        cameraArm = Camera.main.transform.parent.gameObject;

        // Create the layer Mask for the terrain
        int layer = LayerMask.NameToLayer("Terrain");
        terrainLayer = 1 << layer;

        // Initialize Attributes with base Values
        attack = baseAttack;
        fireRate = basefireRate;
        reloadSpeed = baseReloadSpeed;
        shootKnockback = baseShootKnockback;
        shootKnockbackDuration = baseShootKnockbackDuration;

        health = baseHealth;
        defense = baseDefense;

        topSpeed = baseTopSpeed;
        acceleration = baseAcceleration;
        rotationSpeed = baseRotationSpeed;

        mass = baseMass;

    }

    private void FixedUpdate()
    {
        GetInput();
        RotatePlayer();
        UpdateIsGrounded();
        CalculateVelocity();
        if (EventSystem.current)
        {
            if (input.Shoot && Time.realtimeSinceStartup > shootTime + fireRate && !EventSystem.current.IsPointerOverGameObject())
            {
                Shoot();
            }
        }
        else
        {
            if (input.Shoot && Time.realtimeSinceStartup > shootTime + fireRate)
            {
                Shoot();
            }
        }
        if (engineSound)
        {
            PlayEngineSound();
        }
        // Apply the gravity
        if (velocity.y < gravityCap)
        {
            velocity += (Vector3.down * (-Physics.gravity.y * mass)) * Time.fixedDeltaTime;
        }
        transform.position += velocity * Time.fixedDeltaTime;
    }

    #endregion

    #region Private Methods

    public void SpendPointOnAttribute()
    {

    }

    void GetInput()
    {
        // Calculate the offset angle on the y axis from the Camera's forward to the global forward
        float yAngle = Vector3.Angle(Vector3.forward, cameraArm.transform.forward);
        // Calculate the real 360 degree angle for when the rotation is anti clockwise
        if (cameraArm.transform.forward.x < 0f)
        {
            yAngle = 360 - yAngle;
        }
        moveDirection.x = input.Horizontal;
        moveDirection.z = input.Vertical;
        // Apply the offset angle on the y axis to the moveDirection vector --> Camera Relative Controls
        moveDirection = Quaternion.AngleAxis(yAngle, Vector3.up) * moveDirection;

        if (GameManager.Instance.IsControllerInput)
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
        if (input.Horizontal != 0 || input.Vertical != 0)
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

    void UpdateIsGrounded()
    {
        if(Physics.Raycast(transform.position, Vector3.down, rayToGroundLength, terrainLayer))
        {
            bIsGrounded = true;
        }
        else
        {
            bIsGrounded = false;
        }
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
