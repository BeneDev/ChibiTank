using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitConfirmationMenu : Menu<QuitConfirmationMenu>
{
    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
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
