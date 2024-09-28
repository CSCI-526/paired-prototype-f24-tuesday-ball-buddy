using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text wonMessage;  // Change this line

    void Start()
    {
        if (wonMessage != null)
        {
            wonMessage.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Won Message is not assigned in UIManager");
        }
    }

    public void ShowWinMessage()
    {
        if (wonMessage != null)
        {
            wonMessage.text = "Congrats! You won!";
            wonMessage.gameObject.SetActive(true);
            Debug.Log("Win message displayed");
        }
        else
        {
            Debug.LogError("Won Message is null in ShowWinMessage");
        }
    }

    public void HideWinMessage()
    {
        if (wonMessage != null)
        {
            wonMessage.gameObject.SetActive(false);
            Debug.Log("Win message hidden");
        }
    }
}