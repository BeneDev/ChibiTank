using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager> {

    #region Private Fields

    private Stack<Menu> menuStack = new Stack<Menu>();
    [SerializeField] private Menu[] menuPrefabs;

    #endregion

    #region Unity Messages

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(menuStack.Count > 0)
            {
                menuStack.Peek().OnBackPressed();
            }
            else
            {
                PauseMenu.Show();
            }
        }
    }

    #endregion

    #region Controls

    public void CreateInstance<T>() where T : Menu
    {
        var prefab = GetPrefab<T>();

        Instantiate(prefab, transform);
    }

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
