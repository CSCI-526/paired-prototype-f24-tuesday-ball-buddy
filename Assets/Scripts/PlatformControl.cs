using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    private float sensitivity = 30f; 
    private bool isActive = false;

    void Update()
    {
        if (isActive)
        {
            float rotationX = Input.GetKey(KeyCode.UpArrow) ? sensitivity * Time.deltaTime :
                              Input.GetKey(KeyCode.DownArrow) ? -sensitivity * Time.deltaTime : 0f;
            float rotationZ = Input.GetKey(KeyCode.LeftArrow) ? sensitivity * Time.deltaTime :
                              Input.GetKey(KeyCode.RightArrow) ? -sensitivity * Time.deltaTime : 0f;
            transform.Rotate(rotationX, 0, rotationZ);
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }
}
