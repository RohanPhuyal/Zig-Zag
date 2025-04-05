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
    //public Text levelText;
    //public Text levelText2;
    //public Text levelText3;
    public Text currentScore;
    public GameObject settings;
    public GameObject settingsPanel;
    public Dropdown graphicsQuality;

    public GameObject exit;
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
        // Add a listener to the dropdown
        graphicsQuality.onValueChanged.AddListener(OnDropdownValueChanged);
        settingsPanel.SetActive(false);
        //levelText.GameObject().SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameStart()
    {
        settingsPanel.SetActive(false);
        tapText.SetActive(false);
        exit.SetActive(false);
        currentScore.gameObject.SetActive(true);
        zigzagPanel.GetComponent<Animator>().Play("panelUp");
    }
    public void GameOver()
    {
        highScore2.text = PlayerPrefs.GetInt("highScore").ToString();
        //levelText2.text = "Level: "+ PlayerPrefs.GetInt("currentLevel").ToString();
        //levelText.GameObject().SetActive(false);
        levelUpPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void updateScoreUI(int score)
    {
        currentScore.text = "Score: "+ score.ToString();
        this.score.text= score.ToString();
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void onSettings()
    {
        if(PlayerPrefs.HasKey("quality"))
        {
            string value = PlayerPrefs.GetString("quality");
            for (int i = 0; i < graphicsQuality.options.Count; i++)
            {
                if (graphicsQuality.options[i].text == value)
                {
                    graphicsQuality.value = i; // Set the selected index
                    break;
                }
            }
        }
        Debug.Log(PlayerPrefs.GetString("quality"));
        if (settingsPanel.activeInHierarchy == false)
        {
            settingsPanel.SetActive(true);
        }
        else
        {
            settingsPanel.SetActive(false);
        }
    }
    // The callback function for when the value changes
    void OnDropdownValueChanged(int index)
    {
        // Get the selected value by index
        string selectedValue = graphicsQuality.options[index].text;
        PlayerPrefs.SetString("quality", selectedValue);

        GameManager.instance.SetManualQualityLevel(selectedValue);
    }

/*public void LevelUp()
{
    int currentLevel = PlayerPrefs.GetInt("currentLevel");
    setLevel(currentLevel);
    levelText3.text = "Level: "+ currentLevel.ToString();
    levelText.GameObject().SetActive(false);
    gameOverPanel.SetActive(false);
    levelUpPanel.SetActive(true);
}

public void setLevel(int level)
{
    levelText.text = "Level: "+level.ToString();
}*/

public void RestartGame()
{
    SceneManager.LoadScene(0);
}

public void ContinueLevel()
{
    SceneManager.LoadScene(0);
}
}
