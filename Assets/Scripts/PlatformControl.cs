using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    public float sensitivity = 40f;
    private bool isActive = false;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool startActiveState;
    private Vector3 centerPoint;
    private Vector3 currentRotation;

    void Start()
    {
        // Store the initial state
        startPosition = transform.position;
        startRotation = transform.rotation;
        startActiveState = gameObject.activeSelf;
        currentRotation = transform.rotation.eulerAngles;

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

            // Update current rotation
            currentRotation.x += rotationX;
            currentRotation.z += rotationZ;

            // Create rotation quaternion, keeping Y at 0
            Quaternion targetRotation = Quaternion.Euler(currentRotation.x, 0f, currentRotation.z);

            // Apply rotation around the center point
            RotateAroundPoint(targetRotation, centerPoint);
        }
    }

    void RotateAroundPoint(Quaternion targetRotation, Vector3 point)
    {
        Vector3 centerToPosition = transform.position - point;
        centerToPosition = targetRotation * Quaternion.Inverse(transform.rotation) * centerToPosition;
        transform.position = point + centerToPosition;
        transform.rotation = targetRotation;
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
        isActive = false;
        currentRotation = startRotation.eulerAngles;
    }
}