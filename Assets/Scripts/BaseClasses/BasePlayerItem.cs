using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BasePlayerItem : ScriptableObject
{

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
    [SerializeField] protected int maxTimesOfUse = 1;
    protected int timesOfUseLeft = 1;
    protected bool isUnlocked = false;

    // Here goes, what the Item does. The player just calls this function on his items in his item slots. 
    // The items then proceed to do what is implemented for them, without the player having to worry about it
    public virtual void UseItem() { }

    public virtual void ResetItemUsageTimes()
    {
        timesOfUseLeft = maxTimesOfUse;
    }

}
