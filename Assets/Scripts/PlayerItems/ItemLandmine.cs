using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemLandmine : BasePlayerItem
{
    [SerializeField] GameObject landminePrefab;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void UseItem()
    {
        if (landminePrefab && timesOfUseLeft > 0)
        {
            // TODO play landmine deploy sound
            Instantiate(landminePrefab, player.transform.position, player.transform.rotation);
            timesOfUseLeft--;
        }
        else
        {
            // TODO play general item out of usage times sound
        }
    }
}
