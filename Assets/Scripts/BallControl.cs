using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    private float jumpForce = 5000f;  
    private float moveForce = 100f;  
    private bool onBridge = false;  

    private PlatformControl currentPlatform;
    private BridgeControl currentBridge;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Get the Rigidbody component
    }

    void Update()
    {
        if (onBridge)
        {
            HandleMovement();
        } else {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ApplyJump();
            }
        }
    }

    void ApplyJump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    
    void HandleMovement()
    {
        float moveHorizontal = Input.GetKey(KeyCode.D) ? 1f : Input.GetKey(KeyCode.A) ? -1f : 0f;
        float moveVertical = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * moveForce * Time.deltaTime * 1000);  // Use moveForce for WASD movement
    }
    
    void OnCollisionEnter(Collision collision)
    {
        PlatformControl platform = collision.gameObject.GetComponent<PlatformControl>();
        if (platform != null)
        {
            if (currentPlatform != null)
            {
                ChangeColor(currentPlatform.gameObject, Color.white);  // Reset color to white
                currentPlatform.SetActive(false);
            }
            currentPlatform = platform;
            currentPlatform.SetActive(true);
            ChangeColor(currentPlatform.gameObject, Color.green);  // Change color to green

            // Reset the color of the current platform when hitting a bridge
            if (currentBridge != null)
            {
                ChangeColor(currentBridge.gameObject, Color.white);  // Reset color to white
            }
        }

        BridgeControl bridge = collision.gameObject.GetComponent<BridgeControl>();
        if (bridge != null)
        {
            if (currentBridge != null)
            {
                currentBridge.SetActive(false);
            }
            currentBridge = bridge;
            currentBridge.SetActive(true);
            ChangeColor(currentBridge.gameObject, Color.blue);  // Change color to green

            // Reset the color of the current platform when hitting a bridge
            if (currentPlatform != null)
            {
                ChangeColor(currentPlatform.gameObject, Color.white);  // Reset color to white
            }
        }
        
        // Check if the colliding object has the "Bridge" tag
        if (collision.gameObject.CompareTag("Bridge"))
        {
            onBridge = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Check if the exiting object has the "Bridge" tag
        if (collision.gameObject.CompareTag("Bridge"))
        {
            onBridge = false;
        }
    }

    void ChangeColor(GameObject obj, Color color)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = color;
        }
    }
}