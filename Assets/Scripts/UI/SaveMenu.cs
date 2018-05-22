using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenu : Menu<SaveMenu> {

    public static void Show()
    {
        Open();
        // Pause the game
        Time.timeScale = 0f;
    }

    public static void Hide()
    {
        Close();
        // Unpause the game
        Time.timeScale = 1f;
    }

    public void OnSaveButtonClicked()
    {

    }

    public void OnCancelButtonClicked()
    {
        Close();
    }

}
