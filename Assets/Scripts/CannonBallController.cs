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

    struct Sounds
    {
        public int terrain;
        public int mechanical;
        public int metal;
        public int organic;
    }
    Sounds sounds;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        BindSounds();
    }

    void BindSounds()
    {
        sounds.terrain = 0;
        sounds.mechanical = 1;
        sounds.metal = 2;
        sounds.organic = 3;
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
            PlaySound(impactSounds[sounds.terrain]);
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Mechanical"))
        {
            PlaySound(impactSounds[sounds.mechanical]);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Metal"))
        {
            PlaySound(impactSounds[sounds.metal]);
            print("metal");
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Organic"))
        {
            PlaySound(impactSounds[sounds.organic]);
        }
    }
}
