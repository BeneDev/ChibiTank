using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

    [SerializeField] Texture2D cursorTexture;
    Vector2 hotSpot;

    private void Awake()
    {
        if (cursorTexture)
        {
            hotSpot = new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2f);
            Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.ForceSoftware);
        }
    }
}
