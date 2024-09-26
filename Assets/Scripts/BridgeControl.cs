using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeControl : MonoBehaviour
{
    // Speed at which the object shrinks
    public float initialShrinkSpeed;
    public float minShrinkSpeed;  // Minimum shrink speed
    public float speedDecreasePerPress ;  // How much to decrease speed per key press

    private float currentShrinkSpeed;
    private bool shouldShrink = false;
    private bool isJKeyPressed = false;

    private bool isActive = false;


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
        if (isActive) {
            if (isJKeyPressed)
            {
                DecreaseSpeed();
            }

            // Get the current scale of the object
            Vector3 currentScale = transform.localScale;
            // Reduce the x scale over time
            float newXScale = Mathf.Max(0, currentScale.x - currentShrinkSpeed * Time.deltaTime);
            // Apply the new scale to the object
            transform.localScale = new Vector3(newXScale, currentScale.y, currentScale.z);
            // If the x scale is zero, disable the object
            if (newXScale == 0)
            {
                Debug.Log("Bridge fully shrunk, disabling object");
                gameObject.SetActive(false); // Disable the bridge
            }
        } else {
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

    public void SetActive(bool active)
    {
        isActive = active;
    }
}
