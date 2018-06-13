using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameTimeUIController : MonoBehaviour {

    [SerializeField] Text timeText;

    private void Start()
    {
        GameManager.Instance.OnInGameTimeChanged += ChangeTimeDisplay;
    }

    void ChangeTimeDisplay(int hour, int minute)
    {
        string timeString = "";
        timeString += (hour < 10) ? "0" + hour : "" + hour;
        timeString += " : ";
        timeString += (minute < 10) ? "0" + minute : "" + minute;
        timeText.text = timeString;
    }

}
