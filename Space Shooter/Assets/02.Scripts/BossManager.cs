using System;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;

    public int currentHealth = 100;
    
    
    private void Awake()
    {
        instance = this;
    }

    public void HurtBoss()
    {
        currentHealth--;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
