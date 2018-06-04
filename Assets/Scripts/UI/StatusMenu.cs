using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The script which controls the status menu, making it show all the relevant player stats
/// </summary>
public class StatusMenu : Menu<StatusMenu> {

    #region Fields

    [SerializeField] Text[] numberTexts; // The UI Text objects responsible for showing the numbers of the player Attributes
    PlayerController player;

    #endregion

    #region Unity Messages

    // Get the player reference
    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Show the currently appropriate numbers for the player Attributes
    protected override void OnEnable()
    {
        base.OnEnable();
        numberTexts[0].text = "" + player.Level;
        numberTexts[1].text = "" + player.Attack;
        numberTexts[2].text = "" + player.FireRate;
        numberTexts[3].text = "" + player.ReloadSpeed;
        numberTexts[4].text = "" + player.KnockBack;
        numberTexts[5].text = "" + player.Defense;
        numberTexts[6].text = "" + player.TopSpeed;
        numberTexts[7].text = "" + player.Acceleration;
        numberTexts[8].text = "" + player.RotationSpeed;
        numberTexts[9].text = "" + player.Mass;
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

    public void OnBackButton()
    {
        Hide();
    }

    #endregion

}
