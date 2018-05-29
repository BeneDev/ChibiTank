using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseOverEquipmentController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    
    bool isOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuManager.Instance.SpriteUnderMouse = GetComponent<Image>().sprite;
        isOver = true;
        if (HoverOverMenu.Instance == null)
        {
            HoverOverMenu.Show();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MenuManager.Instance.SpriteUnderMouse = null;
        isOver = false;
        if (HoverOverMenu.Instance != null)
        {
            HoverOverMenu.Hide();
        }
    }

    private void Update()
    {
        if(isOver && HoverOverMenu.Instance == null)
        {
            HoverOverMenu.Show();
        }
    }
}
