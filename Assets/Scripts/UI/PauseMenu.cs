using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : Menu<PauseMenu> {

    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }

    public void OnOptionsButtonClicked()
    {
        OptionsMenu.Show();
    }

    public void OnMapButtonClicked()
    {
        MapMenu.Show();
    }

    public void OnStatusButtonClicked()
    {
        StatusMenu.Show();
    }

    public void OnLoadoutButtonClicked()
    {
        LoadoutMenu.Show();
    }

    public void OnSaveButtonClicked()
    {
        SaveMenu.Show();
    }

    public void OnQuitButtonClicked()
    {
        QuitConfirmationMenu.Show();
    }

}
