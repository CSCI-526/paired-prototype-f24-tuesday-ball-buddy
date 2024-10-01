using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }
    private Vector3 lastCheckpoint;
    private const string CHECKPOINT_X = "CheckpointX";
    private const string CHECKPOINT_Y = "CheckpointY";
    private const string CHECKPOINT_Z = "CheckpointZ";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCheckpoint();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void LoadCheckpoint()
    {
        lastCheckpoint = new Vector3(
            PlayerPrefs.GetFloat(CHECKPOINT_X, 0f),
            PlayerPrefs.GetFloat(CHECKPOINT_Y, 0f),
            PlayerPrefs.GetFloat(CHECKPOINT_Z, 0f)
        );
    }

    public void SetCheckpoint(Vector3 position)
    {
        lastCheckpoint = position;
        PlayerPrefs.SetFloat(CHECKPOINT_X, position.x);
        PlayerPrefs.SetFloat(CHECKPOINT_Y, position.y);
        PlayerPrefs.SetFloat(CHECKPOINT_Z, position.z);
        PlayerPrefs.Save();
    }

    public Vector3 GetLastCheckpoint() => lastCheckpoint;

    public bool HasCheckpoint() => lastCheckpoint != Vector3.zero;

    public void ResetCheckpoint()
    {
        lastCheckpoint = Vector3.zero;
        PlayerPrefs.DeleteKey(CHECKPOINT_X);
        PlayerPrefs.DeleteKey(CHECKPOINT_Y);
        PlayerPrefs.DeleteKey(CHECKPOINT_Z);
        PlayerPrefs.Save();
    }
}