using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BaseScriptableUpgrade : ScriptableObject {

    public string upgradeName;
    public Sprite upgradeSprite;
    public Mesh upgradeMesh;

    protected int level = 1;
    public bool isUnlocked = false;

    public float mass = 1f;
}
