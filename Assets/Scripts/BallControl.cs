using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    private float jumpForce = 4000f;  // Adjustable jump force
    private float moveForce = 100f;  // Adjustable movement force for WASD
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
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

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
                currentPlatform.SetActive(false);
            }
            currentPlatform = platform;
            currentPlatform.SetActive(true);
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
}