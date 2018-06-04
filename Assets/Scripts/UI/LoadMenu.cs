using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script which controls the Load Menu. Here the player can load specific savestates.
/// </summary>
public class LoadMenu : Menu<LoadMenu> {

    #region Helper Methods

    public static void Show()
    {
        Open();
        // Pause the game
        Time.timeScale = 0f;
    }

    public static void Hide()
    {
        // Unpause the game
        Time.timeScale = 1f;
        Close();
    }

    // Loads the currently chosen savestate, which there is currently only one at a time
    public void OnLoadButtonClicked()
    {
        SaveFileManager.LoadGame();
        Hide();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ResetPlayerTank();
    }

    public void OnCancelButtonClicked()
    {
        Hide();
    }

    #endregion

}
