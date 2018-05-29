using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverEquipmentController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    bool isOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("is over");
        isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("leaves");
        isOver = false;
    }
}
