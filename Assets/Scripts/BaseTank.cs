using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script from which everything, controlling a tank inherits, to gain access to methods and variables, making it easy to controll and configure the tank
/// </summary>
public class BaseTank : BaseCharacter {

    protected Animator anim;

    protected Vector3 aimDirection;
    [SerializeField] protected float cockPitRotationSpeed;
    protected float rotationSpeed;
    protected Vector3 moveDirection;
    protected Vector3 velocity;

    [SerializeField] protected float drag = 1f;
    [SerializeField] float gravityCap = 3f;

    protected float shootTime;

    [SerializeField] protected GameObject cockPit;
    [SerializeField] protected GameObject shootOrigin;

    [Header("SoundS"), SerializeField] protected AudioSource shotSound;

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
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected void RotateTankFixed()
    {
        // Rotate the player smoothly, depending on the velocity
        if (moveDirection.x != 0 || moveDirection.y != 0)
        {

            Quaternion targetRotation = new Quaternion();
            targetRotation.SetLookRotation(moveDirection);

            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        // Rotate the guns on the ship depending on the input of the right stick
        if (aimDirection.magnitude > 0.1f && cockPit)
        {
            //gunObject.transform.forward = shootDirection;
            Quaternion targetRotation = new Quaternion();
            targetRotation.SetLookRotation(aimDirection);

            cockPit.transform.rotation = Quaternion.Lerp(cockPit.transform.rotation, targetRotation, cockPitRotationSpeed * Time.fixedDeltaTime);
        }
    }

    protected virtual void CalculateVelocity()
    {
        if (velocity.magnitude < topSpeed)
        {
            velocity += moveDirection * acceleration;
        }
        velocity = velocity * (1 - Time.fixedDeltaTime * drag);
    }

    protected void RotateTank()
    {
        // Rotate the player smoothly, depending on the velocity
        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            Quaternion targetRotation = new Quaternion();
            targetRotation.SetLookRotation(moveDirection);

            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Rotate the guns on the ship depending on the input of the right stick
        if (aimDirection.magnitude > 0.1f && cockPit)
        {
            //gunObject.transform.forward = shootDirection;
            Quaternion targetRotation = new Quaternion();
            targetRotation.SetLookRotation(aimDirection);

            cockPit.transform.rotation = Quaternion.Lerp(cockPit.transform.rotation, targetRotation, cockPitRotationSpeed * Time.deltaTime);
        }
    }

    protected virtual void Shoot()
    {
        if (shotSound)
        {
            shotSound.Play();
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
