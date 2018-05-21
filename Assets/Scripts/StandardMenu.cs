using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardMenu : Menu<StandardMenu> {

    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }
}
