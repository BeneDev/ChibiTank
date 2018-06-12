using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : BaseBarController {

    protected override void Awake()
    {
        base.Awake();
        player.OnHealthChanged += ChangeBarValue;
        player.OnMaxHealthChanged += ChangeBarMaximum;
    }
}
