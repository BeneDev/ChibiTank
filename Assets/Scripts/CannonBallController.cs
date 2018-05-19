using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallController : MonoBehaviour {

    Rigidbody rb;
    [SerializeField] float speed = 1f;
    [SerializeField] float durationUntilDespawn = 3f;
    float timeWhenFired = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rb.AddForce(transform.forward * speed);
        timeWhenFired = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        if(Time.realtimeSinceStartup > timeWhenFired + durationUntilDespawn)
        {
            gameObject.SetActive(false);
            gameObject.transform.SetParent(GameManager.Instance.cannonBallParent.transform);
            GameManager.Instance.freeCannonBalls.Push(gameObject);
        }
    }
}
