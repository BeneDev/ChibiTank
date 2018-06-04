using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// The script which controls the images which can be hovered over and show a mouse overlay menu
/// </summary>
public class MouseOverEquipmentController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    
    bool isOver = false;

    #region Unity Messages

    private void Update()
    {
        if (isOver && HoverOverMenu.Instance == null)
        {
            HoverOverMenu.Show();
        }
    }

    #endregion

    #region Helper Methods

    // Shows the mouse overlay when the mouse is over the image
    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuManager.Instance.SpriteUnderMouse = GetComponent<Image>().sprite;
        isOver = true;
        if (HoverOverMenu.Instance == null)
        {
            HoverOverMenu.Show();
        }
    }

    // Hides the mouse overlay when the mouse leaves the image
    public void OnPointerExit(PointerEventData eventData)
    {
        MenuManager.Instance.SpriteUnderMouse = null;
        isOver = false;
        if (HoverOverMenu.Instance != null)
        {
            HoverOverMenu.Hide();
        }
    }

    #endregion

}
