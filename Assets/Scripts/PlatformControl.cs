using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    private float sensitivity = 40f; 
    private bool isActive = false;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool startActiveState;
    private Vector3 centerPoint;

    void Start()
    {
        // Store the initial state
        startPosition = transform.position;
        startRotation = transform.rotation;
        startActiveState = gameObject.activeSelf;

        // Calculate the center point
        CalculateCenterPoint();
    }

    void CalculateCenterPoint()
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        // Include this object and all its children
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            sum += child.position;
            count++;
        }

        centerPoint = sum / count;
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
            
            // Create rotation quaternions
            Quaternion rotationAroundX = Quaternion.AngleAxis(rotationX, Vector3.right);
            Quaternion rotationAroundZ = Quaternion.AngleAxis(rotationZ, Vector3.forward);

            // Combine rotations
            Quaternion combinedRotation = rotationAroundX * rotationAroundZ;

            // Apply rotation around the center point
            transform.RotateAround(centerPoint, combinedRotation * Vector3.right, rotationX);
            transform.RotateAround(centerPoint, combinedRotation * Vector3.forward, rotationZ);
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