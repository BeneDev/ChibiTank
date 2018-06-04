using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// The script which controls the dialogue menu providing a function to set the text, checking if only the text has to change, if any other menus have to be shown or if the menu has to close
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class DialogueMenu : Menu<DialogueMenu>, IPointerEnterHandler, IPointerExitHandler
{

    #region Fields

    bool isOver = false;

    string[] dialogueActions;
    int dialogueIndex = 0;

    [SerializeField] Text text;

    PlayerInput input;

    #endregion

    #region Unity Messages

    protected override void Awake()
    {
        base.Awake();
        input = GetComponent<PlayerInput>();
    }

    // Reset the dialogue index, so the menu always starts with the first text to show even if it was only disabled and not destroyed before
    protected override void OnEnable()
    {
        dialogueIndex = 0;
    }

    // Checks if the player clicks or presses RB when over the menu and then check for the next text to show or next menu to open. If there is no next assignment, the menu closes
    private void Update()
    {
        // TODO make the control input independent of mouse cursor position and make the player use A to confirm as another option
        if (isOver && input.Shoot)
        {
            dialogueIndex++;
            if (dialogueActions.Length > dialogueIndex)
            {
                if (dialogueActions[dialogueIndex] != "SAVE")
                {
                    text.text = dialogueActions[dialogueIndex];
                }
                else
                {
                    Hide();
                    SaveMenu.Show();
                }
            }
            else
            {
                Hide();
            }
        }
    }

    #endregion

    #region Helper Methods

    public static DialogueMenu Show()
    {
        Open();
        return Instance;
    }

    public static void Hide()
    {
        Close();
    }

    // Set the string array of text to show
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

    #endregion

}
