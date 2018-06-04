using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The script controlling the pause menu and providing it with all the methods for the buttons, showing the specific sub menus
/// </summary>
public class PauseMenu : Menu<PauseMenu> {

    Animator camAnim;

    #region Unity Messages

    protected override void Awake()
    {
        base.Awake();
        camAnim = Camera.main.GetComponentInParent<Animator>();
    }

    #endregion

    #region Helper Methods

    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }

    #endregion

    #region Button Methods

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

    #endregion

}
