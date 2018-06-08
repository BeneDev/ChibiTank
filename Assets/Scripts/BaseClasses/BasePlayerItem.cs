using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO maybe make items a scriptable object, like the upgrades as well
public class BasePlayerItem : MonoBehaviour {

    public string Name
    {
        get
        {
            return itemName;
        }
    }

    public Sprite Sprite
    {
        get
        {
            return itemSprite;
        }
    }

    [SerializeField] protected string itemName;
    [SerializeField] protected Sprite itemSprite;

    public virtual void UseItem()
    {
        // Here goes, what the Item does. The player just calls this function on his items in his item slots. 
        // The items then proceed to do what is implemented for them, without the player having to worry about it
    }

}
