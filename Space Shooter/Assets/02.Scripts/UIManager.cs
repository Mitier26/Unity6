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

    private void Awake()
    {
        instance = this;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMain()
    {
        
    }
}
