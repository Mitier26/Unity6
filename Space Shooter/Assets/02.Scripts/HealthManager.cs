using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;
    
    public int currentHealth;
    public int maxHealth = 3;
    
    public GameObject deathEffect;

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
        currentHealth = maxHealth;
    }

    public void HurtPlayer()
    {
        currentHealth--;

        if (currentHealth <= 0)
        {
            Instantiate(deathEffect, transform.position, transform.rotation);
            gameObject.SetActive(false);
            
            GameManager.instance.KillPlayer();
            
            WaveManager.instance.canSpawnWaves = false;
        }
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        currentHealth = maxHealth;
    }
}
