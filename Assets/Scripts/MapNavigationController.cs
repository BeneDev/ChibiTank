using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This script controls the Navigation for the map, enabling the player to zoom and pan
/// </summary>
public class MapNavigationController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    #region Fields

    Camera mapCam;
    [SerializeField] float zoomSpeed = 5f;
    [SerializeField] float panSpeed = 5f;
    [SerializeField] float minZoom = 5f;
    [SerializeField] float maxZoom = 500f;

    Vector3 mouseOrigin; // Get orignial mouse position, to calculate the amount the player is panning
    bool isPanning = false; // Stores if the player is currently in panning mode

    bool isMouseOver = false; // Stores if the mouse is over the menu, to make panning and zooming only work then

    #endregion

    #region Unity Messages

    private void Awake()
    {
        mapCam = GameObject.FindGameObjectWithTag("MapCam").GetComponent<Camera>() as Camera;
        SetZoom(maxZoom);
    }

    private void Update()
    {
        if (!isMouseOver) { return; }
        // Zoom the camera
        mapCam.orthographicSize = Mathf.Clamp(mapCam.orthographicSize - (Input.GetAxis("Mouse ScrollWheel") * zoomSpeed != 0 ? Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")) * zoomSpeed : 0), minZoom, maxZoom);

        // If mouse button is held down, the player can pan, as the map goes into pan mode
        if (Input.GetMouseButtonDown(0))
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

    #endregion

    #region Helper Methods

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }
    
    void SetZoom(float zoomLevel)
    {
        mapCam.orthographicSize = zoomLevel;
    }

    #endregion

}
