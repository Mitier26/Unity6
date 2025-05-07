using System;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    
    public int maxHealth;
    public int currentHealth;

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
    
    public void DamagePlayer()
    {
        currentHealth--;
        
        // UI 업데이트
        UIController.instance.UpdateHealthUI(currentHealth, maxHealth);
        
        if (currentHealth <= 0)
        {
            // 플레이어 사망 처리
            PlayerDie();
        }
    }
    
    private void PlayerDie()
    {
        PlayerController.instance.gameObject.SetActive(false);
    }
}
