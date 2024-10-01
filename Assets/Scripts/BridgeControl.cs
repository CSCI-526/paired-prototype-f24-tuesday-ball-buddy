using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeControl : MonoBehaviour
{
    private float shrinkSpeed = 2f;
    private float widthIncreasePerPress = 0.25f;  
    private bool isJKeyPressed = false;
    private bool isActive = false;

    void Update()
    {
        if (isActive) {
            Vector3 currentScale = transform.localScale;

            if (Input.GetKeyDown(KeyCode.J))
            {
                float newWidth = currentScale.x + widthIncreasePerPress;
                transform.localScale = new Vector3(newWidth, currentScale.y, currentScale.z);
            } 
            else 
            {
                float newXScale = Mathf.Max(0, currentScale.x - shrinkSpeed * Time.deltaTime);
                transform.localScale = new Vector3(newXScale, currentScale.y, currentScale.z);
            }

            if (transform.localScale.x == 0)
            {
                Debug.Log("Bridge fully shrunk");
                gameObject.SetActive(false); 
            }
        } 
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }
}
