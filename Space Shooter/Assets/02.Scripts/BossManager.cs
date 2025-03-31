using System;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;

    public string bossName;
    public int currentHealth = 100;
    
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UIManager.instance.bossName.text = bossName;
        UIManager.instance.bossSlider.maxValue = currentHealth;
        UIManager.instance.bossSlider.value = currentHealth;
        UIManager.instance.bossSlider.gameObject.SetActive(true);
    }

    public void HurtBoss()
    {
        currentHealth--;
        UIManager.instance.bossSlider.value = currentHealth;
        
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            UIManager.instance.bossSlider.gameObject.SetActive(false);
        }
    }
}
