using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script controls the menu which shows the current loadout of the player, meaning currently equipped Upgrades and Items
/// </summary>
public class LoadoutMenu : Menu<LoadoutMenu> {

    PlayerController player;

    [SerializeField] Sprite noItemSprite;

    [Header("Display Fields"), SerializeField] Text attackName;
    [SerializeField] Text bodyName;
    [SerializeField] Text tracksName;
    [SerializeField] Image attackImage;
    [SerializeField] Image bodyImage;
    [SerializeField] Image tracksImage;
    [SerializeField] Image item1Image;
    [SerializeField] Image item2Image;
    [SerializeField] Image item3Image;


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
        if(item1Image && player.EquippedItem1)
        {
            item1Image.sprite = player.EquippedItem1.Sprite;
        }
        else
        {
            item1Image.sprite = noItemSprite;
        }
        if (item2Image && player.EquippedItem2)
        {
            item1Image.sprite = player.EquippedItem2.Sprite;
        }
        else
        {
            item1Image.sprite = noItemSprite;
        }
        if (item3Image && player.EquippedItem3)
        {
            item1Image.sprite = player.EquippedItem3.Sprite;
        }
        else
        {
            item1Image.sprite = noItemSprite;
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
