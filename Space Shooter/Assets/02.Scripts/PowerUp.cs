using System;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public bool isShield;
    public bool isBoost;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);

            if (isShield)
            {
                HealthManager.instance.ActivateShield();
            }

            if (isBoost)
            {
                PlayerController.instance.ActivateSpeedBoost();
            }
            
        }
    }
}
