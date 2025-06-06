using System;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어에게 데미지 주기
            PlayerHealthController.instance.DamagePlayer();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어에게 데미지 주기
            PlayerHealthController.instance.DamagePlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 플레이어에게 데미지 주기
            PlayerHealthController.instance.DamagePlayer();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 플레이어에게 데미지 주기
            PlayerHealthController.instance.DamagePlayer();
        }
    }
}
