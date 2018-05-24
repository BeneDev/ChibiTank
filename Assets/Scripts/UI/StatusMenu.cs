using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusMenu : Menu<StatusMenu> {

    [SerializeField] Text[] numberTexts;
    PlayerController player;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

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

}
