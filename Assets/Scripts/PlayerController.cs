using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : BaseTank {

    #region Properties

    // Lets the enemies react to a dead player, instead of keeping attacking
    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    // Lets the camera reset its own rotation
    public Vector3 CockPitForward
    {
        get
        {
            return cockPit.transform.forward;
        }
    }

    // The attributes, so they can be viewed in the status menu.
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
    GameObject cameraArm;

    // Stores an enemy if the player aims at one
    GameObject enemyInFront;
    // Stores the gameobject of an npc if the player aims at one
    GameObject npcToTalkTo;

    bool bIsGrounded = false;
    
    [SerializeField] float rayToGroundLength = 1f; 
    [SerializeField] float talkDistance = 5f; // How far the player can be away from an NPC and still talk to him instead of shoot at him
    
    [SerializeField] AudioSource engineSound;
    [Range(0f, 0.1f), SerializeField] float engineSoundGain = 0.01f; // How much the engine sound volume gains when the player starts driving
    [Range(0f, 1f), SerializeField] float idleEngineVolume = 0.01f; // The volume of the engine sound, when the player is not driving

    [Header("Camera Shake"), SerializeField] float shootCameraShakeAmount = 1f;
    [SerializeField] float shootCameraShakeDuration = 1f;

    LayerMask terrainLayer; // LayerMask with the ground included, to raycast for isGrounded

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

    // Get necessary components, create ground layer and initialise attributes
    protected override void Awake()
    {
        base.Awake();
        input = GetComponent<PlayerInput>();
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

    // Let the player move and shoot, whilst playing the right animations and particle effects, reacting to the environment at all times
    protected override void FixedUpdate()
    {
        if(isDead) { return; }
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
                    Vector3 toNPC = npcToTalkTo.transform.position - transform.position;
                    if (toNPC.magnitude < talkDistance)
                    {
                        npcToTalkTo.GetComponent<NPCTank>().Interact();
                    }
                    else
                    {
                        Shoot();
                    }
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
                    Vector3 toNPC = npcToTalkTo.transform.position - transform.position;
                    if (toNPC.magnitude < talkDistance)
                    {
                        npcToTalkTo.GetComponent<NPCTank>().Interact();
                    }
                    else
                    {
                        Shoot();
                    }
                }
            }
        }
        if (engineSound)
        {
            PlayEngineSound();
        }
        base.FixedUpdate();
    }

    // Update the enemyInFront and npcToTalkTo Variables with the gameObjects in front of the player
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

    // Update the enemyInFront and npcToTalkTo Variables to be empty again
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

    // When the player levels up, he gains attribute points, which he can use to strengthen specific attributes. 
    //When the player chose to do so, this method takes in a parameter, to define what attribute to enhance, and enhances that particular attribute then.
    public void SpendPointOnAttribute()
    {

    }

    // Make the tank die and explode and show the Gameover menu after that
    protected override void Die()
    {
        base.Die();
        GameoverMenu.Show();
    }

    // Get the player tank into the original form. Called after the tank respawns
    public void ResetPlayerTank()
    {
        health = baseHealth;
        foreach(var menu in MenuManager.Instance.MenuStack)
        {
            Destroy(menu.gameObject);
        }
        MenuManager.Instance.MenuStack.Clear();
        Camera.main.GetComponentInParent<Animator>().SetTrigger("Idle");
        isDead = false;
    }

    // Reset the camera to the current cockpit rotation or lock it to an enemy if one is in front of the player
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

    // Reads the input and writes the data into the right variables. Also converts to Camera Relative controls. 
    // Wont let the player aim, when hovering over UI.
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

    // Shoot a projectile and shake the camera
    protected override void Shoot()
    {
        base.Shoot();
        camShake.shakeAmount = shootCameraShakeAmount;
        camShake.shakeDuration = shootCameraShakeDuration;
    }

    // Play the sound of the tank engine at the right volume
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

    // Check if the player is on the ground or not, to apply more gravity when he is in the air
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
