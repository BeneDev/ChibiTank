using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableTracksUpgrade : ScriptableObject
{
    [SerializeField] string upgradeName;
    [SerializeField] Sprite upgradeSprite;

    [Header("Attributes"), SerializeField] float topSpeed = 1f;
    [SerializeField] float acceleration = 1f;
    [SerializeField] float rotationSpeed = 1f;
}
