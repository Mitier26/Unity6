using System;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public bool isShield;
    public bool isBoost;
    public bool isDoubleShot;

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

            if (isDoubleShot)
            {
                PlayerController.instance.doubleShotActive = true;
            }
            
        }
    }
}
