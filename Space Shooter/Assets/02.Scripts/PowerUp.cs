using System;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public bool isShield;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);

            if (!isShield)
            {
                HealthManager.instance.ActivateShield();
            }
        }
    }
}
