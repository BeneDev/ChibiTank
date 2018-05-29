using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMenu : Menu<MapMenu> {

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

}
