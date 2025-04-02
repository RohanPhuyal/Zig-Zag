using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int score;
    public int highScore;
    //private int currentLevel;
    
    //private int levelThreshold = 200; // Score required for the next level
    //private int currentLevelScore;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        //currentLevelScore = 0;
        //currentLevel = PlayerPrefs.GetInt("currentLevel", 1); // Load the current level, default to level 1
        //UIManager.instance.setLevel(currentLevel);
        PlayerPrefs.SetInt("score", score);
        //PlayerPrefs.SetInt("currentLevel", currentLevel); // Ensure the level starts correctly
        //score = currentLevel*levelThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IncrementScore()
    {
        score += 1;
        UIManager.instance.updateScoreUI(score);
        if (score % 50 == 0)
        {
            GameManager.instance.RandomizeColorFunction();
            GameManager.instance.ChangeColor();
        }
        
        //currentLevelScore += 1;
        // Check if the score exceeds the level milestone
        //CheckLevelCompletion();
    }

    public void StartScore()
    {
        InvokeRepeating("IncrementScore",0.1f,0.5f);
    }

    public void StopScore()
    {
        CancelInvoke("IncrementScore");
        PlayerPrefs.SetInt("score", score);

        if (PlayerPrefs.HasKey("highScore"))
        {
            if (score > PlayerPrefs.GetInt("highScore"))
            {
                PlayerPrefs.SetInt("highScore", score);
                highScore = score;
            }
        }
        else
        {
            PlayerPrefs.SetInt("highScore", score);
            highScore = score;
        }
    }
    
    // Check if the player has passed the level threshold
    /*void CheckLevelCompletion()
    {
        int nextLevelThreshold = currentLevel * levelThreshold;

        if (currentLevelScore >= nextLevelThreshold)
        {
            // Level is complete, so update the current level
            currentLevel++;
            PlayerPrefs.SetInt("currentLevel", currentLevel); // Save the current level progress
            Debug.Log("LEVEL UP: "+currentLevel);
            // Optionally display a "level complete" message or update UI
            GameObject.Find("Ball").GetComponent<BallController>().LevelUp();
        }
    }
    */
}
