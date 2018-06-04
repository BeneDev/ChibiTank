using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The script which controls the options menu, letting the player tweak settings regarding the graphics and also some gameplay elements
/// </summary>
public class OptionsMenu : Menu<OptionsMenu> {

    #region Fields

    PlayerController player; // Stores the player to gain access to the cockpit rotation speed value

    // TODO dont make the player control this value
    [SerializeField] Slider cockPitRotationSpeedSlider; // Stores the rotation speed slider, to get the value for the player field 
    [SerializeField] Toggle controllerInputToggle; // Stores the toggle for controller input to get the data for the gameManager

    Dictionary<int, int[]> resolutionDict = new Dictionary<int, int[]>(); // Stores some of the resolution values mapped to integer keys, to call the resolutions from a dropdown menu

    #endregion

    #region Unity Messages

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

    public void OnBackButtonPressed()
    {
        Hide();
    }

    // Set the player cockpit rotaion speed value based on the slider value
    public void OnTurretRotationspeedSliderChange()
    {
        if (cockPitRotationSpeedSlider)
        {
            player.CockPitRotationSpeed = cockPitRotationSpeedSlider.value;
        }
    }

    // Set the Graphics quality based on the dropdown menu value
    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.LogFormat("Quality Settings has been changed to {0}", qualityIndex);
    }

    // Set the Game Manager field, which stores, if the player wants to play with controller or not
    public void OnControllerToggleChanged(bool value)
    {
        GameManager.Instance.IsControllerInput = value;
        if (controllerInputToggle)
        {
            controllerInputToggle.isOn = GameManager.Instance.IsControllerInput;
        }
    }

    // Make the game run in fullscreen or not, depending on the value
    public void OnFullscreenToggleChanged(bool value)
    {
        // TODO bug with the game not going into windowed mode again after being in fullscreen mode once
        Screen.fullScreen = value;
    }

    // Set the resolution based on the value of the dropdown menu
    public void SetResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutionDict[resolutionIndex][0], resolutionDict[resolutionIndex][1], Screen.fullScreen);
    }

    #endregion


}
