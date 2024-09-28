using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public BallControl ball;
    public List<PlatformControl> platforms;
    public List<BridgeControl> bridges;
    
    private Vector3 ballStartPosition;

    void Start()
    {
        ballStartPosition = ball.transform.position;
    }

    public void RestartGame()
    {
        // Reset ball position
        ball.transform.position = ballStartPosition;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Reset platforms
        foreach (var platform in platforms)
        {
            Debug.Log("Resetting platform");
            
            platform.ResetPlatform();
            
        }

        // Reset bridges
        foreach (var bridge in bridges)
        {
            Debug.Log("Resetting bridge");
            bridge.ResetBridge();
        }
    }
}