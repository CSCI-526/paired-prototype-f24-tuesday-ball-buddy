using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeControl1 : MonoBehaviour
{
    // Speed at which the object shrinks
    public float initialShrinkSpeed;
    public float minShrinkSpeed;  // Minimum shrink speed
    public float speedDecreasePerPress ;  // How much to decrease speed per key press

    private float currentShrinkSpeed;
    private bool shouldShrink = false;
    private bool isJKeyPressed = false;

    // Add this to allow setting the expected tag in the Inspector
    public string ballTag = "Sphere";

    // Update is called once per frame
    void Start()
    {
        currentShrinkSpeed = initialShrinkSpeed;
        Debug.Log($"BridgeControl started. Initial shrink speed: {currentShrinkSpeed}");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            isJKeyPressed = true;
            Debug.Log("J key pressed");
        }
        else if (Input.GetKeyUp(KeyCode.J))
        {
            isJKeyPressed = false;
            ResetSpeed();
            Debug.Log("J key released, speed reset to normal");
        }

        // Shrinking logic
        if (shouldShrink)
        {
            if (isJKeyPressed)
            {
                DecreaseSpeed();
            }

            // Get the current scale of the object
            Vector3 currentScale = transform.localScale;

            // Reduce the z scale over time
            float newZScale = Mathf.Max(0, currentScale.z - currentShrinkSpeed * Time.deltaTime);

            // Apply the new scale to the object
            transform.localScale = new Vector3(currentScale.x, currentScale.y, newZScale);

            // If the z scale is zero, disable the object
            if (newZScale == 0)
            {
                Debug.Log("Bridge fully shrunk, disabling object");
                gameObject.SetActive(false); // Disable the bridge
            }
        }
        else
        {
            Debug.Log("Not shrinking. shouldShrink is false.");
        }
    }

    private void DecreaseSpeed()
    {
        currentShrinkSpeed = Mathf.Max(minShrinkSpeed, currentShrinkSpeed - speedDecreasePerPress * Time.deltaTime);
        Debug.Log($"Decreasing speed. Current shrink speed: {currentShrinkSpeed}");
    }

    private void ResetSpeed()
    {
        currentShrinkSpeed = initialShrinkSpeed;
        Debug.Log($"Speed reset to normal: {currentShrinkSpeed}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision detected with: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");
        
        // Check if the colliding object is the ball, either by tag or name
        if (collision.gameObject.CompareTag(ballTag) || 
            collision.gameObject.name.ToLower().Contains("ball") || 
            collision.gameObject.name.ToLower().Contains("sphere"))
        {
            Debug.Log("Collision detected with ball. Starting to shrink.");
            shouldShrink = true;
        }
        else
        {
            Debug.Log("Collision detected, but not with the ball. No shrinking started.");
        }
    }
}
