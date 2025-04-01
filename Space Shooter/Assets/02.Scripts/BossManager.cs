using System;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;

    public string bossName;
    public int currentHealth = 100;

    public BattleShot[] shotsToFire;
    
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
        
        MusicController.instance.PlayBoos();
    }

    private void Update()
    {
        for (int i = 0; i < shotsToFire.Length; i++)
        {
            shotsToFire[i].shotCounter -= Time.deltaTime;

            if (shotsToFire[i].shotCounter <= 0)
            {
                shotsToFire[i].shotCounter = shotsToFire[i].timeBetweenShots;
                Instantiate(shotsToFire[i].theShot, shotsToFire[i].firePoint.position, shotsToFire[i].firePoint.rotation);
            }
        }
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

[System.Serializable]
public class BattleShot
{
    public GameObject theShot;
    public float timeBetweenShots;
    public float shotCounter;
    public Transform firePoint;
}
