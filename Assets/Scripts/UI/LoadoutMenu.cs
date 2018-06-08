using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script controls the menu which shows the current loadout of the player, meaning currently equipped Upgrades and Items
/// </summary>
public class LoadoutMenu : Menu<LoadoutMenu> {

    PlayerController player;

    [SerializeField] Image attackImage;
    [SerializeField] Image bodyImage;
    [SerializeField] Image tracksImage;


    #region Unity Messages

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if(attackImage)
        {
            attackImage.sprite = player.AttackUpgradeSprite;
        }
        if(bodyImage)
        {
            bodyImage.sprite = player.BodyUpgradeSprite;
        }
        if(tracksImage)
        {
            tracksImage.sprite = player.TracksUpgradeSprite;
        }
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

    public void OnBackButton()
    {
        Hide();
    }

    #endregion

}
