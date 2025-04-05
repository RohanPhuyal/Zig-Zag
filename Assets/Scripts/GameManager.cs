using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool gameOver;
    public bool gameStarted;
    public Color platformColor;
    [SerializeField]
    private PlatformSpawner platformSpawner;
    private float transitionDuration = 0.5f; // Time to complete color transition

    void Awake(){
        Application.targetFrameRate = 60;
        if (instance == null)
        {
            instance = this;
        }
        SetAutoQualityLevel();  // Call it when the game starts

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOver = false;
        gameStarted = false;
        
    }
    public void SetAutoQualityLevel()
    {
        int processorCount = SystemInfo.processorCount;
        int graphicsMemory = SystemInfo.graphicsMemorySize;
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        Debug.Log("Resolution: "+screenWidth+"x"+screenHeight);

        // Check for low-end devices
        if (processorCount <= 2 && graphicsMemory <= 1024)
        {
            QualitySettings.SetQualityLevel(0);  // Low quality
            Debug.Log("Low quality set.");
            SetGameResolution(720);  // Set resolution to 720p for low quality
        }
        // Check for mid-range devices
        else if (processorCount <= 4 && graphicsMemory <= 2048)
        {
            QualitySettings.SetQualityLevel(2);  // Medium quality
            Debug.Log("Medium quality set.");
            // Optionally, you can set medium resolution or stick to 1080p
            SetGameResolution(1080);  // You could adjust to 1080p if you'd like for medium
        }
        // High-end devices
        else
        {
            QualitySettings.SetQualityLevel(5);  // High quality
            Debug.Log("High quality set.");
            SetGameResolution(1080);  // Set resolution to 1080p for high quality
        }
    }

    private void SetGameResolution(int baseResolutionHeight)
    {
        // Get the current aspect ratio of the device (width / height)
        float aspectRatio = (float)Screen.width / Screen.height;

        // Calculate the corresponding width based on the desired height while maintaining the aspect ratio
        int resolutionHeight = baseResolutionHeight;
        int resolutionWidth = Mathf.RoundToInt(resolutionHeight * aspectRatio);  // Ensure proper rounding

        // Apply the resolution, making sure it's in portrait mode
        Screen.SetResolution(resolutionWidth, resolutionHeight, true);

        Debug.Log($"Resolution set to {resolutionWidth}x{resolutionHeight}");
    }

    public void StartGame()
    {
        gameStarted = true;
        //Difficulty.instance.toggle.gameObject.SetActive(false);
        UIManager.instance.GameStart();
        ScoreManager.instance.StartScore();
        GameObject.Find("PlatformSpawner").GetComponent<PlatformSpawner>().StartSpawningPlatforms();
    }

    public void GameOver()
    {
        gameStarted = false;
        UIManager.instance.GameOver();
        ScoreManager.instance.StopScore();
        GameObject.Find("PlatformSpawner").GetComponent<PlatformSpawner>().StopSpawningPlatforms();
    }
    public void RandomizeColorFunction()
    {
        // Randomly choose whether to change Red, Green, or Blue
        int randomComponent = UnityEngine.Random.Range(0, 3); // 0 = Red, 1 = Green, 2 = Blue

        // Set the other two components to either 0 or 255
        int red = (randomComponent == 0) ? UnityEngine.Random.Range(0, 256) : UnityEngine.Random.Range(0, 2) * 255;
        int green = (randomComponent == 1) ? UnityEngine.Random.Range(0, 256) : UnityEngine.Random.Range(0, 2) * 255;
        int blue = (randomComponent == 2) ? UnityEngine.Random.Range(0, 256) : UnityEngine.Random.Range(0, 2) * 255;

        Debug.Log("Red: " + red + " Green: " + green + " Blue: " + blue);
        // Create a new Color with randomized values (normalized to 0-1 range)
        Color newColor = new Color(red / 255f, green / 255f, blue / 255f);
        platformColor = newColor;
        // Apply the color to the object's material
        /*if (objectRenderer != null)
        {
            objectRenderer.material.color = newColor; // Apply color to material
        }*/
    }
    public void ChangeColor()
    {
        if (platformColor != null && platformSpawner != null)
        {
            StartCoroutine(ChangeColorRoutine());
        }
    }
    private IEnumerator ChangeColorRoutine()
    {
        // Create a list from the keys of the dictionary to avoid modifying the collection during iteration
        List<GameObject> platformList = new List<GameObject>(platformSpawner.allPlatforms.Keys);
        foreach (var platform in platformList)
        {
            if (platform != null)
            {
                Renderer rend = platform.GetComponent<Renderer>();
                if (rend != null)
                {
                    StartCoroutine(LerpColor(rend, platformColor, transitionDuration));
                }
            }
            //yield return new WaitForSeconds(0.1f); // Delay between platforms
            yield return null; // Delay between platforms
        }
    }

    private IEnumerator LerpColor(Renderer rend, Color targetColor, float duration)
    {
        Color startColor = rend.material.color;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            rend.material.color = Color.Lerp(startColor, targetColor, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        rend.material.color = targetColor; // Ensure final color is applied
    }


/*public void LevelUp()
{
    UIManager.instance.LevelUp();
    ScoreManager.instance.StopScore();
    GameObject.Find("PlatformSpawner").GetComponent<PlatformSpawner>().StopSpawningPlatforms();
}*/
}
