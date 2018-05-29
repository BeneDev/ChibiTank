using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverEquipmentController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    bool isOver = false;

    // TODO Fix bug, when player switches too fast between several images

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverOverMenu.Show();
        isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverOverMenu.Hide();
        isOver = false;
    }
}
