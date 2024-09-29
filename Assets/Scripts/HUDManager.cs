using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public TMP_Text player1HUD;
    public TMP_Text player2HUD;
    public BallControl ballControl;

    private void Update()
    {
        UpdatePlayer1HUD();
        UpdatePlayer2HUD();
    }

    private void UpdatePlayer1HUD()
    {
        string controls = ballControl.onBridge ? "[ WASD ]  Move" : "[ Space ]  Jump";
        string colorTag = ballControl.onBridge ? "<color=#6699FF>" : "<color=#B3CCFF>"; // Light blue color when on bridge, lighter blue when on platform
        string closeColorTag = "</color>";
        player1HUD.text = $"{colorTag}Player 1\n<size=60%>{controls}</size>{closeColorTag}";
    }

    private void UpdatePlayer2HUD()
    {
        string controls = !ballControl.onBridge ? "[ Arrows ]  Tilt" : "[ J ]  Stop Shrinking";
        string colorTag = ballControl.onBridge ? "<color=#B2FFB2>" : "<color=#66CC66>"; // Light green when on bridge, light green when not on bridge (platform active)
        player2HUD.text = $"{colorTag}Player 2\n<size=60%>{controls}</size>";
        string closeColorTag = !ballControl.onBridge ? "</color>" : "";
        player2HUD.text = $"{colorTag}Player 2\n<size=60%>{controls}</size>{closeColorTag}";
    }
}
