using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableBodyUpgrade : ScriptableObject
{
    public string upgradeName;
    public Sprite upgradeSprite;

    [Header("Attributes")] public int health = 1;
    public int defense = 1;
    public float mass = 1f;
}
