using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script which controls the gameover menu, providing the methods for the buttons shown to the player.
/// </summary>
public class GameoverMenu : Menu<GameoverMenu> {

    #region Helper Methods

    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    public void OnLoadSaveButtonClicked()
    {
        LoadMenu.Show();
    }

    #endregion

}
