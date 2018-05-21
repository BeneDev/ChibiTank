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
        
    }

    public void OnMapButtonClicked()
    {

    }

    public void OnQuestsButtonClicked()
    {

    }

    public void OnStatusButtonClicked()
    {

    }

    public void OnLoadoutButtonClicked()
    {

    }

    public void OnSaveButtonClicked()
    {

    }

    public void OnQuitButtonClicked()
    {
        QuitConfirmationMenu.Show();
    }

}
