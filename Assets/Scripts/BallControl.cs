using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    private float jumpForce = 4000f;  
    private float moveForce = 200f;  
    public bool onBridge = false;  
    private bool canJump = true;  
    public float fallThreshold = -50f;  

    private PlatformControl currentPlatform;
    private BridgeControl currentBridge;
    private Rigidbody rb;

    private Renderer ballRenderer; 
    public LayerMask goalLayer; 

    private HUDManager hudManager; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ballRenderer = GetComponent<Renderer>();
        hudManager = FindObjectOfType<HUDManager>();
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
                canJump = false;  // Prevent double jump
            }
        }
        if (transform.position.y < fallThreshold)
        {
            Debug.Log("Ball fell off");
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
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
        //PLATFORM
        PlatformControl platform = collision.gameObject.GetComponent<PlatformControl>();
        if (platform != null)
        {
            onBridge = false;
            canJump = true;
            
            if (currentPlatform != null)
            {
                ChangeColor(currentPlatform.gameObject, Color.white);  // White
                currentPlatform.SetActive(false);
            }

            currentPlatform = platform;
            currentPlatform.SetActive(true);
            ChangeColor(currentPlatform.gameObject, new Color(0.4f, 0.8f, 0.4f));  // Green

            if (ballRenderer != null)
            {
                ballRenderer.material.color = Color.white;  // White 
            }
            
            if (currentBridge != null)
            {
                ChangeColor(currentBridge.gameObject, Color.white);  // Reset color to white
            }
        }

        //BRIDGE
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
            
            ChangeColor(currentBridge.gameObject, Color.gray);  // Gray

            if (ballRenderer != null)
            {
                ballRenderer.material.color = new Color(0.25f, 0.41f, 0.88f);  // Blue
            }

            // Reset the color of the current platform when hitting a bridge
            if (currentPlatform != null)
            {
                ChangeColor(currentPlatform.gameObject, Color.white);  // Reset color to white
            }

            onBridge = true;
        }
        
        //GOAL
        if (collision.gameObject.CompareTag("Goal"))
        {
            if (hudManager != null)
            {
                hudManager.ShowWinMessage();
                Time.timeScale = 0;
                rb.velocity = Vector3.zero;
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
}