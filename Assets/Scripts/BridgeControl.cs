using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeControl : MonoBehaviour
{
    // Speed at which the object shrinks
    public float shrinkSpeed;
    public float widthIncreasePerPress;  // How much to increase width per key press

    private bool isJKeyPressed = false;
    private bool isActive = false;

    // Update is called once per frame
    void Start()
    {
        Debug.Log($"BridgeControl started. Shrink speed: {shrinkSpeed}");
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
            Debug.Log("J key released");
        }

        // Shrinking logic
        if (isActive) {
            // Get the current scale of the object
            Vector3 currentScale = transform.localScale;

            // Reduce the x scale over time
            float newXScale = Mathf.Max(0, currentScale.x - shrinkSpeed * Time.deltaTime);
            transform.localScale = new Vector3(newXScale, currentScale.y, currentScale.z);
            Debug.Log($"Shrinking. Current scale: {transform.localScale}");

            if (isJKeyPressed)
            {
                // Add width when J key is pressed
                float newWidth = currentScale.x + widthIncreasePerPress * Time.deltaTime;
                transform.localScale = new Vector3(newWidth, currentScale.y, currentScale.z);
                Debug.Log($"Width increased. Current scale: {transform.localScale}");
            }

            // If the x scale is zero, disable the object
            if (transform.localScale.x == 0)
            {
                Debug.Log("Bridge fully shrunk, disabling object");
                gameObject.SetActive(false); // Disable the bridge
            }
        } else {
            Debug.Log("Not shrinking. isActive is false.");
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }
}
