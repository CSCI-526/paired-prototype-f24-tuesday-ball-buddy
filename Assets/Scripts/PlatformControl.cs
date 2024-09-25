using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    public float sensitivity = 20f; // Increase sensitivity
    private bool isActive = false;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Make the platform kinematic
    }

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
