using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : Menu<OptionsMenu> {

    PlayerController player;

    [SerializeField] Slider cockPitRotationSpeedSlider;
    [SerializeField] Toggle controllerInputToggle;

    Dictionary<int, int[]> resolutionDict = new Dictionary<int, int[]>();

    protected override void Awake()
    {
        base.Awake();
        resolutionDict.Add(0, new int[] { 640, 480 });
        resolutionDict.Add(1, new int[] { 800, 600 });
        resolutionDict.Add(2, new int[] { 1280, 720 });
        resolutionDict.Add(3, new int[] { 1360, 768 });
        resolutionDict.Add(4, new int[] { 1600, 900 });
        resolutionDict.Add(5, new int[] { 1920, 1080 });
    }

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

    public void OnFullscreenToggleChanged(bool value)
    {
        Screen.fullScreen = value;
    }

    public void SetResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutionDict[resolutionIndex][0], resolutionDict[resolutionIndex][1], Screen.fullScreen);
    }

}
