using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script, which manages all the menus and is responsible for opening new menus and closing existing ones. Stores an array of all possible Menu kinds
/// </summary>
public class MenuManager : Singleton<MenuManager> {

    #region Properties

    public Stack<Menu> MenuStack
    {
        get
        {
            return menuStack;
        }
        set
        {
            menuStack = value;
        }
    }

    public Sprite SpriteUnderMouse
    {
        get
        {
            return spriteUnderMouse;
        }
        set
        {
            spriteUnderMouse = value;
        }
    }

    #endregion

    #region Private Fields

    private Stack<Menu> menuStack = new Stack<Menu>(); // The stack which stores all the open menus
    [SerializeField] private Menu[] menuPrefabs; // The array which stores all the known prefabs which can be shown

    Animator camAnim; // Stores the camera animator to make the camera zoom in when the player is in any menu
    PlayerInput input; // Stores the player input to react to it and show or close menus

    bool cameraIsInZoom = false; // Stores wether the camera is currently zoomed in or not

    Sprite spriteUnderMouse; // Stores the sprite under the mouse to get the name later to show that name in the mouse overlay menu

    #endregion

    #region Unity Messages

    // Get necessary Components
    private void Start()
    {
        input = GetComponent<PlayerInput>();
        camAnim = Camera.main.GetComponentInParent<Animator>();
    }

    // Closes the top menu if the player presses the cancel button or opens the root menu if no menu was open. Also controls the camera animator appropriately
    private void Update()
    {
        if(input)
        {
            if (input.Cancel)
            {
                if (menuStack.Count > 0)
                {
                    menuStack.Peek().OnBackPressed();
                    if(Time.timeScale == 0f)
                    {
                        Time.timeScale = 1f;
                    }
                }
                else
                {
                    PauseMenu.Show();
                }
            }
        }
        if(menuStack.Count > 0)
        {
            camAnim.enabled = true;
            if (!cameraIsInZoom)
            {
                camAnim.SetTrigger("ZoomIn");
                cameraIsInZoom = true;
            }
        }
        else
        {
            if(cameraIsInZoom)
            {
                camAnim.SetTrigger("ZoomOut");
                cameraIsInZoom = false;
            }
        }
    }

    #endregion

    #region Controls

    // Creates an instance of the right prefab of the given menu
    public void CreateInstance<T>() where T : Menu
    {
        var prefab = GetPrefab<T>();

        Instantiate(prefab, transform);
    }

    // Open a given menu and disable all menus beneath if the menu implements this behavior. Also sets the sorting order to the highest there is, always being one above all underlying menus
    public void OpenMenu(Menu instance)
    {
        if(menuStack.Count > 0)
        {
            if(instance.disableMenusUnderneath)
            {
                foreach (var menu in menuStack)
                {
                    CanvasGroup cg = menu.GetComponent<CanvasGroup>();
                    if(cg != null)
                    {
                        cg.interactable = false;
                    }
                    else
                    {
                        menu.gameObject.SetActive(false);
                    }
                    if(menu.disableMenusUnderneath)
                    {
                        break;
                    }
                }
            }

            Canvas topCanvas = instance.GetComponent<Canvas>();
            Canvas prevCanvas = menuStack.Peek().GetComponent<Canvas>();

            topCanvas.sortingOrder = prevCanvas.sortingOrder + 1;
        }

        menuStack.Push(instance);
    }

    // Closes a menu. Will log an error if there are no menus at all of if the chosen menu is not the top menu. Otherwise CloseTopMenu is called
    public void CloseMenu(Menu instance)
    {
        if(menuStack.Count <= 0)
        {
            Debug.LogErrorFormat("There are no menus to be closed. {0} cannot be closed.", instance.GetType());
            return;
        }

        if(menuStack.Peek() != instance)
        {
            Debug.LogErrorFormat("{0} is not the top menu. You can only close the top menu.", instance.GetType());
            return;
        }

        CloseTopMenu();
    }

    // Closes the top menu and acts properly, depending on if the menu should be destroyed when closed or not. Sets the underlying menu to active again, if there is one
    public void CloseTopMenu()
    {
        var instance = menuStack.Pop();

        if(instance.destroyWhenClosed)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance.gameObject.SetActive(false);
        }

        foreach(var menu in menuStack)
        {
            CanvasGroup cg = menu.GetComponent<CanvasGroup>();

            if(cg != null)
            {
                cg.interactable = true;
            }
            else
            {
                menu.gameObject.SetActive(true);
            }

            if(menu.disableMenusUnderneath)
            {
                break;
            }
        }
    }

    #endregion

    #region Helper

    // Get the prefab of the requested type out of the array of known prefabs
    private T GetPrefab<T>() where T : Menu
    {
        foreach(var menu in menuPrefabs)
        {
            var prefab = menu as T;
            if(prefab != null)
            {
                return prefab;
            }
        }

        throw new MissingReferenceException("No prefab of type" + typeof(T) + "found.");
    }

    #endregion

}
