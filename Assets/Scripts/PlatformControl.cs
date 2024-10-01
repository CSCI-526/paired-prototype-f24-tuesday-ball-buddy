using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    private float sensitivity = 40f;
    private bool isActive = false;

    private Quaternion startRotation;
    private Vector3 centerPoint;
    private Vector3 currentRotation;

    void Start()
    {
        CalculateCenterPoint();
    }

    void Update()
    {
        if (isActive)
        {
            float rotationX = Input.GetKey(KeyCode.UpArrow) ? sensitivity * Time.deltaTime :
                              Input.GetKey(KeyCode.DownArrow) ? -sensitivity * Time.deltaTime : 0f;
            float rotationZ = Input.GetKey(KeyCode.LeftArrow) ? sensitivity * Time.deltaTime :
                              Input.GetKey(KeyCode.RightArrow) ? -sensitivity * Time.deltaTime : 0f;

            currentRotation.x += rotationX;
            currentRotation.z += rotationZ;

            // Keep y Rotation at 0
            Quaternion targetRotation = Quaternion.Euler(currentRotation.x, 0f, currentRotation.z);

            RotateAroundPoint(targetRotation, centerPoint);
        }
    }

    void CalculateCenterPoint()
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            sum += child.position;
            count++;
        }
        centerPoint = sum / count;
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
}