﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayController : Singleton<OverlayController> {

    [SerializeField] Vector2 hotSpotOffset;
    [SerializeField] CanvasGroup cursorCanvasGroup;
    [SerializeField] GameObject image;

    [SerializeField] CanvasGroup itemCanvasGroup;
    [SerializeField] CanvasGroup dayTimeCanvasGroup;

    [SerializeField] Text shotsInMagazineText;
    [SerializeField] Text magazineSizeText;

    [SerializeField] Image[] itemImages;

    [SerializeField] Text[] itemUsageCountTexts;

    PlayerController player;

    [SerializeField] BaseBarController[] bars; // To fade out the health and shield bar when not in a fight
    bool areBarsFadedIn = true;
    
    public event System.Action OnReloadFinished;

    Animator anim;

    private void Awake()
    {
        Cursor.visible = false;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.OnMagazineSizeChanged += ChangeMagazineSizeText;
        player.OnShotsInMagazineChanged += ChangeShotsInMagazineText;

        player.OnEquippedItemChanged += ChangeEquippedItemDisplay;

        player.OnEquippedItemUsageCountChanged += ChangeEquippedItemUsageCountText;
        GameManager.Instance.OnEnemiesNearbyPlayerChanged += ControlBars;
        MenuManager.Instance.OnMenuCountZero += ToggleFadeForMenus;
    }

    void ToggleFadeForMenus(bool isFaded)
    {
        if(isFaded)
        {
            dayTimeCanvasGroup.alpha = 1f;
            itemCanvasGroup.alpha = 1f;
            foreach (BaseBarController obj in bars)
            {
                obj.CanvasAlpha = 1f;
            }
        }
        else if(!isFaded)
        {
            dayTimeCanvasGroup.alpha = 0f;
            itemCanvasGroup.alpha = 0f;
            foreach (BaseBarController obj in bars)
            {
                obj.CanvasAlpha = 0f;
            }
        }
    }

    void ControlBars(int enemyCount)
    {
        // TODO control bars with animations just as items
        if(enemyCount <= 0 && areBarsFadedIn)
        {
            StartCoroutine(FadeOutBars(1f));
        }
        else if (enemyCount > 0 && !areBarsFadedIn)
        {
            StartCoroutine(FadeInBars(1f));
        }
    }

    IEnumerator FadeInBars(float duration)
    {
        areBarsFadedIn = true;
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            foreach (BaseBarController obj in bars)
            {
                obj.CanvasAlpha = t / duration;
            }
            yield return new WaitForEndOfFrame();
        }
        foreach (BaseBarController obj in bars)
        {
            obj.CanvasAlpha = 1f;
        }
    }

    IEnumerator FadeOutBars(float duration)
    {
        areBarsFadedIn = false;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            foreach (BaseBarController obj in bars)
            {
                obj.CanvasAlpha = 1 - t / duration;
            }
            yield return new WaitForEndOfFrame();
        }
        foreach (BaseBarController obj in bars)
        {
            obj.CanvasAlpha = 0f;
        }
    }

    private void Update()
    {
        Cursor.visible = false;
        if (!GameManager.Instance.IsCursorVisible && cursorCanvasGroup)
        {
            cursorCanvasGroup.alpha = 0f;
            return;
        }
        else if (cursorCanvasGroup)
        {
            cursorCanvasGroup.alpha = 1f;
        }
        if(image)
        {
            image.transform.position = Input.mousePosition;
        }
        else
        {
            Debug.LogError("There is no panel serialized for the Cursor Controller in the Inspector!");
        }
    }

    void ChangeMagazineSizeText(int newSize)
    {
        magazineSizeText.text = newSize.ToString();
    }

    void ChangeShotsInMagazineText(int newShotsCount)
    {
        shotsInMagazineText.text = newShotsCount.ToString();
    }

    void ChangeEquippedItemDisplay(int slot, Sprite newSprite)
    {
        itemImages[slot].sprite = newSprite;
    }

    void ChangeEquippedItemUsageCountText(int slot, int newCount)
    {
        itemUsageCountTexts[slot].text = newCount.ToString();
    }

    public void FlashupAnimation()
    {
        anim.SetTrigger("Flashup");
    }

    public void TriggerReloadAnimation(float duration)
    {
        StartCoroutine(ReloadAnimation(duration));
    }

    IEnumerator ReloadAnimation(float duration)
    {
        RectTransform imageTransform = image.GetComponent<RectTransform>();
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float angleZ = 360f * (t / duration);
            Quaternion newRotation = Quaternion.Euler(0f, 0f, angleZ);
            imageTransform.rotation = newRotation;
            yield return new WaitForEndOfFrame();
        }
        imageTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
        FlashupAnimation();
        if(OnReloadFinished != null)
        {
            OnReloadFinished();
        }
    }
}
