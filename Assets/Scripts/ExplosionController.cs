using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour {

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

    [SerializeField] float explosionRange = 1f;
    [SerializeField] float explosionDamageDelay = 1f;
    [SerializeField] float damageHitboxStayDuration = 1f; // This has to be bigger than explosionDamageDelay, or else, it wont do any damage at all
    int damage = 3;
    float timeWhenStarted = -100f;

    private void Update()
    {
        if(!GetComponent<ParticleSystem>().isPlaying) { return; }
        if(timeWhenStarted < 0)
        {
            timeWhenStarted = Time.realtimeSinceStartup;
        }
        if (Time.realtimeSinceStartup >= timeWhenStarted + explosionDamageDelay && Time.realtimeSinceStartup < timeWhenStarted + damageHitboxStayDuration)
        {
            Collider[] charactersInExplosionRange = Physics.OverlapSphere(transform.position, explosionRange);
            if (charactersInExplosionRange.Length > 0)
            {
                foreach (var obj in charactersInExplosionRange)
                {
                    if (obj.gameObject.GetComponent<BaseCharacter>())
                    {
                        obj.gameObject.GetComponent<BaseCharacter>().TakeDamage(damage);
                    }
                }
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, explosionRange);
    //}

}
