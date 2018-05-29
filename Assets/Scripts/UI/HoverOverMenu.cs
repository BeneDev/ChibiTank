using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverOverMenu : Menu<HoverOverMenu> {

    [SerializeField] Text objectName;
    [SerializeField] GameObject panel;
    [SerializeField] Vector3 offset = Vector3.zero;

    protected override void OnEnable()
    {
        base.OnEnable();
        if(objectName)
        {
            objectName.text = "";
        }
    }

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
}
