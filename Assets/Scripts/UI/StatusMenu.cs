using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusMenu : Menu<StatusMenu> {

    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }

    public void OnBackButton()
    {
        Hide();
    }

}
