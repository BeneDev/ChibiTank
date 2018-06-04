using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base menu container, which makes sure there is always only one instance of any kind of menu and provides methods, which all menus need
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Menu<T> : Menu where T : Menu<T>{

    #region Properties

    public static T Instance
    {
        get;
        private set;
    }

    #endregion

    #region Fields

    AudioSource aS; // The audio source to play the swoosh sound for showing and hiding the menu
    Animator anim; // The animator to play the opening and closing animations

    // TODO make controller menu support
    [SerializeField] GameObject[] buttonArray; // All the buttons the menu has. Currently unused. Planned usage is for controller Menu Support

    #endregion

    #region Unity Messages

    // Get the necessary references
    protected virtual void Awake()
    {
        Instance = (T)this;
        aS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    // Play the swoosh Animation everytime the menu gets opened
    protected virtual void OnEnable()
    {
        PlaySwoosh();
    }

    // Set the instance to null again to make space for the next instance of this kind of menu
    protected virtual void OnDestroy()
    {
        Instance = null;
    }

    #endregion

    #region Menu Controls

    // Open the menu, creating a new instance if there is no currently open menu, and setting the currently open, but disabled menu to being enabled if there is one
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

    // Closes the Menu, calling the OnBackPressed Method, which makes sure, that the close trigger of the animator gets called no matter how the menu was closed
    protected static void Close()
    {
        Instance.OnBackPressed();
    }

    // This method ultimately closes the menu, getting called as the close animation is completed by OnAnimationClose
    void CloseMenu()
    {
        if (Instance == null)
        {
            Debug.LogErrorFormat("No menu of type {0} is currently open.", typeof(T));
            return;
        }
        MenuManager.Instance.CloseMenu(Instance);
    }

    // This gets called when the button is pressed, which closes the top menu
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

    #region Helper Methods

    // Play the swoosh sound for the open animation of a menu
    void PlaySwoosh()
    {
        if (aS)
        {
            aS.Play();
        }
    }

    // This method gets called on the end of every closing animation to ultimately close the menu
    public void OnCloseAnimation()
    {
        CloseMenu();
    }

    #endregion

}

/// <summary>
/// The class which forward declares the basic members of the menu container class
/// </summary>
public abstract class Menu : MonoBehaviour
{
    public bool destroyWhenClosed = true;
    public bool disableMenusUnderneath = true;

    public abstract void OnBackPressed();
}
