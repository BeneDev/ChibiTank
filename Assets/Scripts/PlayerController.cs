using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : BaseTank {

    #region Properties

    public bool IsDead
    {
        get
        {
            return bIsDead;
        }
    }

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
    CameraShake camShake;
    GameObject cameraArm;

    // Stores an enemy if the player aims at one
    GameObject enemyInFront;
    // Stores the gameobject of an npc if the player aims at one
    GameObject npcToTalkTo;

    bool bIsGrounded = false;
    bool bIsDead = false;
    
    [SerializeField] float rayToGroundLength = 1f;
    
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

    #endregion

    #region Unity Messages

    protected override void Awake()
    {
        base.Awake();
        input = GetComponent<PlayerInput>();
        camShake = Camera.main.GetComponent<CameraShake>();
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

    protected override void FixedUpdate()
    {
        if(bIsDead) { return; }
        GetInput();
        RotateTurret();
        RotateBody();
        UpdateIsGrounded();
        CalculateVelocity();
        if(input.ResetCam)
        {
            SetCamera();
        }
        if (EventSystem.current)
        {
            if (input.Shoot)
            {
                if (Time.realtimeSinceStartup > shootTime + fireRate && !EventSystem.current.IsPointerOverGameObject() && npcToTalkTo == null)
                {
                    Shoot();
                }
                else if (npcToTalkTo != null && !EventSystem.current.IsPointerOverGameObject())
                {
                    npcToTalkTo.GetComponent<NPCTank>().Interact();
                }
            }
        }
        else
        {
            if (input.Shoot)
            {
                if (Time.realtimeSinceStartup > shootTime + fireRate && npcToTalkTo == null)
                {
                    Shoot();
                }
                else if (npcToTalkTo != null)
                {
                    npcToTalkTo.GetComponent<NPCTank>().Interact();
                }
            }
        }
        if (engineSound)
        {
            PlayEngineSound();
        }
        base.FixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemyInFront = other.gameObject;
        }
        if(other.tag == "NPC")
        {
            npcToTalkTo = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemyInFront = null;
        }
        if(other.tag == "NPC")
        {
            npcToTalkTo = null;
        }
    }

    #endregion

    #region Private Methods

    public void SpendPointOnAttribute()
    {

    }

    protected override void Die()
    {
        if (!bIsDead)
        {
            bIsDead = true;
            GameoverMenu.Show();
        }
    }

    public void ResetPlayerTank()
    {
        health = baseHealth;
        foreach(var menu in MenuManager.Instance.MenuStack)
        {
            Destroy(menu.gameObject);
        }
        MenuManager.Instance.MenuStack.Clear();
        Camera.main.GetComponentInParent<Animator>().SetTrigger("Idle");
        transform.position = GameManager.Instance.RespawnPoint;
        bIsDead = false;
    }

    private void SetCamera()
    {
        if (enemyInFront)
        {
            cameraArm.GetComponent<CameraController>().FocussedObject = enemyInFront;
            return;
        }
        cameraArm.GetComponent<CameraController>().FocussedObject = null;
        cameraArm.GetComponent<CameraController>().CamResetRotation = aimDirection;
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

    protected override void Shoot()
    {
        base.Shoot();
        camShake.shakeAmount = shootCameraShakeAmount;
        camShake.shakeDuration = shootCameraShakeDuration;
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

    #endregion
}
