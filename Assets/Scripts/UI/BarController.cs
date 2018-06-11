using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour {

    [SerializeField] Text numberDisplay;
    [SerializeField] Slider barSlider;

    PlayerController player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        player.OnHealthChanged += ChangeBarValue;
        player.OnMaxHealthChanged += ChangeBarMaximum;
    }

    void ChangeBarValue(int newValue)
    {
        barSlider.value = newValue;
    }

    void ChangeBarMaximum(int newMax)
    {
        barSlider.maxValue = newMax;
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
