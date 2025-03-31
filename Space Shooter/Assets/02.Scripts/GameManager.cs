using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentLives = 3;
    
    public float respawnTime = 2.0f;

    public int currentScore;
    public int highScore = 500;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UIManager.instance.livesText.text =  $"X {currentLives}";

        UIManager.instance.scoreText.text = $"Score: {currentScore}";

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        
        UIManager.instance.scoreText.text = "HI-Score: " + highScore;
    }
    

    public void KillPlayer()
    {
        currentLives--;
        UIManager.instance.livesText.text =  $"X {currentLives}";

        if ( currentLives > 0)
        {
            StartCoroutine(RespawnCoroutine());
        }
        else
        {
            UIManager.instance.gameOverScreen.SetActive(true);
            WaveManager.instance.canSpawnWaves = false;
        }
    }

    public IEnumerator RespawnCoroutine()
    {
        
        yield return new WaitForSeconds(respawnTime);
        HealthManager.instance.Respawn();
        
        WaveManager.instance.ContinueSpawning();
    }

    public void AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        
        UIManager.instance.scoreText.text = $"Score: {currentScore}";

        if (currentScore > highScore)
        {
            highScore = currentScore;
            UIManager.instance.scoreText.text = "HI-Score: " + highScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }
}
