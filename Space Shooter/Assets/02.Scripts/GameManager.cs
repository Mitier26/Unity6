using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentLives = 3;
    
    public float respawnTime = 2.0f;

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
}
