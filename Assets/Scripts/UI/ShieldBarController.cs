using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBarController : MonoBehaviour {

    [SerializeField] Text numberDisplay;
    [SerializeField] Slider barSlider;
    [SerializeField] GameObject[] displayingElements;

    PlayerController player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
        if(newMax <= 0 && displayingElements.Length > 0)
        {
            foreach(GameObject obj in displayingElements)
            {
                obj.SetActive(false);
            }
        }
        else if (newMax > 0 && displayingElements.Length > 0)
        {
            foreach (GameObject obj in displayingElements)
            {
                obj.SetActive(true);
            }
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
