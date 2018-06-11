using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseBarController : MonoBehaviour {

    public float CanvasAlpha
    {
        get
        {
            return canvasGroup.alpha;
        }
        set
        {
            if(canModifyAlphaOutside)
            {
                canvasGroup.alpha = value;
            }
        }
    }

    [SerializeField] protected Text numberDisplay;
    [SerializeField] protected Slider barSlider;
    protected CanvasGroup canvasGroup;

    protected PlayerController player;

    protected bool canModifyAlphaOutside = true;

    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    protected virtual void ChangeBarValue(int newValue)
    {
        barSlider.value = newValue;
    }

    protected virtual void ChangeBarMaximum(int newMax)
    {
        barSlider.maxValue = newMax;
    }

    public virtual void OnBarValueChanged(float newValue)
    {
        if (numberDisplay)
        {
            int displayValue = (int)newValue;
            numberDisplay.text = displayValue.ToString();
        }
    }

}
