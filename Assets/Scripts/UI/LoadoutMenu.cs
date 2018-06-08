using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script controls the menu which shows the current loadout of the player, meaning currently equipped Upgrades and Items
/// </summary>
public class LoadoutMenu : Menu<LoadoutMenu> {

    PlayerController player;

    [SerializeField] Text attackName;
    [SerializeField] Text bodyName;
    [SerializeField] Text tracksName;
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
        if(attackImage && attackName)
        {
            attackName.text = player.AttackUpgrade.upgradeName;
            attackImage.sprite = player.AttackUpgrade.upgradeSprite;
        }
        if(bodyImage && bodyName)
        {
            bodyName.text = player.BodyUpgrade.upgradeName;
            bodyImage.sprite = player.BodyUpgrade.upgradeSprite;
        }
        if(tracksImage && tracksName)
        {
            tracksName.text = player.TracksUpgrade.upgradeName;
            tracksImage.sprite = player.TracksUpgrade.upgradeSprite;
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
