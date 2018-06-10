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
    [SerializeField] Light pointLight;
    float lastTimeLightFlash;
    [SerializeField] float flashOffset = 1f;
    [SerializeField] float flashDuration = 0.2f;
    [SerializeField] AudioSource beepAudioSource;
    bool beepPlayed = false;

    MeshRenderer[] allMeshRenderer;

    float timeWhenPlanted;
    [SerializeField] float activationTime;
    bool isActive = false;
    bool isExploded = false;

    CameraShake camShake;

    private void Awake()
    {
        timeWhenPlanted = Time.realtimeSinceStartup;
        explosionParticle.gameObject.GetComponent<ExplosionController>().Damage = explosionDamage;
        camShake = Camera.main.GetComponent<CameraShake>();
        allMeshRenderer = GetComponentsInChildren<MeshRenderer>();
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
        if(pointLight && Time.realtimeSinceStartup > lastTimeLightFlash + flashOffset && isActive && !isExploded)
        {
            StartCoroutine(FlashUp(flashDuration));
        }
    }

    IEnumerator FlashUp(float seconds)
    {
        if(beepAudioSource && !beepPlayed)
        {
            beepAudioSource.Play();
            beepPlayed = true;
        }
        pointLight.enabled = true;
        yield return new WaitForSeconds(seconds);
        pointLight.enabled = false;
        lastTimeLightFlash = Time.realtimeSinceStartup;
        beepPlayed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isActive) { return; }
        if(other.gameObject.GetComponentInParent<BaseCharacter>() && !other.isTrigger)
        {
            if (!explosionParticle.isPlaying)
            {
                foreach(MeshRenderer rend in allMeshRenderer)
                {
                    rend.enabled = false;
                }
                isExploded = true;
                explosionParticle.Play();
                if (!explosionAudioSource.isPlaying)
                {
                    explosionAudioSource.Play();
                    StartCoroutine(DeleteAfterExplosion(explosionParticle.main.duration));
                }
                camShake.shakeAmount = exploShakeAmount;
                camShake.shakeDuration = exploShakeDuration;
            }
        }
    }

    IEnumerator DeleteAfterExplosion(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

}
