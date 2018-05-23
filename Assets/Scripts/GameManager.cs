using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    [Header("Cannon Balls"), SerializeField] int cannonBallCount = 100;
    [SerializeField] public GameObject cannonBallParent;
    [SerializeField] GameObject cannonBallPrefab;
    public Stack<GameObject> freeCannonBalls = new Stack<GameObject>();

    private int controllerCount = 0;

    bool isPaused = false;

    CursorLockMode lockedToWindow;

    [SerializeField] public bool isControllerInput = false;

    private void Awake()
    {
        lockedToWindow = CursorLockMode.Confined;
        Cursor.lockState = lockedToWindow;
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
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
        if(controllerCount > 0)
        {
            isControllerInput = true;
        }
        else
        {
            isControllerInput = false;
        }
    }

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

    public GameObject GetCannonBall(Vector3 positionToSpawn, Vector3 direction)
    {
        // TODO make direction aim a little bit higher for nicer flying curve
        GameObject ballToReturn = freeCannonBalls.Pop();
        ballToReturn.transform.position = positionToSpawn;
        ballToReturn.transform.forward = direction;
        ballToReturn.SetActive(true);
        return ballToReturn;
    }
}
