using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject zigzagPanel;
    public GameObject gameOverPanel;
    public GameObject tapText;
    public Text score;
    public Text highScore1;
    public Text highScore2;

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
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
