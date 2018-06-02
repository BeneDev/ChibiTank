using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : Menu<PauseMenu> {

    Animator camAnim;

    protected override void Awake()
    {
        base.Awake();
        camAnim = Camera.main.GetComponentInParent<Animator>();
    }

    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }

    public void OnBackButtonClicked()
    {
        Hide();
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
