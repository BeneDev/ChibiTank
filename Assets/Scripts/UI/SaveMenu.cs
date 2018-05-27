using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenu : Menu<SaveMenu> {

    public static void Show()
    {
        Open();
        // Pause the game
        Time.timeScale = 0f;
    }

    public static void Hide()
    {
        // Unpause the game
        Time.timeScale = 1f;
        Close();
    }

    public void OnSaveButtonClicked()
    {
        //TODO save the game somehow
        GameManager.Instance.RespawnPoint = GameObject.FindGameObjectWithTag("Player").transform.position;
        Hide();
    }

    public void OnCancelButtonClicked()
    {
        Hide();
    }

}
