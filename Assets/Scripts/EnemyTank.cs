using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script to control the enemy tank, taking advantage of the baseTank script
/// </summary>
public class EnemyTank : BaseTank {

    #region Fields

    GameObject player;
    Vector3 toPlayer;
    bool bAimingAtPlayer = false; // Used to check if the enemy aims rightfully at the player, to check if shooting would make sense
    float timeWhenPlayerLeftSight; // The time, the player left the enemy sight

    [SerializeField] float patrolRadius = 10f; // The radius, the enemy will search a new position to walk towards in
    [Range(0, 1), SerializeField] float patrolSpeedMultiplier = 0.3f; // The speed the enemy walks when only patroling

    Vector3 targetPosition; // The position the enemy will walk towards
    bool hasTarget = false; // stores if the enemy has to find a new target position

    Vector3 pointWherePlayerLastSpotted; // The position the player was last spotted to be in the sight of the enemy

    [SerializeField] float searchPlayerDuration = 3f; // How long the enemy will search for the player after he lost sight

    [SerializeField] float sightReach; // How far the enemy can see the player
    [SerializeField] float sightReachMultiplier; // This value will get multiplied to the sight reach, making up the distance, when the enemy communicates, that he is nearby the player
    [SerializeField] float aimingTolerance = 0.1f; // The difference between the actual aiming rotation and where the player is, to check if shooting makes sense

    // The possible states and the field to store the current state for the enemy AI State Machine
    enum EnemyState
    {
        patroling,
        playerSpotted,
        searchingForPlayer
    };
    EnemyState state = EnemyState.patroling;

    #endregion

    #region Unity Messages

    // Reinitialise the Attributes from the baseTank class with the serialized values from this script
    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Change the state of the enemy accordingly and react to the new state 
    protected override void FixedUpdate()
    {
        toPlayer = player.transform.position - transform.position;
        if(isDead)
        {
            // TODO Let the player control this value in options menu
            if(toPlayer.magnitude > 150f)
            {
                Destroy(gameObject);
            }
            return;
        }
        if(!isDead && toPlayer.magnitude < sightReach * sightReachMultiplier && !GameManager.Instance.enemiesNearbyPlayer.Contains(gameObject))
        {
            GameManager.Instance.enemiesNearbyPlayer.Add(gameObject);
        }
        else if (isDead || toPlayer.magnitude > sightReach * sightReachMultiplier && GameManager.Instance.enemiesNearbyPlayer.Contains(gameObject))
        {
            GameManager.Instance.enemiesNearbyPlayer.Remove(gameObject);
        }
        if (state == EnemyState.patroling)
        {
            acceleration = baseAcceleration * patrolSpeedMultiplier;
            if (toPlayer.magnitude < sightReach && !player.GetComponent<PlayerController>().IsDead)
            {
                state = EnemyState.playerSpotted;
            }
            if(!hasTarget)
            {
                Vector2 relativePoint = Random.insideUnitCircle * patrolRadius;
                targetPosition = transform.position + new Vector3(relativePoint.x, 0f, relativePoint.y);
                hasTarget = true;
            }
            MoveTo(targetPosition);
            if(V3Equal(transform.position, targetPosition))
            {
                hasTarget = false;
            }
        }
        else if(state == EnemyState.playerSpotted)
        {
            acceleration = baseAcceleration;
            if(player.GetComponent<PlayerController>().IsDead)
            {
                state = EnemyState.patroling;
            }
            if(toPlayer.magnitude < sightReach - 5f)
            {
                Attack();
            }
            else if(toPlayer.magnitude < sightReach + 5f)
            {
                MoveTo(player.transform.position);
            }
            else
            {
                state = EnemyState.searchingForPlayer;
                pointWherePlayerLastSpotted = player.transform.position;
                timeWhenPlayerLeftSight = Time.realtimeSinceStartup;
            }
        }
        else if(state == EnemyState.searchingForPlayer)
        {
            acceleration = baseAcceleration;
            MoveTo(pointWherePlayerLastSpotted);
            if(toPlayer.magnitude < sightReach && !player.GetComponent<PlayerController>().IsDead)
            {
                state = EnemyState.playerSpotted;
            }
            if(Time.realtimeSinceStartup > timeWhenPlayerLeftSight + searchPlayerDuration)
            {
                state = EnemyState.patroling;
            }
        }
        base.FixedUpdate();
    }

    #endregion

    #region Helper Methods

    // Overrides the takeDamage method, making the enemy rush towards the position, the shot was fired from
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        pointWherePlayerLastSpotted = player.transform.position;
        timeWhenPlayerLeftSight = Time.realtimeSinceStartup;
        state = EnemyState.searchingForPlayer;
    }

    // Try to aim at the player and if the barrel finally arrives at roughly the right orientation, the enemy begins to shoot
    private void Attack()
    {
        aimDirection = toPlayer.normalized;
        RotateTurret();
        if (V3Equal(cockPit.transform.forward.normalized, aimDirection.normalized))
        {
            bAimingAtPlayer = true;
        }
        else
        {
            bAimingAtPlayer = false;
        }
        if (Time.realtimeSinceStartup > shootTime + fireRate && bAimingAtPlayer)
        {
            Shoot();
        }
    }

    // Make the enemy move towards the given point
    void MoveTo(Vector3 point)
    {
        // Move to the point
        Vector3 toPoint = point - transform.position;
        aimDirection = toPoint.normalized;
        moveDirection = toPoint.normalized;
        RotateBody();
        CalculateVelocity();
    }

    // Check if two vectors are the same taking into account a small margin, in this case, the aimingtolerance
    bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < aimingTolerance;
    }

    #endregion

}
