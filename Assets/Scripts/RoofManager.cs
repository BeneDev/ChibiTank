using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofManager : MonoBehaviour {

    [SerializeField] GameObject roof;

    private void OnTriggerEnter(Collider other)
    {
        if(roof && other.tag == "Player")
        {
            StartCoroutine(FadeAlphaTo(0f, 1f));
            roof.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (roof && other.tag == "Player")
        {
            //StartCoroutine(FadeAlphaTo(1f, 1f));
            roof.SetActive(true);
        }
    }

    IEnumerator FadeAlphaTo(float value, float duration)
    {
        Color color = roof.GetComponent<MeshRenderer>().material.color;
        float initalAlpha = color.a;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            color.a = (value - initalAlpha) / duration;
            roof.GetComponent<MeshRenderer>().material.color = color;
            yield return new WaitForEndOfFrame();
        }
        color.a = value;
        roof.GetComponent<MeshRenderer>().material.color = color;
    }
}
