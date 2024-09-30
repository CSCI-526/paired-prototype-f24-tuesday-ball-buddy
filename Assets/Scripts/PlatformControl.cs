using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    private float sensitivity = 30f; 
    private bool isActive = false;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool startActiveState;

    void Start()
    {
        // Store the initial state
        startPosition = transform.position;
        startRotation = transform.rotation;
        startActiveState = gameObject.activeSelf;
    }

    void Update()
    {
        if (isActive)
        {
            // Calculate rotation for X and Z axes
            float rotationX = Input.GetKey(KeyCode.UpArrow) ? sensitivity * Time.deltaTime :
                              Input.GetKey(KeyCode.DownArrow) ? -sensitivity * Time.deltaTime : 0f;
            float rotationZ = Input.GetKey(KeyCode.LeftArrow) ? sensitivity * Time.deltaTime :
                              Input.GetKey(KeyCode.RightArrow) ? -sensitivity * Time.deltaTime : 0f;
            
            // Apply rotation around local X and Z axes only
            transform.Rotate(rotationX, 0, rotationZ, Space.Self);
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }

    public void ResetPlatform()
    {
        // Reset position, rotation, and active state
        transform.position = startPosition;
        transform.rotation = startRotation;
        gameObject.SetActive(startActiveState);
        isActive = false;  // Ensure the platform is not active when reset
    }
}