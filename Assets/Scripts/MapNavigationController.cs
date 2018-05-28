using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapNavigationController : MonoBehaviour {

    Camera mapCam;
    [SerializeField] float zoomSpeed = 5f;
    [SerializeField] float panSpeed = 5f;
    [SerializeField] float minZoom = 5f;
    [SerializeField] float maxZoom = 500f;

    Vector3 mouseOrigin;
    bool isPanning = false;

    private void Awake()
    {
        mapCam = GameObject.FindGameObjectWithTag("MapCam").GetComponent<Camera>() as Camera;
        SetZoom(maxZoom);
    }

    private void Update()
    {
        // Zoom the camera
        mapCam.orthographicSize = Mathf.Clamp(mapCam.orthographicSize - (Input.GetAxis("Mouse ScrollWheel") * zoomSpeed != 0 ? Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")) * zoomSpeed : 0), minZoom, maxZoom);

        // If mouse button is held down, the player can pan, as the map goes into pan mode
        if(Input.GetMouseButtonDown(0))
        {
            mouseOrigin = Input.mousePosition;
            isPanning = true;
        }

        // Disable isPanning again if mouse button is left up again
        if (!Input.GetMouseButton(0)) { isPanning = false; }

        // Let the player pan
        if (isPanning)
        {
            Vector3 pos = mapCam.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            Vector3 move = new Vector3(pos.x * panSpeed * (mapCam.orthographicSize * 0.1f), pos.y * panSpeed * (mapCam.orthographicSize * 0.1f), 0);
            mapCam.transform.Translate(-move, Space.Self);
        }
    }
    
    void SetZoom(float zoomLevel)
    {
        mapCam.orthographicSize = zoomLevel;
    }
}
