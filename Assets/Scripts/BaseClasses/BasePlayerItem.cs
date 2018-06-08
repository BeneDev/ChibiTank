using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
