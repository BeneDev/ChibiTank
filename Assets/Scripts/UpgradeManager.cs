using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the attributes of the player based on the upgrade he has equipped
/// </summary>
public class UpgradeManager : Singleton<UpgradeManager> {

    [SerializeField] ScriptableObject[] baseUpgrades;

    /// <summary>
    /// Returns a certain upgrade from the base set of upgrades
    /// </summary>
    /// <param name="kind"></param> kind 0 = attack | 1 = body | 2 = tracks
    /// <returns></returns>
    public ScriptableObject GetBaseUpgrade(int kind)
    {
        return baseUpgrades[kind];
    }

}