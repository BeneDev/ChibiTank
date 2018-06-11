using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayController : Singleton<OverlayController> {

    [SerializeField] Vector2 hotSpotOffset;
    CanvasGroup ownCanvasGroup;
    [SerializeField] GameObject image;

    [SerializeField] Text shotsInMagazineText;
    [SerializeField] Text magazineSizeText;

    [SerializeField] Image[] itemImages;

    [SerializeField] Text[] itemUsageCountTexts;

    PlayerController player;
    
    public event System.Action OnReloadFinished;

    Animator anim;

    private void Awake()
    {
        Cursor.visible = false;
        ownCanvasGroup = GetComponent<CanvasGroup>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.OnMagazineSizeChanged += ChangeMagazineSizeText;
        player.OnShotsInMagazineChanged += ChangeShotsInMagazineText;

        player.OnEquippedItemChanged += ChangeEquippedItemDisplay;

        player.OnEquippedItemUsageCountChanged += ChangeEquippedItemUsageCountText;
    }

    private void Update()
    {
        Cursor.visible = false;
        if (!GameManager.Instance.IsCursorVisible)
        {
            ownCanvasGroup.alpha = 0f;
            return;
        }
        else
        {
            ownCanvasGroup.alpha = 1f;
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
