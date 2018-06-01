using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script from which everything, controlling a tank inherits, to gain access to methods and variables, making it easy to controll and configure the tank
/// </summary>
public class BaseTank : BaseCharacter {

    protected Animator anim;

    protected bool isDead = false;

    protected CameraShake camShake;

    [Header("Camera Shake"), SerializeField] protected float deathCamShakeAmount = 0.2f;
    [SerializeField] protected float deathCamShakeDuration = 0.1f;

    protected Vector3 aimDirection;
    [SerializeField] protected float cockPitRotationSpeed;
    protected float rotationSpeed;
    protected Vector3 moveDirection;
    protected Vector3 velocity;

    [Header("Physics"), SerializeField] protected float drag = 1f;
    [SerializeField] float gravityCap = 3f;

    protected float shootTime;

    [Header("Parts"), SerializeField] protected GameObject cockPit;
    [SerializeField] protected GameObject shootOrigin;

    [Header("Particles"), SerializeField] ParticleSystem shotSparks;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem engineSmoke;
    [SerializeField] ParticleSystem deathSmoke;

    [Header("SoundS"), SerializeField] protected AudioSource shotSound;
    [SerializeField] AudioClip[] explosionSounds;
    [SerializeField] AudioSource explosionAudioSource;

    protected int level = 1;
    protected int attack;
    protected float reloadSpeed;
    protected float fireRate;
    protected float shootKnockback;
    protected float shootKnockbackDuration;

    protected float topSpeed;
    protected float acceleration;

    protected float mass;

    protected virtual void Awake()
    {
        shootTime = Time.realtimeSinceStartup;
        anim = GetComponent<Animator>();
        camShake = Camera.main.GetComponent<CameraShake>();
    }

    // This should be called at the end of each fixed update 
    protected virtual void FixedUpdate()
    {
        // Apply the gravity
        if (velocity.y > -gravityCap)
        {
            velocity += (Vector3.down * (-Physics.gravity.y * mass)) * Time.fixedDeltaTime;
        }
        transform.position += velocity * Time.fixedDeltaTime;
        if(!isDead && deathSmoke.isPlaying)
        {
            deathSmoke.Stop();
        }
        if(health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        StartCoroutine(ExplosionCameraShake());
        if(deathParticle && !deathParticle.isPlaying)
        {
            deathParticle.Play();
        }
        if(engineSmoke && engineSmoke.isPlaying)
        {
            engineSmoke.Stop();
        }
        if(deathSmoke && !deathSmoke.isPlaying)
        {
            deathSmoke.Play();
        }
        isDead = true;
    }

    protected virtual void PlaySoundAtSource(AudioClip clip, AudioSource source)
    {
        source.clip = clip;
        if(!source.isPlaying)
        {
            source.Play();
        }
    }

    protected virtual IEnumerator ExplosionCameraShake()
    {
        if (explosionSounds[0] && explosionAudioSource)
        {
            PlaySoundAtSource(explosionSounds[0], explosionAudioSource);
        }
        camShake.shakeAmount = deathCamShakeAmount;
        camShake.shakeDuration = deathCamShakeDuration;
        yield return new WaitForSeconds(1f);
        if (explosionSounds[1] && explosionAudioSource)
        {
            PlaySoundAtSource(explosionSounds[1], explosionAudioSource);
        }
        camShake.shakeAmount = deathCamShakeAmount * 2f;
        camShake.shakeDuration = deathCamShakeDuration * 2f;
    }

    protected virtual void CalculateVelocity()
    {
        // TODO make tank movement smoother
        if (velocity.magnitude < topSpeed)
        {
            velocity += moveDirection * acceleration;
        }
        velocity = velocity * (1 - Time.fixedDeltaTime * drag);
    }

    protected void RotateTurret()
    {
        // Rotate the guns on the ship depending on the input of the right stick
        if (aimDirection.magnitude > 0.1f && cockPit)
        {
            //gunObject.transform.forward = shootDirection;
            Quaternion targetRotation = new Quaternion();
            targetRotation.SetLookRotation(aimDirection);

            cockPit.transform.rotation = Quaternion.Lerp(cockPit.transform.rotation, targetRotation, cockPitRotationSpeed * Time.fixedDeltaTime);
        }
    }

    protected void RotateBody()
    {
        // Rotate the player smoothly, depending on the velocity
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = new Quaternion();
            targetRotation.SetLookRotation(moveDirection);
            targetRotation = Quaternion.Euler(new Vector3(0f, targetRotation.eulerAngles.y, 0f));
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    protected virtual void Shoot()
    {
        if (shotSound)
        {
            shotSound.Play();
        }
        if (shotSparks)
        {
            shotSparks.Play();
        }
        shootTime = Time.realtimeSinceStartup;
        GameObject currentBall = GameManager.Instance.GetCannonBall(shootOrigin.transform.position, cockPit.transform.forward);
        currentBall.GetComponent<CannonBallController>().Damage = attack;
        currentBall.GetComponent<CannonBallController>().Owner = gameObject;
        anim.SetTrigger("Shoot");
        StartCoroutine(ShotKnockBack(shootKnockbackDuration));
    }

    protected IEnumerator ShotKnockBack(float duration)
    {
        for (float t = 0; t < duration; t += Time.fixedDeltaTime)
        {
            // Apply knockback in the -aimDirection
            velocity = -cockPit.transform.forward * shootKnockback * (duration - t);
            yield return new WaitForEndOfFrame();
        }
    }

}
