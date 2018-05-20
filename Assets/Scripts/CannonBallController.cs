using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallController : MonoBehaviour {

    Rigidbody rb;
    AudioSource audioSource;

    [SerializeField] float speed = 1f;
    [SerializeField] float durationUntilDespawn = 3f;
    float timeWhenFired = 0f;

    [SerializeField] AudioClip[] impactSounds;

    Dictionary<string, int> audioDict = new Dictionary<string, int>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        audioDict["terrain"] = 0;
        audioDict["mechanical"] = 1;
        audioDict["organic"] = 2;
    }

    private void OnEnable()
    {
        rb.velocity = Vector3.zero;
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

    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            PlaySound(impactSounds[audioDict["terrain"]]);
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Mechanical"))
        {
            PlaySound(impactSounds[audioDict["mechanical"]]);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Organic"))
        {
            PlaySound(impactSounds[audioDict["organic"]]);
        }
    }
}
