using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the attributes of the player based on the upgrade he has equipped
/// </summary>
public class UpgradeManager : Singleton<UpgradeManager> {

    [SerializeField] ScriptableObject[] baseUpgrades;

    // kind 0 = attack | 1 = body | 2 = tracks

    public ScriptableObject GetBaseUpgrade(int kind)
    {
        return baseUpgrades[kind];
    }

}