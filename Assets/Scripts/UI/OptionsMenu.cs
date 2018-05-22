using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : Menu<OptionsMenu> {

    PlayerController player;

    [SerializeField] Slider cockPitRotationSpeedSlider;

    private void OnEnable()
    {
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

    public void OnBackButtonPressed()
    {
        Hide();
    }

    public void OnTurretRotationspeedSliderChange()
    {
        if (cockPitRotationSpeedSlider)
        {
            player.CockPitRotationSpeed = cockPitRotationSpeedSlider.value;
        }
    }

}
