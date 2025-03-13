using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject zigzagPanel;
    public GameObject gameOverPanel;
    public GameObject levelUpPanel;
    public GameObject tapText;
    public Text score;
    public Text highScore1;
    public Text highScore2;
    public Text levelText;
    public Text levelText2;
    public Text levelText3;

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
        highScore1.text = "High Score: " + PlayerPrefs.GetInt("highScore");
        levelText.GameObject().SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameStart()
    {
        tapText.SetActive(false);
        zigzagPanel.GetComponent<Animator>().Play("panelUp");
    }
    public void GameOver()
    {
        score.text = PlayerPrefs.GetInt("score").ToString();
        highScore2.text = PlayerPrefs.GetInt("highScore").ToString();
        levelText2.text = "Level: "+ PlayerPrefs.GetInt("currentLevel").ToString();
        levelText.GameObject().SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void LevelUp()
    {
        int currentLevel = PlayerPrefs.GetInt("currentLevel");
        setLevel(currentLevel);
        levelText3.text = "Level: "+ currentLevel.ToString();
        levelText.GameObject().SetActive(false);
        levelUpPanel.SetActive(true);
    }
    
    public void setLevel(int level)
    {
        levelText.text = "Level: "+level.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ContinueLevel()
    {
        SceneManager.LoadScene(0);
    }
}
