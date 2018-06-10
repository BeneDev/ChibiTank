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
        // Subtract the shield, if there is some, from the damage
        if (shield > 0)
        {
            actualDamage -= shield;
            shield -= damage;
            // TODO give visual feedback about the fact, that the shield absorbed some damage
        }
        if(shield < 0)
        {
            shield = 0;
        }
        // If the shield tanked all damage, the character will not be damaged
        if (actualDamage <= 0)
        {
            return;
        }
        // Else the character will be damaged at least one point, if the defense is greater than the damage. 
        // Else the difference of the defense subtracted from the damage will be the damage, the character will suffer from
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
