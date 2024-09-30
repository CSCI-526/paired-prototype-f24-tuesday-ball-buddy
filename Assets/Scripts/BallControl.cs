using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    private float jumpForce = 4000f;  
    private float moveForce = 100f;  
    public bool onBridge = false;  
    private bool canJump = true;  // New variable to track if the ball can jump

    private PlatformControl currentPlatform;
    private BridgeControl currentBridge;
    private Rigidbody rb;

    public float fallThreshold = -2f;  // Y position below which the ball is considered fallen
    private GameManager gameManager;
    private Vector3 startPosition;

    private UIManager uiManager;
    private bool isGameWon = false;
    
    private Renderer ballRenderer; // Add this variable at the class level
    public LayerMask goalLayer; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
            enabled = false; // Disable this script if GameManager is missing
            return;
        }  // Get the Rigidbody component
        startPosition = transform.position;

        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)  // Changed from != to ==
        {
            Debug.LogError("UIManager not found in the scene!");
        }
        else
        {
            Debug.Log("UIManager found successfully");
        }
        
        // Add this line to get the ball's renderer
        ballRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (onBridge)
        {
            HandleMovement();
        } 
        else 
        {
            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                ApplyJump();
                canJump = false;  // Prevent jumping again until grounded
            }
        }
        if (transform.position.y < fallThreshold)
        {
            Debug.Log("Ball has fallen");
            gameManager.RestartGame();
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
            ChangeColor(currentPlatform.gameObject, new Color(0.4f, 0.8f, 0.4f));  // Brighter, minty green

            // Change the ball's color to white when on a platform
            if (ballRenderer != null)
            {
                ballRenderer.material.color = Color.white;  // White color
            }
            onBridge = false;
        }

        BridgeControl bridge = collision.gameObject.GetComponentInParent<BridgeControl>();
        if (bridge != null)
        {
            if (currentBridge != null && currentBridge != bridge)
            {
                ChangeColor(currentBridge.gameObject, Color.white);  // Reset previous bridge color
                currentBridge.SetActive(false);
            }
            currentBridge = bridge;
            currentBridge.SetActive(true);
            
            // Keep the bridge color white
            ChangeColor(currentBridge.gameObject, Color.white);

            // Change the ball's color to royal blue when on a bridge
            if (ballRenderer != null)
            {
                ballRenderer.material.color = new Color(0.25f, 0.41f, 0.88f);  // Royal blue
            }

            // Reset the color of the current platform when hitting a bridge
            if (currentPlatform != null)
            {
                ChangeColor(currentPlatform.gameObject, Color.white);  // Reset color to white
            }

            onBridge = true;
        }
        
        // Check if the colliding object has the "Bridge" tag
        if (collision.gameObject.CompareTag("Bridge"))
        {
            onBridge = true;
        }

        // Check if the ball has hit the ground or any platform
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.GetComponent<PlatformControl>() != null)
        {
            canJump = true;  // Allow jumping again
        }
        Debug.Log($"Collision detected with: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");
        
        // Check if the collision is with an object on the Goal layer
        if ((goalLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Debug.Log("Ball hit the Goal");
            if (uiManager != null)
            {
                Debug.Log("Calling WinGame");
                WinGame();
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // Ensure the ball can be controlled on all parts of the bridge
        if (collision.gameObject.GetComponentInParent<BridgeControl>() != null)
        {
            onBridge = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Check if the exiting object is part of a bridge
        if (collision.gameObject.GetComponentInParent<BridgeControl>() != null)
        {
            // Bridge color remains white when the ball leaves
            if (currentBridge != null)
            {
                ChangeColor(currentBridge.gameObject, Color.white);
            }
            
            // Reset the ball's color to white
            if (ballRenderer != null)
            {
                ballRenderer.material.color = Color.white;
            }
        }
    }

    void ChangeColor(GameObject obj, Color color)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            // Skip color change for objects tagged as "Goal"
            if (!renderer.gameObject.CompareTag("Goal"))
            {
                renderer.material.color = color;
            }
        }
    }
    
    void WinGame()
    {
        if (!isGameWon)
        {
            isGameWon = true;
            Debug.Log("Game is won");
            


            // Show win message
        if (uiManager != null)
            {
                uiManager.ShowWinMessage();
            }

        }
    }
}