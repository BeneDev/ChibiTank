using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour {

    [SerializeField] Vector2 hotSpotOffset;
    CanvasGroup ownCanvasGroup;
    [SerializeField] GameObject panel;

    private void Awake()
    {
        Cursor.visible = false;
        ownCanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if(!GameManager.Instance.IsCursorVisible)
        {
            ownCanvasGroup.alpha = 0f;
            return;
        }
        else
        {
            ownCanvasGroup.alpha = 1f;
        }
        if(panel)
        {
            panel.transform.position = Input.mousePosition;
        }
        else
        {
            Debug.LogError("There is no panel serialized for the Cursor Controller in the Inspector!");
        }
    }
}
