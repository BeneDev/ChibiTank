using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitConfirmationMenu : Menu<QuitConfirmationMenu>
{
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
}
