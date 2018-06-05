using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script, which will fade out the roofs of the houses its attached to when the player enters the given trigger volume
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class RoofHouseManager : MonoBehaviour {

    #region Fields

    [SerializeField] GameObject roof; // The roof which will be faded out
    [SerializeField] float fadeInDuration = 1f; 
    [SerializeField] float fadeOutDuration = 1f;
    [SerializeField] float fadedOutValue = 0.1f; // To what alpha value the roof material will fade
    [SerializeField] float distanceUntilMaterialSwitch = 10f; // The distance the player has to be away from the roof until it gets set to the right material again.
    // This distance is implemented, because the player would be able to tell the difference in a short ficker in color on the roof. 
    //To prevent this, this change is done when the player is far enough gone.

    // Fields to calculate the distance to the player for the material switch
    GameObject player;
    Vector3 toPlayer;

    bool isMaterialTransparent = false;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Set the material back to normal when the player is far enough gone
    private void Update()
    {
        // Set material to opaque again
        if (isMaterialTransparent)
        {
            toPlayer = transform.position - player.transform.position;
            if (toPlayer.magnitude > distanceUntilMaterialSwitch)
            {
                Invoke("SetMaterialOpaque", 1.0f);
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        // Fade out the roof
        if (collider.tag == "Player")
        {
            // Set material to transparent
            SetMaterialTransparent();

            iTween.FadeTo(roof, fadedOutValue, fadeOutDuration);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        // Fade in the roof again
        if (collider.tag == "Player")
        {
            // Fade in the roof again
            iTween.FadeTo(roof, 1, fadeInDuration);

            // If you want to make material appear normal again right after fade in animation (the player can spot that) then activate invoke call here
            //Invoke("SetMaterialOpaque", 1.0f);
        }
    }

    #endregion

    #region Private Methods

    // Set the material to transparent to enable fading
    private void SetMaterialTransparent()
    {
        foreach (Material m in roof.GetComponent<Renderer>().materials)
        {
            m.SetFloat("_Mode", 2);
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            m.SetInt("_ZWrite", 0);
            m.DisableKeyword("_ALPHATEST_ON");
            m.EnableKeyword("_ALPHABLEND_ON");
            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            m.renderQueue = 3000;
        }
        isMaterialTransparent = true;
    }

    // Set the material to the standard opaque shader again to make it look normal again
    private void SetMaterialOpaque()
    {
        foreach (Material m in roof.GetComponent<Renderer>().materials)
        {
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            m.SetInt("_ZWrite", 1);
            m.DisableKeyword("_ALPHATEST_ON");
            m.DisableKeyword("_ALPHABLEND_ON");
            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            m.renderQueue = -1;
        }
        isMaterialTransparent = false;
    }

    // Fadde the alpha to a certain value, making it fade out
    IEnumerator FadeAlphaTo(float value, float duration)
    {
        Color color = roof.GetComponent<MeshRenderer>().material.color;
        float initalAlpha = color.a;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(initalAlpha, value, duration);
            roof.GetComponent<MeshRenderer>().material.color = color;
            yield return new WaitForEndOfFrame();
        }
        color.a = value;
        roof.GetComponent<MeshRenderer>().material.color = color;
    }

    #endregion

}