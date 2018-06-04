using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Every Cover in the Level Design inherits from this script, as this enables those certain GameObjects to take damage and dissolve into dust when their hitpoints fall to zero.
/// </summary>
public class BaseCover : MonoBehaviour {

    #region Fields

    [SerializeField] protected int hitPoints;

    protected MeshRenderer[] meshRend; // All the meshrenderer, to disable them during the dust explosion so that the player does not see them anymore, but the particle system is stilly played correctly

    [SerializeField] protected ParticleSystem destructionDust; // The particle System, which gets played when the Cover vanishes
    protected bool isExploded = false; // The boolean to assure that the dust explosion only gets played one time and does not start several times

    #endregion

    #region Unity Messages

    private void Awake()
    {
        // Get all the mesh renderer on the object
        meshRend = GetComponentsInChildren<MeshRenderer>();
    }

    protected virtual void Update()
    {
        // Play the dust explosion, disappear and get destroyed after the dust explosion is finished, when the hitpoints fall to 0 or below
        if(hitPoints <= 0)
        {
            if(destructionDust)
            {
                if (!isExploded)
                {
                    destructionDust.Play();
                    isExploded = true;
                }
                StartCoroutine(Disappear(destructionDust.main.duration));
            }
        }
    }

    #endregion

    #region Helper Methods

    // Make the cover disappear for the duration of the dustexplosion and get destroyed after that
    protected IEnumerator Disappear(float seconds)
    {
        foreach(MeshRenderer rend in meshRend)
        {
            rend.enabled = false;
        }
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    // This function is called by the projectiles, to damage the cover and ultimately destroy it
    public virtual void TakeDamage(int damage)
    {
        hitPoints -= damage;
    }

    #endregion

}
