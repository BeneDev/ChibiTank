﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

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

    [Header("Cannon Balls"), SerializeField] int cannonBallCount = 100;
    [SerializeField] public GameObject cannonBallParent;
    [SerializeField] GameObject cannonBallPrefab;
    public Stack<GameObject> freeCannonBalls = new Stack<GameObject>();

    Vector3 respawnPoint = new Vector3(22f, 0f, -60);

    private int controllerCount = 0;

    bool isPaused = false;

    CursorLockMode lockedToWindow;

    [SerializeField] bool isControllerInput = false;

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
        GameObject ballToReturn = freeCannonBalls.Pop();
        ballToReturn.transform.position = positionToSpawn;
        ballToReturn.transform.forward = direction;
        ballToReturn.SetActive(true);
        return ballToReturn;
    }
}
