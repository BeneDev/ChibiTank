using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenu : Menu<SaveMenu> {

    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }

    public void OnSaveButtonClicked()
    {

    }

    public void OnCancelButtonClicked()
    {
        Close();
    }

}
