using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverMenu : Menu<GameoverMenu> {

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

}
