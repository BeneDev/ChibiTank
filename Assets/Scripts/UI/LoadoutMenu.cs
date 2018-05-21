using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutMenu : Menu<LoadoutMenu> {

    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }
}
