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
    protected int shield;

    #endregion

    #region Helper Methods

    // The actual function, which gets called by the projectiles
    public virtual void TakeDamage(int damage)
    {
        int actualDamage = damage;
        if (shield > 0)
        {
            actualDamage -= shield;
            shield -= damage;
        }
        if(shield < 0)
        {
            shield = 0;
        }
        if (actualDamage - defense > 1)
        {
            health -= actualDamage - defense;
        }
        else
        {
            health--;
        }
    }

    #endregion

}
