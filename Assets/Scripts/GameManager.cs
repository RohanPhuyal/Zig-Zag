using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool gameOver;
    public bool gameStarted;
    public Color platformColor;
    void Awake(){
        Application.targetFrameRate = 60;
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOver = false;
        gameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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

        // Create a new Color with randomized values (normalized to 0-1 range)
        Color newColor = new Color(red / 255f, green / 255f, blue / 255f);
        platformColor = newColor;
        // Apply the color to the object's material
        /*if (objectRenderer != null)
        {
            objectRenderer.material.color = newColor; // Apply color to material
        }*/
    }

/*public void LevelUp()
{
    UIManager.instance.LevelUp();
    ScoreManager.instance.StopScore();
    GameObject.Find("PlatformSpawner").GetComponent<PlatformSpawner>().StopSpawningPlatforms();
}*/
}
