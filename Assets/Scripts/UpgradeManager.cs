using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the attributes of the player based on the upgrade he has equipped
/// </summary>
public class UpgradeManager : Singleton<UpgradeManager> {

    [SerializeField] BaseScriptableUpgrade[] upgrades;

    /// <summary>
    /// Returns a certain upgrade from the base set of upgrades
    /// </summary>
    /// <param name="kind"></param> kind 0 = attack | 1 = body | 2 = tracks
    /// <returns></returns>
    public T GetUpgrade<T>(string name) where T : BaseScriptableUpgrade
    {
        foreach(BaseScriptableUpgrade upg in upgrades)
        {
            var upgrade = upg as T;
            if(upgrade != null && upgrade.upgradeName == name)
            {
                return upgrade;
            }
        }
        throw new MissingReferenceException("No Upgrade of type " + typeof(T) + "with the name " + name + " found.");
    }

}