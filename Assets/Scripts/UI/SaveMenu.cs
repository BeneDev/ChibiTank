using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script controls the menu which lets the player save his current progress in the game
/// </summary>
public class SaveMenu : Menu<SaveMenu> {

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

    // Save the game and set the respawn point(obsolete) for the player. This is called when the save button is pressed
    public void OnSaveButtonClicked()
    {
        // TODO remove the respawn point system
        SaveFileManager.SaveGame();
        GameManager.Instance.RespawnPoint = GameObject.FindGameObjectWithTag("Player").transform.position;
        Hide();
    }

    public void OnCancelButtonClicked()
    {
        Hide();
    }

    #endregion

}
