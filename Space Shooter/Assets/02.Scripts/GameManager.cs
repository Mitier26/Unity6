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

    public bool levelEnding;
    
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

    private void Update()
    {
        if (levelEnding)
        {
            PlayerController.instance.transform.position +=
                new Vector3(PlayerController.instance.boostSpeed * Time.deltaTime, 0f, 0f);
        }
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
            
            MusicController.instance.PlayGameOver();
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

    public IEnumerator EndLevelCo()
    {
        UIManager.instance.levelEndScreen.SetActive(true);

        PlayerController.instance.stopMovement = true;

        levelEnding = true;
        MusicController.instance.PlayVictory();
        
        yield return new WaitForSeconds(0.5f);
    }
    
}
