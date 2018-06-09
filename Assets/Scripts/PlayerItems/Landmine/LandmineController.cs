using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LandmineController : MonoBehaviour {

    [SerializeField] ParticleSystem explosionParticle;
    [SerializeField] int explosionDamage;

    float timeWhenPlanted;
    [SerializeField] float activationTime;
    bool isActive = false;

    private void Awake()
    {
        timeWhenPlanted = Time.realtimeSinceStartup;
        explosionParticle.gameObject.GetComponent<ExplosionController>().Damage = explosionDamage;
    }

    private void Update()
    {
        if(!isActive)
        {
            if(Time.realtimeSinceStartup > timeWhenPlanted + activationTime)
            {
                isActive = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isActive) { return; }
        if(other.gameObject.GetComponentInParent<BaseCharacter>())
        {
            if (!explosionParticle.isPlaying)
            {
                explosionParticle.Play();
            }
        }
    }

}
