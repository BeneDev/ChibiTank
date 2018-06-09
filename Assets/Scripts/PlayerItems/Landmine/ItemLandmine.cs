using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemLandmine : BasePlayerItem
{
    [SerializeField] GameObject landminePrefab;
    GameObject player; // This will always be empty. Works anyways... probably, because a null vector is used then

    public override void UseItem()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
