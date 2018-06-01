using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCover : MonoBehaviour {

    [SerializeField] protected int hitPoints;

    protected MeshRenderer[] meshRend;

    [SerializeField] protected ParticleSystem destructionDust;
    protected bool isExploded = false;

    private void Awake()
    {
        meshRend = GetComponentsInChildren<MeshRenderer>();
    }

    protected virtual void Update()
    {
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

    protected IEnumerator Disappear(float seconds)
    {
        foreach(MeshRenderer rend in meshRend)
        {
            rend.enabled = false;
        }
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    public virtual void TakeDamage(int damage)
    {
        hitPoints -= damage;
    }
}
