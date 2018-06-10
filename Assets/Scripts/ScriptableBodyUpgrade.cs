using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableBodyUpgrade : BaseScriptableUpgrade
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
            // Apply 10% to all the stats to make it the leveled up version of the stat
            health += (int)health / 10;
            defense += (int)defense / 10;
            shield += (int)shield / 10;
        }
    }

    [Header("Attributes")] public int health = 1;
    public int defense = 1;
    public int shield = 0;
}
