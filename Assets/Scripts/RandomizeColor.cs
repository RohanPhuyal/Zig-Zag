using System;
using UnityEngine;
using UnityEngine.UI;

public class RandomizeColor : MonoBehaviour
{
    private Renderer objectRenderer; // Reference to the object's Renderer
    private Color currentColor; // Current color of the object
    private Color targetColor;  // Target color we want to transition to
    private float lerpTime = 0f; // Time for lerping
    private float transitionDuration = 2f; // Duration of the transition in seconds
    private bool isLerping = false; // Flag to check if lerping is happening
    private void Start()
    {
        // Get the Renderer component of the object the script is attached to
        objectRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (ScoreManager.instance && GameManager.instance && GameManager.instance.gameStarted)
        {
            if (ScoreManager.instance.score % 50 == 0 && ScoreManager.instance.score != 0)
            {
                if (GameManager.instance.platformColor != null)
                {
                    Debug.Log("RON COLOR");
                    ApplyColor(GameManager.instance.platformColor);
                }
            }
        }
        // Transition only if lerping is in progress
        if (isLerping)
        {
            lerpTime += Time.deltaTime / transitionDuration; // Increment lerp time based on transition duration

            // Lerp the color only if lerping is active
            objectRenderer.material.color = Color.Lerp(currentColor, targetColor, lerpTime);

            // Stop lerping when the transition is complete
            if (lerpTime >= 1f)
            {
                isLerping = false;
            }
        }
    }

    public void ApplyColor(Color newColor )
    {
        targetColor = newColor; // Set the target color
        currentColor = objectRenderer.material.color; // Store the current color before starting the transition
        lerpTime = 0f; // Reset lerp time for the new transition
        isLerping = true; // Start lerping
    }

    /*private void RandomizeColorFunction()
    {
        // Randomly choose whether to change Red, Green, or Blue
        int randomComponent = UnityEngine.Random.Range(0, 3); // 0 = Red, 1 = Green, 2 = Blue

        // Set the other two components to either 0 or 255
        int red = (randomComponent == 0) ? UnityEngine.Random.Range(0, 256) : UnityEngine.Random.Range(0, 2) * 255;
        int green = (randomComponent == 1) ? UnityEngine.Random.Range(0, 256) : UnityEngine.Random.Range(0, 2) * 255;
        int blue = (randomComponent == 2) ? UnityEngine.Random.Range(0, 256) : UnityEngine.Random.Range(0, 2) * 255;

        // Create a new Color with randomized values (normalized to 0-1 range)
        Color newColor = new Color(red / 255f, green / 255f, blue / 255f);
        // Apply the color to the object's material
        if (objectRenderer != null)
        {
            objectRenderer.material.color = newColor; // Apply color to material
        }
    }*/
    
}