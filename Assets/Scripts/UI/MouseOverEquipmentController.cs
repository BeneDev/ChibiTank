using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseOverEquipmentController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    // TODO Fix bug, when player switches too fast between several images

    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuManager.Instance.SpriteUnderMouse = GetComponent<Image>().sprite;
        HoverOverMenu.Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MenuManager.Instance.SpriteUnderMouse = null;
        HoverOverMenu.Hide();
    }
}
