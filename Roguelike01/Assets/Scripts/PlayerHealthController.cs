using System;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    
    public int maxHealth;
    public int currentHealth;

    public float damageInvincLength = 1f;
    private float damageInvincCounter;

    
    private void Awake()
    {
        if (instance == null)
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
        
        UIController.instance.UpdateHealthUI(currentHealth, maxHealth);
        
    }

    private void Update()
    {
        if (damageInvincCounter > 0)
        {
            damageInvincCounter -= Time.deltaTime;

            if (damageInvincCounter <= 0)
            {
                PlayerController.instance.bodySprite.color = new Color(PlayerController.instance.bodySprite.color.r, PlayerController.instance.bodySprite.color.g, PlayerController.instance.bodySprite.color.b, 1f);
            }
        }
    }

    public void DamagePlayer()
    {
        if (damageInvincCounter <= 0)
        {
            AudioManager.instance.PlaySfx(11);
            
            currentHealth--;

            damageInvincCounter = damageInvincLength;
           
            PlayerController.instance.bodySprite.color = new Color(PlayerController.instance.bodySprite.color.r, PlayerController.instance.bodySprite.color.g, PlayerController.instance.bodySprite.color.b, 0.5f);
            
            // UI 업데이트
            UIController.instance.UpdateHealthUI(currentHealth, maxHealth);
        
            if (currentHealth <= 0)
            {
                // 플레이어 사망 처리
                PlayerDie();
            }
        }
    }
    
    private void PlayerDie()
    {
        AudioManager.instance.PlaySfx(8);
        AudioManager.instance.PlayGameOver();
        PlayerController.instance.gameObject.SetActive(false);
        UIController.instance.deathScreen.SetActive(true);
    }

    public void MakeInvincible(float length)
    {
        damageInvincCounter = length;
        PlayerController.instance.bodySprite.color = new Color(PlayerController.instance.bodySprite.color.r, PlayerController.instance.bodySprite.color.g, PlayerController.instance.bodySprite.color.b, 0.5f);
    }
    
    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.UpdateHealthUI(currentHealth, maxHealth);
    }
}
