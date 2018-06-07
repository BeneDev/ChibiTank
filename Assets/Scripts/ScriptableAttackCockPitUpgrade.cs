using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableAttackCockPitUpgrade : ScriptableObject
{
    [SerializeField] string upgradeName;
    [SerializeField] Sprite upgradeSprite;

    [Header("Attributes"), SerializeField] int attack = 1;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float reloadSpeed = 1f;
    [SerializeField] int magazineSize = 5;
    [SerializeField] float shootKnockback = 1f;
    [SerializeField] float shootKnockbackDuration = 1f;
}
