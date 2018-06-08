using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableTracksUpgrade : BaseScriptableUpgrade
{
    [Header("Attributes")] public float topSpeed = 1f;
    public float acceleration = 1f;
    public float rotationSpeed = 1f;
}
