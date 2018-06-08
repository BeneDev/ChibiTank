using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableAttackCockPitUpgrade : BaseScriptableUpgrade
{
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            // TODO apply (10%?) bonus to all the stats because its a higher leveled version of the upgrade now
        }
    }

    // Put the barrel mesh in here
    public Mesh secondaryUpgradeMesh;

    [Header("Attributes")] public int attack = 1;
    public float fireRate = 1f;
    public float reloadSpeed = 1f;
    public int magazineSize = 5;
    public float shootKnockback = 1f;
    public float shootKnockbackDuration = 1f;
}
