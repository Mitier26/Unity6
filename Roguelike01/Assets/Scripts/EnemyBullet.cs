using System;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 7.5f;
    private Vector3 moveDirection;

    private void Start()
    {
        // 총알이 발사될 방향을 설정
        moveDirection = (PlayerController.instance.transform.position - transform.position).normalized;
    }
    
    private void Update()
    {
        // 총알을 발사 방향으로 이동
        transform.position += moveDirection * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthController.instance.DamagePlayer();
            Destroy(gameObject);
        }
        
        AudioManager.instance.PlaySfx(4);
    }

    private void OnBecameInvisible()
    {
        // 화면에서 벗어났을 때 총알 삭제
        Destroy(gameObject);
    }
}
