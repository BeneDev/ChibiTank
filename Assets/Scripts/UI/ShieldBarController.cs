using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBarController : MonoBehaviour {

    [SerializeField] Text numberDisplay;
    [SerializeField] Slider barSlider;
    CanvasGroup canvasGroup;

    PlayerController player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        player.OnShieldChanged += ChangeBarValue;
        player.OnMaxShieldChanged += ChangeBarMaximum;
    }

    void ChangeBarValue(int newValue)
    {
        barSlider.value = newValue;
    }

    void ChangeBarMaximum(int newMax)
    {
        barSlider.maxValue = newMax;
        if(newMax <= 0 && canvasGroup)
        {
            canvasGroup.alpha = 0f;
        }
        else if (newMax > 0 && canvasGroup)
        {
            canvasGroup.alpha = 1f;
        }
    }

    public void OnBarValueChanged(float newValue)
    {
        if(numberDisplay)
        {
            int displayValue = (int)newValue;
            numberDisplay.text = displayValue.ToString();
        }
    }
}
