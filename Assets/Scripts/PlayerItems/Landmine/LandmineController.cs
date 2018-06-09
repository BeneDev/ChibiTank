using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LandmineController : MonoBehaviour {

    [SerializeField] ParticleSystem explosionParticle;
    [SerializeField] int explosionDamage;
    [SerializeField] AudioSource explosionAudioSource;
    [SerializeField] float exploShakeAmount;
    [SerializeField] float exploShakeDuration;

    float timeWhenPlanted;
    [SerializeField] float activationTime;
    bool isActive = false;

    CameraShake camShake;

    private void Awake()
    {
        timeWhenPlanted = Time.realtimeSinceStartup;
        explosionParticle.gameObject.GetComponent<ExplosionController>().Damage = explosionDamage;
        camShake = Camera.main.GetComponent<CameraShake>();
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
        if(other.gameObject.GetComponentInParent<BaseCharacter>() && !other.isTrigger)
        {
            if (!explosionParticle.isPlaying)
            {
                explosionParticle.Play();
                if (!explosionAudioSource.isPlaying)
                {
                    explosionAudioSource.Play();
                }
                camShake.shakeAmount = exploShakeAmount;
                camShake.shakeDuration = exploShakeDuration;
            }
        }
    }

}
