﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoofHouseManager : MonoBehaviour {

    [SerializeField] GameObject roof;
    [SerializeField] float fadeInDuration = 1f;
    [SerializeField] float fadeOutDuration = 1f;
    [SerializeField] float fadedOutValue = 0.1f;

    #region Unity Messages

    void OnTriggerEnter(Collider collider)

    {
        // Fade out the roof
        if (collider.tag == "Player")

        {
            // Set material to transparent
            SetMaterialTransparent();

            iTween.FadeTo(roof, fadedOutValue, fadeOutDuration);
            //FadeAlphaTo(fadedOutValue, fadeOutDuration);
        }

    }

    void OnTriggerExit(Collider collider)

    {
        // Fade in the roof again
        if (collider.tag == "Player")

        {
            // Set material to opaque
            iTween.FadeTo(roof, 1, fadeInDuration);
            //FadeAlphaTo(1, fadeInDuration);

            // This brings back the ambient occlusion usw. but causes a bug where, when you reenter the building quickly, the roof stays opaque
            //Invoke("SetMaterialOpaque", 1.0f);

        }

    }

    #endregion

    #region Private Methods

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

    }


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

    }

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