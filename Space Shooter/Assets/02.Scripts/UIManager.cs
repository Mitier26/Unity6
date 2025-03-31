using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    public GameObject gameOverScreen;

    public TextMeshProUGUI livesText;

    public Slider healthBar, shieldBar;
    
    public TextMeshProUGUI scoreText, highScoreText;

    public GameObject levelEndScreen;

    public TextMeshProUGUI endLevelScore, endCurrentScore;
    public GameObject highScoreNotice;

    public GameObject pauseScreen;

    public string mainMenuName = "MainMenu";

    public Slider bossSlider;
    public TextMeshProUGUI bossName;

    private void Awake()
    {
        instance = this;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void QuitToMain()
    {
        SceneManager.LoadScene(mainMenuName);
        Time.timeScale = 1f;
    }

    public void Resume()
    {
        GameManager.instance.PauseUnpause();
    }
}
