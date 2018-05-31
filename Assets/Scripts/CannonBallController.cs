using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallController : MonoBehaviour {

    public int Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }

    public GameObject Owner
    {
        get
        {
            return owner;
        }
        set
        {
            owner = value;
        }
    }

    Rigidbody rb;
    AudioSource audioSource;

    [SerializeField] float speed = 1f;
    [SerializeField] float durationUntilDespawn = 3f;
    float timeWhenFired = 0f;

    [SerializeField] ParticleSystem onContactExplosion;

    int damage;
    bool isStillDamaging = true;

    GameObject owner;

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
        isStillDamaging = true;
    }

    private void Update()
    {
        if (rb.velocity.magnitude < 8f)
        {
            isStillDamaging = false;
        }
        if (Time.realtimeSinceStartup > timeWhenFired + durationUntilDespawn)
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
        if (collision.gameObject == owner || !isStillDamaging) { return; }
        if (onContactExplosion)
        {
            onContactExplosion.Play();
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
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
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Organic"))
        {
            PlaySound(impactSounds[sounds.organic]);
        }
        if(collision.gameObject.GetComponent<BaseCharacter>())
        {
            collision.gameObject.GetComponent<BaseCharacter>().TakeDamage(damage);
            isStillDamaging = false;
        }
    }
}
