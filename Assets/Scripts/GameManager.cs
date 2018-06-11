using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The GameManager script, which controls several aspects of the game unaffected by loading scenes etc.
/// </summary>
public class GameManager : MonoBehaviour {

    #region Properties

    public static GameManager Instance { get; private set; }

    public bool IsControllerInput
    {
        get
        {
            return isControllerInput;
        }
        set
        {
            if (value)
            {
                if (controllerCount > 0)
                {
                    isControllerInput = value;
                }
            }
            else
            {
                isControllerInput = value;
            }
        }
    }

    public Vector3 RespawnPoint
    {
        get
        {
            return respawnPoint;
        }
        set
        {
            respawnPoint = value;
        }
    }

    public bool IsCursorVisible
    {
        get
        {
            return isCursorVisible;
        }
    }

    #endregion

    #region Fields

    [Header("Cannon Balls"), SerializeField] int cannonBallCount = 100; // How many cannonball projectiles will be instantiated at the beginning of the game for object pooling
    [SerializeField] public GameObject cannonBallParent; // Where in the hierarchy every cannonball gets stored to prevent the hierarchy from getting flodded
    [SerializeField] GameObject cannonBallPrefab;
    public Stack<GameObject> freeCannonBalls = new Stack<GameObject>(); // The cannonball stack of free to use cannonballs

    Vector3 respawnPoint = new Vector3(22f, 0f, -60); // TODO remove this obsolete field, because player position is read out of the save file now, when respawning

    private int controllerCount = 0; // How many controllers are connected, to check if the player can possibly play in controller mode

    CursorLockMode lockedToWindow; // The cursorLockMode which lets the mouse only move inside of the window

    [SerializeField] bool isControllerInput = false; // Stores if the player actually plays with controller

    bool isCursorVisible = true;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        // Load the game if there is a savestate
        SaveFile save = SaveFileManager.LoadGame();
        GameObject.FindGameObjectWithTag("Player").transform.position = save.playerPos;
        // Lock the mouse to the game window
        lockedToWindow = CursorLockMode.Confined;
        Cursor.lockState = lockedToWindow;
        // Make this a singleton class
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            // If there is no GameManager besides this one, make this the game Manager
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // Instantiate a defined number of balls into the free cannon balls stack (Preparation for object pooling)
            for (int i = 0; i < cannonBallCount; i++)
            {
                GameObject ballToPush = Instantiate(cannonBallPrefab, Vector3.zero, transform.rotation);
                ballToPush.SetActive(false);
                ballToPush.transform.SetParent(cannonBallParent.transform);
                freeCannonBalls.Push(ballToPush);
            }
        }
    }

    private void Update()
    {
        GetControllerCount();
        if(isControllerInput && isCursorVisible)
        {
            isCursorVisible = false;
        }
        if(!isControllerInput && !isCursorVisible)
        {
            isCursorVisible = true;
        }
    }

    #endregion

    #region Helper Methods

    // Looks for any connected controller and updates the counter for every connected controller
    void GetControllerCount()
    {
        string[] names = Input.GetJoystickNames();
        controllerCount = 0;
        for (int i = 0; i < names.Length; i++)
        {
            if (!string.IsNullOrEmpty(names[i]))
            {
                controllerCount++;
            }
        }
    }

    // Return a free to use cannonball to shoot 
    public GameObject GetCannonBall(Vector3 positionToSpawn, Vector3 direction)
    {
        GameObject ballToReturn = freeCannonBalls.Pop();
        ballToReturn.transform.position = positionToSpawn;
        ballToReturn.transform.forward = direction;
        ballToReturn.SetActive(true);
        return ballToReturn;
    }

    #endregion

}
