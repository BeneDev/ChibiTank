using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMenu : Menu<LoadMenu> {

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

    public void OnLoadButtonClicked()
    {
        //TODO load game somehow
        GameObject.FindGameObjectWithTag("Player").transform.position = GameManager.Instance.RespawnPoint;
        Hide();
    }

    public void OnCancelButtonClicked()
    {
        Hide();
    }
}
