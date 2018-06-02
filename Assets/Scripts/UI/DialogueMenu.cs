using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
public class DialogueMenu : Menu<DialogueMenu>, IPointerEnterHandler, IPointerExitHandler
{
    bool isOver = false;

    string[] dialogueActions;
    int dialogueIndex = 0;

    [SerializeField] Text text;

    PlayerInput input;

    public static DialogueMenu Show()
    {
        Open();
        return Instance;
    }

    public static void Hide()
    {
        Close();
    }

    protected override void Awake()
    {
        base.Awake();
        input = GetComponent<PlayerInput>();
        dialogueIndex = 0;
    }

    private void Update()
    {
        if(isOver && input.Shoot)
        {
            dialogueIndex++;
            if(dialogueActions.Length > dialogueIndex)
            {
                if(dialogueActions[dialogueIndex] != "SAVE")
                {
                    text.text = dialogueActions[dialogueIndex];
                }
                else
                {
                    SaveMenu.Show();
                }
            }
            else
            {
                Hide();
            }
        }
    }

    public void SetDialogueActions(string[] actions)
    {
        dialogueActions = actions;
        if (dialogueActions.Length > 0)
        {
            text.text = dialogueActions[dialogueIndex];
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOver = false;
    }
}
