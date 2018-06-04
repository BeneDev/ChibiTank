using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The script which controls the HoverOver Menu for certain Equipment. When the player hovers over the item, this menu gets shown as a mouse overlay
/// </summary>
public class HoverOverMenu : Menu<HoverOverMenu> {

    #region Fields

    [SerializeField] Text objectName; // Stores the UI Text Object, which shows the name of the item, which the player currently hovers over, to change the name of the Item properly
    [SerializeField] GameObject panel; // Stores the panel, which shows the Text, to make it follow the mouse cursor with a given offset
    [SerializeField] Vector3 offset = Vector3.zero; // The offset relative to the mouse cursor

    #endregion

    #region Helper Mehtods

    // When the menu is enabled, the right Item name at the top gets shown
    protected override void OnEnable()
    {
        base.OnEnable();
        if(objectName && MenuManager.Instance.SpriteUnderMouse)
        {
            objectName.text = MenuManager.Instance.SpriteUnderMouse.name;
        }
    }

    // Follow the mouse cursor of the player
    private void Update()
    {
        if(panel)
        {
            panel.transform.position = Input.mousePosition + offset;
        }
    }

    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }

    #endregion

}
