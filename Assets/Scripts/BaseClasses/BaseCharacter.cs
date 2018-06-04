using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Every acting GameObject has to inherit from this, as this script enables the certain GameObject to be damaged by projectiles
/// </summary>
public class BaseCharacter : MonoBehaviour {

    #region Fields

    protected int health;
    protected int defense;

    #endregion

    #region Helper Methods

    // The actual function, which gets called by the projectiles
    public virtual void TakeDamage(int damage)
    {
        if (damage - defense > 1)
        {
            health -= damage - defense;
        }
        else
        {
            health--;
        }
    }

    #endregion

}
