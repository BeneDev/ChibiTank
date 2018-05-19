using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    [Header("Cannon Balls"), SerializeField] int cannonBallCount = 100;
    [SerializeField] public GameObject cannonBallParent;
    [SerializeField] GameObject cannonBallPrefab;
    public Stack<GameObject> freeCannonBalls = new Stack<GameObject>();

    private void Awake()
    {
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
