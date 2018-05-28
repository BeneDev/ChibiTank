using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour {

    protected int health;
    protected int defense;

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
}
