using System;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 1;
    
    public float waitToBeCollected = 0.5f;

    private void Update()
    {
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && waitToBeCollected <= 0)
        {
            // 플레이어에게 체력 회복
            PlayerHealthController.instance.HealPlayer(healAmount);
            Destroy(gameObject);
            AudioManager.instance.PlaySfx(7);
        }
    }
}
