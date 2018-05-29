using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Menu<T> : Menu where T : Menu<T>{

	public static T Instance
    {
        get;
        private set;
    }

    AudioSource aS;
    Animator anim;

    [SerializeField] GameObject[] buttonArray;

    #region Unity Messages

    protected virtual void Awake()
    {
        Instance = (T)this;
        aS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    protected virtual void OnEnable()
    {
        PlaySwoosh();
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
        Instance.OnBackPressed();
    }

    void CloseMenu()
    {
        if (Instance == null)
        {
            Debug.LogErrorFormat("No menu of type {0} is currently open.", typeof(T));
            return;
        }
        MenuManager.Instance.CloseMenu(Instance);
    }

    public override void OnBackPressed()
    {
        // Make the swoosh away animation trigger, which then closes the menu
        PlaySwoosh();
        if(anim)
        {
            anim.SetTrigger("Close");
        }
    }

    #endregion

    void PlaySwoosh()
    {
        if (aS)
        {
            aS.Play();
        }
    }

    public void OnCloseAnimation()
    {
        CloseMenu();
    }

}

public abstract class Menu : MonoBehaviour
{
    public bool destroyWhenClosed = true;
    public bool disableMenusUnderneath = true;

    public abstract void OnBackPressed();
}
