using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBarController : BaseBarController {

    protected override void Awake()
    {
        base.Awake();
        player.OnShieldChanged += ChangeBarValue;
        player.OnMaxShieldChanged += ChangeBarMaximum;
    }

    protected override void ChangeBarMaximum(int newMax)
    {
        base.ChangeBarMaximum(newMax);
        if(newMax <= 0 && canvasGroup)
        {
            canvasGroup.alpha = 0f;
            canModifyAlphaOutside = false;
        }
        else if (newMax > 0 && canvasGroup)
        {
            canvasGroup.alpha = 1f;
            canModifyAlphaOutside = true;
        }
    }
}
