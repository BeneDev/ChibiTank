using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    GameObject player;

    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSpeed = 3f;
    [SerializeField] float controllerRotationSpeed = 4f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position, speed);

        // Rotate with the player tanks forward
        //transform.forward = Vector3.Lerp(transform.forward, player.transform.forward, rotationSpeed);

        // Rotate based on the player aim Direction
        if(GameManager.Instance.isControllerInput)
        {
            print(Time.deltaTime);
            transform.forward = Vector3.Lerp(transform.forward, player.GetComponent<PlayerController>().CockPitForward, controllerRotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.forward = Vector3.Lerp(transform.forward, player.GetComponent<PlayerController>().CockPitForward, rotationSpeed * Time.deltaTime);
        }
    }
}
