using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableAttackCockPitUpgrade : ScriptableObject
{
    public string upgradeName;
    public Sprite upgradeSprite;

    [Header("Attributes")] public int attack = 1;
    public float fireRate = 1f;
    public float reloadSpeed = 1f;
    public int magazineSize = 5;
    public float shootKnockback = 1f;
    public float shootKnockbackDuration = 1f;
    public float mass = 1f;
}
