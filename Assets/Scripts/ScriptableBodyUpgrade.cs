using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableBodyUpgrade : ScriptableObject
{
    [SerializeField] string upgradeName;
    [SerializeField] Sprite upgradeSprite;

    [Header("Attributes"), SerializeField] int health = 1;
    [SerializeField] int defense = 1;
}
