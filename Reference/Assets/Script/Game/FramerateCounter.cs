using UnityEngine;
using TMPro; 

public class FramerateCounter : MonoBehaviour
{
    public TMP_Text uiText; 
    public TMP_Text uiTextWorldSpace; 

    private float deltaTime = 0.0f;

    void Update()
    {
        // Update the frame time for FPS calculation
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Calculate the FPS value
        int fps = Mathf.CeilToInt(1.0f / deltaTime);

        // Update both the UI Text and World Space Text with the FPS value
        if (uiText != null)
        {
            // Update the FPS in the UI canvas
            uiText.text = $"{fps} FPS"; 
        }

        if (uiTextWorldSpace != null)
        {
            // Update the FPS in the world space
            uiTextWorldSpace.text = $"{fps} FPS"; 
        }
    }
}
