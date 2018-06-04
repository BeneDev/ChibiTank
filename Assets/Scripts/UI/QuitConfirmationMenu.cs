using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script controls the quit confirmation menu, which is shown, when the player wants to leave the game, to prevent quitting the game on accident
/// </summary>
public class QuitConfirmationMenu : Menu<QuitConfirmationMenu>
{

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

    public void OnNoButtonClicked()
    {
        Hide();
    }

    public void OnYesButtonClicked()
    {
        Application.Quit();
    }

    #endregion

}
