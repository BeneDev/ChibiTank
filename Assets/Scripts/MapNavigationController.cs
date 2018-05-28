using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapNavigationController : MonoBehaviour {

    Camera mapCam;
    [SerializeField] float zoomSpeed = 5f;
    [SerializeField] float minZoom = 5f;
    [SerializeField] float maxZoom = 500f;

    private void Awake()
    {
        mapCam = GameObject.FindGameObjectWithTag("MapCam").GetComponent<Camera>() as Camera;
        SetZoom(maxZoom);
    }

    private void Update()
    {
        mapCam.orthographicSize = Mathf.Clamp(mapCam.orthographicSize - (Input.GetAxis("Mouse ScrollWheel") * zoomSpeed != 0 ? Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")) * zoomSpeed : 0), minZoom, maxZoom);
    }
    
    void SetZoom(float zoomLevel)
    {
        mapCam.orthographicSize = zoomLevel;
    }
}
