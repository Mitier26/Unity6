using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentLives = 3;
    
    public float respawnTime = 2.0f;

    public int currentScore;
    private int highScore;

    public bool levelEnding;

    private int levelScore;

    public float waitForLevelEnd = 5;

    public string nextLevel;

    private bool canPause;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        currentLives = PlayerPrefs.GetInt("CurrentLives");
        UIManager.instance.livesText.text =  $"X {currentLives}";
        
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        
        UIManager.instance.scoreText.text = "HI-Score: " + highScore;

        currentScore = PlayerPrefs.GetInt("CurrentScore");
        
        UIManager.instance.scoreText.text = $"Score: {currentScore}";
        
        canPause = true;
    }

    private void Update()
    {
        if (levelEnding)
        {
            PlayerController.instance.transform.position +=
                new Vector3(PlayerController.instance.boostSpeed * Time.deltaTime, 0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            PauseUnpause();
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
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.SetInt("CurrentLives", currentLives);

            canPause = false;
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

        levelScore += scoreToAdd;
        
        UIManager.instance.scoreText.text = $"Score: {currentScore}";

        if (currentScore > highScore)
        {
            highScore = currentScore;
            UIManager.instance.highScoreText.text = "HI-Score: " + highScore;
        }
    }

    public IEnumerator EndLevelCo()
    {
        UIManager.instance.levelEndScreen.SetActive(true);

        PlayerController.instance.stopMovement = true;

        levelEnding = true;
        MusicController.instance.PlayVictory();

        canPause = false;
        
        yield return new WaitForSeconds(0.5f);

        UIManager.instance.endLevelScore.text = "Level Score: " + levelScore;
        UIManager.instance.endLevelScore.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(0.5f);
        
        PlayerPrefs.SetInt("CurrentScore", currentScore);
        UIManager.instance.endCurrentScore.text = $"Total Score: {currentScore}";
        UIManager.instance.endCurrentScore.gameObject.SetActive(true);

        if (currentScore == highScore)
        {
            yield return new WaitForSeconds(0.5f);
            UIManager.instance.highScoreNotice.SetActive(true);
        }
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.SetInt("CurrentLives", currentLives);

        yield return new WaitForSeconds(waitForLevelEnd);

        SceneManager.LoadScene(nextLevel);
    }

    public void PauseUnpause()
    {
        if (UIManager.instance.pauseScreen.activeInHierarchy)
        {
            UIManager.instance.pauseScreen.SetActive(false);
            Time.timeScale = 1;
            PlayerController.instance.stopMovement = false;
        }
        else
        {
            UIManager.instance.pauseScreen.SetActive(true);
            Time.timeScale = 0f;
            PlayerController.instance.stopMovement = true;
        }
    }
}
