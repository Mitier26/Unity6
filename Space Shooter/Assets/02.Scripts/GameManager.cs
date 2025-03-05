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

    public void KillPlayer()
    {
        currentLives--;

        if ( currentLives > 0)
        {
            StartCoroutine(RespawnCoroutine());
        }
        else
        {
            
        }
    }

    public IEnumerator RespawnCoroutine()
    {
        
        yield return new WaitForSeconds(respawnTime);
        HealthManager.instance.Respawn();
        
        WaveManager.instance.ContinueSpawning();
    }
}
