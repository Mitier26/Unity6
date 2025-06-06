using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;
    
    public int currentHealth;
    public int maxHealth = 3;
    
    public GameObject deathEffect;

    public float invincibleLength;
    private float invincCounter;

    public SpriteRenderer theSR;

    public int shieldPwr;
    public int shieldMaxPwr;
    public GameObject theShield;

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

        UIManager.instance.healthBar.maxValue = maxHealth;
        UIManager.instance.healthBar.value = currentHealth;

        UIManager.instance.shieldBar.maxValue = shieldMaxPwr;
        UIManager.instance.shieldBar.value = shieldPwr;
        
    }

    private void Update()
    {
        if (invincCounter >= 0)
        {
            invincCounter -= Time.deltaTime;

            if (invincCounter <= 0)
            {
                theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 1);
            }
        }
    }

    public void HurtPlayer()
    {
        if (invincCounter <= 0)
        {
            if (theShield.activeInHierarchy)
            {
                shieldPwr--;

                if (shieldPwr <= 0)
                {
                    theShield.SetActive(false);
                }
                
                UIManager.instance.shieldBar.value = shieldPwr;
            }
            else
            {
                currentHealth--;
            
                UIManager.instance.healthBar.value = currentHealth;

                if (currentHealth <= 0)
                {
                    Instantiate(deathEffect, transform.position, transform.rotation);
                    gameObject.SetActive(false);
            
                    GameManager.instance.KillPlayer();
            
                    WaveManager.instance.canSpawnWaves = false;

                    PlayerController.instance.doubleShotActive = false;
                }
            }
        }
        
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        currentHealth = maxHealth;
        UIManager.instance.healthBar.value = currentHealth;

        invincCounter = invincibleLength;
        theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 0.5f);
    }

    public void ActivateShield()
    {
        theShield.SetActive(true);
        shieldPwr = shieldMaxPwr;
        
        UIManager.instance.shieldBar.value = shieldPwr;
    }
}
