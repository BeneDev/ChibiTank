using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : Menu<OptionsMenu> {

    PlayerController player;

    [SerializeField] Slider cockPitRotationSpeedSlider;
    [SerializeField] Toggle controllerInputToggle;

    protected override void OnEnable()
    {
        base.OnEnable();
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

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.LogFormat("Quality Settings has been changed to {0}", qualityIndex);
    }

    public void OnControllerToggleChanged(bool value)
    {
        GameManager.Instance.IsControllerInput = value;
        if (controllerInputToggle)
        {
            controllerInputToggle.isOn = GameManager.Instance.IsControllerInput;
        }
    }

}
