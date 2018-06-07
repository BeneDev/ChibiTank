using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableTracksUpgrade : ScriptableObject
{
    public string upgradeName;
    public Sprite upgradeSprite;

    [Header("Attributes")] public float topSpeed = 1f;
    public float acceleration = 1f;
    public float rotationSpeed = 1f;
    public float mass = 1f;
}
