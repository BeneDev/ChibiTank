using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is an immutable, to prevent the upgrade from changing its values after it was initialised
/// </summary>
public class BaseUpgrade : MonoBehaviour {

    //TODO make base classes of upgrades for offensive, defensive and acceleration based upgrades

    public int Damage
    {
        get
        {
            return damage;
        }
    }

    private readonly int damage;

    public BaseUpgrade(int damage)
    {
        this.damage = damage;
    }
}
