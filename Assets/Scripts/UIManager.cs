using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gameStartPanel;
    public GameObject gameOverPanel;
    public GameObject tapText;
    public Text score;
    public Text highScore1;
    public Text highScore2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameStart()
    {
        tapText.SetActive(false);
        gameStartPanel.GetComponent<Animator>().Play("panelUp");
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }
}
