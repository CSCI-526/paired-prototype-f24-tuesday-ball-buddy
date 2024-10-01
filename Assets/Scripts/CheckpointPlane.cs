using UnityEngine;

public class CheckpointPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        BallControl ball = other.GetComponent<BallControl>();
        if (ball != null)
        {
            ball.SetCheckpoint(transform.position);
            Debug.Log("Checkpoint set at: " + transform.position);
        }
    }
}