using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    public float jumpForce;  // Adjustable jump force
    public float moveForce = 40f;  // Adjustable movement force for WASD
    public Transform platform;  // Reference to the platform
    public int stageIdentifier = 0;

    private PlatformControl currentPlatform;
    private Rigidbody rb;
    private bool onBridge = false;  // Flag to check if the ball is on the bridge

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Get the Rigidbody component
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))  // Detect spacebar press
        {
            ApplyJump();
        }

        if (onBridge)
        {
            HandleMovement();
        }
    }

    void ApplyJump()
    {
        // Apply vertical impulse to the ball
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    
    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * moveForce);  // Use moveForce for WASD movement
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

        // Check if the colliding object has the "Platform" tag
        if (collision.gameObject.CompareTag("Platform"))
        {
            stageIdentifier = 1;
            
            // Attempt to make the platform glow
            Renderer stageRenderer = collision.gameObject.GetComponent<Renderer>();
            if (stageRenderer != null)
            {
                // Enable emission on the material
                stageRenderer.material.EnableKeyword("_EMISSION");
                
                // Set the emission color (you can adjust this color as needed)
                Color glowColor = Color.yellow; // Example: yellow glow
                stageRenderer.material.SetColor("_EmissionColor", glowColor);
            }
            else
            {
                Debug.LogWarning("Renderer component not found on the platform.");
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
}