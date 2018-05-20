using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofManager : MonoBehaviour {

    [SerializeField] GameObject roof;

    private void OnTriggerStay(Collider other)
    {
        if(roof)
        {
            roof.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(roof)
        {
            roof.SetActive(true);
        }
    }
}
