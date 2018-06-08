using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableTracksUpgrade : BaseScriptableUpgrade
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

    // Put the wheel mesh in here
    public Mesh secondaryUpgradeMesh;

    [Header("Attributes")] public float topSpeed = 1f;
    public float acceleration = 1f;
    public float rotationSpeed = 1f;
}
