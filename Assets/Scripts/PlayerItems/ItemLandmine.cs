using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemLandmine : BasePlayerItem
{
    [SerializeField] GameObject landminePrefab;
    GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void UseItem()
    {
        if(landminePrefab)
        {
            Instantiate(landminePrefab, player.transform.position, player.transform.rotation);
        }
    }
}
