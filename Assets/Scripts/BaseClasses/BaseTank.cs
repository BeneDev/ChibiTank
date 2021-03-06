﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script from which everything, which controls a tank, inherits to gain access to methods and variables, making it easy to control and configure the tank
/// </summary>
public class BaseTank : BaseCharacter {

    #region Fields

    protected Animator anim; // The animator, to trigger the shooting animation

    protected bool isDead = false; // Stores wether the player is dead or not

    protected CameraShake camShake; // The camera shake component on the camera, to make the camera shake when shooting

    // The fields, defining how strong and long the Camera will shake when the Tank dies
    [Header("Camera Shake"), SerializeField] protected float deathCamShakeAmount = 0.2f;
    [SerializeField] protected float deathCamShakeDuration = 0.1f;

    [Header("Timer"), SerializeField] float autoReloadDelay = 0.3f; // The delay after the last shot in the magazine was fired and the automatic reload start

    protected Vector3 aimDirection; // This value is used to make the tanks turret aim at certain positions in the world
    [SerializeField] protected float cockPitRotationSpeed; // How fast the cockpit rotates
    protected float rotationSpeed; // How fast the tank body rotates
    protected Vector3 moveDirection; // Where the tank moves
    protected Vector3 velocity; // How fast the tank moves

    [Header("Physics"), SerializeField] protected float drag = 1f; // How fast the Tank loses velocity (friction with ground can be simulated here)
    [SerializeField] float gravityCap = 3f; // The gravity velocity wont go higher than this value in the negative

    protected float shootTime; // This stores the time when the player last shot, to assure the cooldown time between shots

    [Header("Parts"), SerializeField] protected GameObject cockPit; // The cockpit gameobject, to rotate it
    [SerializeField] protected GameObject shootOrigin; // The point, where projectiles will be instantiated
    [SerializeField] protected GameObject flashLight;

    [Header("Particles"), SerializeField] ParticleSystem shotSparks; // The particle system for shots
    [SerializeField] ParticleSystem deathExplosion; // The particle system for dying
    [SerializeField] ParticleSystem engineSmoke; // The particle system for driving and standing with the engine running
    [SerializeField] ParticleSystem deathSmoke; // The particle system for when the tank is already destroyed

    [Header("Sounds"), SerializeField] protected AudioSource shotSound; // Sound which gets played when the tank shoots
    [SerializeField] AudioClip[] explosionSounds; // Sounds for the explosion when the tank is being destroyed
    [SerializeField] AudioSource explosionAudioSource; // The Audio source which plays the explosion sounds
    [SerializeField] AudioSource reloadSource; // The audio Source with the reload clip inside of it. Played when the reload cursor animation is finished

    // Attributes necessary for every Tank
    protected int level = 1;
    protected int attack;
    protected float reloadSpeed;
    protected int magazineSize;
    protected float fireRate;
    protected float shootKnockback;
    protected float shootKnockbackDuration;

    protected int maxHealth;
    protected int maxShield;
    protected float topSpeed;
    protected float acceleration;

    protected float mass;

    protected int shotsInMagazine = 5;

    protected bool isReloading = false;

    // Attributes of the tank for serialization
    [Header("Offensive Attributes"), SerializeField] protected int baseAttack = 1;
    [SerializeField] protected float basefireRate = 1f;
    [SerializeField] protected float baseReloadSpeed = 1f;
    [SerializeField] protected int baseMagazineSize = 5;
    [SerializeField] protected float baseShootKnockback = 1f;
    [SerializeField] protected float baseShootKnockbackDuration = 1f;

    [Header("Defensive Attributes"), SerializeField] protected int baseHealth = 1;
    [SerializeField] protected int baseDefense = 1;
    [SerializeField] protected int baseShield = 0;

    [Header("Agility Attributes"), SerializeField] protected float baseTopSpeed = 1f;
    [SerializeField] protected float baseAcceleration = 1f;
    [SerializeField] protected float baseRotationSpeed = 1f;

    [Header("Overall Attributes"), SerializeField] protected float baseMass = 1f;

    #endregion

    #region Unity Messages

    // Get necessary References to Components and set the inital shootTime
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        camShake = Camera.main.GetComponent<CameraShake>();

        shootTime = Time.realtimeSinceStartup;

        OverlayController.Instance.OnReloadFinished += WhenReloadFinished;

        // Initialize Attributes with base Values
        attack = baseAttack;
        fireRate = basefireRate;
        reloadSpeed = baseReloadSpeed;
        magazineSize = baseMagazineSize;
        shootKnockback = baseShootKnockback;
        shootKnockbackDuration = baseShootKnockbackDuration;

        health = baseHealth;
        maxHealth = baseHealth;
        defense = baseDefense;
        shield = baseShield;
        maxShield = baseShield;

        topSpeed = baseTopSpeed;
        acceleration = baseAcceleration;
        rotationSpeed = baseRotationSpeed;
        cockPitRotationSpeed = baseRotationSpeed;

        mass = baseMass;
    }

    // This should be called at the end of each fixed update of all Tanks who inherit from this baseTank script, as it Calculates the velocity based on the current MoveDirection and the gravity. 
    // Also checks if the Tank is dead and plays deathsmoke then.
    protected virtual void FixedUpdate()
    {
        // Apply the gravity
        if (velocity.y > -gravityCap)
        {
            velocity += (Vector3.down * (-Physics.gravity.y * mass)) * Time.fixedDeltaTime;
        }
        // TODO This causes a weird bug, making the enemy jumping to (-infinity, -infinity, infinity) probably after some time, because the velocity gets too big in the gravity value
        transform.position += velocity * Time.fixedDeltaTime;
        if(shotsInMagazine <= 0 && !isReloading)
        {
            StartCoroutine(AutoStartReloading(autoReloadDelay));
        }
        if(!isDead && deathSmoke.isPlaying)
        {
            deathSmoke.Stop();
        }
        if(health <= 0)
        {
            Die();
        }
    }

    #endregion

    #region Helper Methods

    protected virtual void ToggleFlashlight()
    {
        if(flashLight)
        {
            flashLight.SetActive(!flashLight.activeSelf);
        }
    }

    IEnumerator AutoStartReloading(float seconds)
    {
        isReloading = true;
        yield return new WaitForSeconds(seconds);
        StartReloading();
    }

    // This makes the Tank Explode, stop the engine smoke and start the death smoke. Also sets the isDead field.
    protected virtual void Die()
    {
        StartCoroutine(ExplosionCameraShake());
        if(deathExplosion && !deathExplosion.isPlaying)
        {
            deathExplosion.Play();
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

    // Plays a given clip from a given audio source
    protected virtual void PlaySoundAtSource(AudioClip clip, AudioSource source)
    {
        source.clip = clip;
        if(!source.isPlaying)
        {
            source.Play();
        }
    }

    // Controls several aspects of the game whilst something is exploding, like screenshake and sound
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
        yield return new WaitForSeconds(1f);
    }

    // Calculates the velocity, taking into account the tank specific attributes and the gravity
    protected virtual void CalculateVelocity()
    {
        // This heavily relies on the fixed timestep. If this one is too low, the tank will seem laggy
        if (velocity.magnitude + (moveDirection.magnitude * acceleration) < topSpeed)
        {
            velocity += moveDirection * acceleration;
        }
        else
        {
            velocity += moveDirection.normalized * (topSpeed - velocity.magnitude + (moveDirection.magnitude * acceleration));
        }
        velocity = velocity * (1 - Time.fixedDeltaTime * drag * mass);
    }

    // Call this to rotate the turret of the tank
    protected void RotateTurret()
    {
        // Rotate the guns on the ship depending on the input of the right stick
        if (aimDirection.magnitude > 0.1f && cockPit)
        {
            //gunObject.transform.forward = shootDirection;
            Quaternion targetRotation = new Quaternion();
            targetRotation.SetLookRotation(aimDirection);
            targetRotation.z = 0f;

            cockPit.transform.rotation = Quaternion.Lerp(cockPit.transform.rotation, targetRotation, cockPitRotationSpeed * Time.fixedDeltaTime);
        }
    }

    // Call this to Rotate the body of the tank
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

    // Call this to make the tank shoot. Handles the shoot time for cooldown, sound, knockback and instantiation of the projectile
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
        // TODO make this functional for different kinds of projectiles, even a flamethrower
        GameObject currentBall = GameManager.Instance.GetCannonBall(shootOrigin.transform.position, cockPit.transform.forward);

        currentBall.GetComponent<CannonBallController>().Damage = attack;
        currentBall.GetComponent<CannonBallController>().Owner = gameObject;
        anim.SetTrigger("Shoot");
        StartCoroutine(ShotKnockBack(shootKnockbackDuration));
        shotsInMagazine--;
    }

    // Make the cursor rotate to show player is reloading and make magazine full again
    protected virtual void StartReloading()
    {
        isReloading = true;
    }

    protected virtual void WhenReloadFinished()
    {
        isReloading = false;
        shotsInMagazine = magazineSize;
        // Play sound to show the player that the reloading is finished
        if(reloadSource && !reloadSource.isPlaying)
        {
            reloadSource.Play();
        }
    }

    // Apply the knockback after a shot is fired from the tank
    protected IEnumerator ShotKnockBack(float duration)
    {
        for (float t = 0; t < duration; t += Time.fixedDeltaTime)
        {
            // Apply knockback in the -aimDirection
            velocity = -cockPit.transform.forward * shootKnockback * (duration - t);
            yield return new WaitForEndOfFrame();
        }
    }

    #endregion

}
