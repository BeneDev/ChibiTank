using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BaseScriptableUpgrade : ScriptableObject {

    public string upgradeName;
    public Sprite upgradeSprite; // This sprite should be of the dimensions 4 in width, 3 in height. I will go for 200 x 150 as standard
    public Mesh upgradeMesh;

    protected int level = 1;
    public bool isUnlocked = false;

    public float mass = 1f;
}
