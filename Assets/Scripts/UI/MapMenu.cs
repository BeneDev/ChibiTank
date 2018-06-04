using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script controls the menu, which shows the map and the currently active quest
/// </summary>
public class MapMenu : Menu<MapMenu> {

    #region Helper Methods

    public static void Show()
    {
        Open();
        // Pause the game
        //Time.timeScale = 0f;
    }

    public static void Hide()
    {
        Close();
        // Unpause the game
        //Time.timeScale = 1f;
    }

    public void OnBackButton()
    {
        Hide();
    }

    #endregion

}
