using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Menu<T> : Menu where T : Menu<T>{

	public static T Instance
    {
        get;
        private set;
    }

    #region Unity Messages

    protected virtual void Awake()
    {
        Instance = (T)this;
    }

    protected virtual void OnDestroy()
    {
        Instance = null;
    }

    #endregion

    #region Menu Controls

    protected static void Open()
    {
        if(Instance == null)
        {
            MenuManager.Instance.CreateInstance<T>();
        }
        else
        {
            Instance.gameObject.SetActive(true);
        }

        MenuManager.Instance.OpenMenu(Instance);
    }

    protected static void Close()
    {
        if(Instance == null)
        {
            Debug.LogErrorFormat("No menu of type {0} is currently open.", typeof(T));
            return;
        }

        MenuManager.Instance.CloseMenu(Instance);
    }

    public override void OnBackPressed()
    {
        Close();
    }

    #endregion

}

public abstract class Menu : MonoBehaviour
{
    public bool destroyWhenClosed = true;
    public bool disableMenusUnderneath = true;

    public abstract void OnBackPressed();
}
