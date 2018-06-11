using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : BaseBarController {

    private void Start()
    {
        player.OnHealthChanged += ChangeBarValue;
        player.OnMaxHealthChanged += ChangeBarMaximum;
    }
}
