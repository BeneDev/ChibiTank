using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script controls the menu which shows the current loadout of the player, meaning currently equipped Upgrades and Items
/// </summary>
public class LoadoutMenu : Menu<LoadoutMenu> {

    #region Helper Methods

    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }

    public void OnBackButton()
    {
        Hide();
    }

    #endregion

}
