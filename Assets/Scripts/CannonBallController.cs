using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script which controls the cannonball behavior after intantiated
/// </summary>
public class CannonBallController : MonoBehaviour {

    #region Properties

    // The damage the cannonball will deal on impact
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

    // The owner, to prevent hitting the owner
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

    #endregion

    #region Fields

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
    [SerializeField] AudioSource enemyHitSound;

    struct Sounds
    {
        public int terrain;
        public int mechanical;
        public int metal;
        public int organic;
    }
    Sounds sounds;

    #endregion

    #region Unity Messages

    // Get the necessary Components and bind the sounds to the materials in the struct
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        BindSounds();
    }

    // Add the force forward to make the cannonball go flying
    private void OnEnable()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * speed);
        timeWhenFired = Time.realtimeSinceStartup;
        isStillDamaging = true;
    }

    // Check if the cannonball is still damaging and put the cannonball back into the stack of free cannonballs after 3 seconds
    private void Update()
    {
        if (rb.velocity.magnitude < 10f)
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

    // Handle the actions, the cannonball will do when hitting a material
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
        if(collision.gameObject.GetComponent<BaseCover>())
        {
            collision.gameObject.GetComponent<BaseCover>().TakeDamage(damage);
            isStillDamaging = false;
        }
        if(collision.gameObject.GetComponent<BaseCharacter>())
        {
            if(enemyHitSound && !enemyHitSound.isPlaying && collision.gameObject.tag != "Player")
            {
                enemyHitSound.Play();
            }
            collision.gameObject.GetComponent<BaseCharacter>().TakeDamage(damage);
            isStillDamaging = false;
        }
    }

    #endregion

    #region Helper Methods

    // Play the audio clip in the given audio source
    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    // Write the right sounds for different materials in the world into the struct
    void BindSounds()
    {
        sounds.terrain = 0;
        sounds.mechanical = 1;
        sounds.metal = 2;
        sounds.organic = 3;
    }

    #endregion
}
