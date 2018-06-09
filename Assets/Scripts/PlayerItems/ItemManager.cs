using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager> {

    [SerializeField] BasePlayerItem[] items;

    public T GetItem<T>() where T : BasePlayerItem
    {
        foreach (var it in items)
        {
            var item = it as T;
            if (item != null)
            {
                return item;
            }
        }

        throw new MissingReferenceException("No item of type " + typeof(T) + " found.");
    }

}
