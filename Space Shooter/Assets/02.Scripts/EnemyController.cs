using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;

    public Vector2 startDirection;
    
    public bool shouldChangeDirection;
    public float changeDirectionXPoint;
    public Vector2 changeDirection;
    
    public GameObject shotToFire;
    public Transform firePoint;
    public float timeBetweenShots;
    private float shotCounter;

    public bool canShoot;
    private bool allowShooting;
    
    public float currentHealth;
    public GameObject deathEffect;

    public int scoreValue = 100;

    public GameObject[] powerUps;
    public int dropSuccessRate = 15;
    
    private void Start()
    {
        shotCounter = timeBetweenShots;
    }

    private void Update()
    {
        if (!shouldChangeDirection)
        {
            transform.position += new Vector3(startDirection.x * moveSpeed * Time.deltaTime, startDirection.y * moveSpeed * Time.deltaTime, 0);
        }
        else
        {
            if (transform.position.x > changeDirectionXPoint)
            {
                transform.position += new Vector3(startDirection.x * moveSpeed * Time.deltaTime, startDirection.y * moveSpeed * Time.deltaTime, 0);
            }
            else
            {
                transform.position += new Vector3(changeDirection.x * moveSpeed * Time.deltaTime, changeDirection.y * moveSpeed * Time.deltaTime, 0);
            }
        }
        
        if (allowShooting)
        {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                shotCounter = timeBetweenShots;
                Instantiate(shotToFire, firePoint.position, firePoint.rotation);
            }
        }
    }

    public void HurtEnemy()
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            GameManager.instance.AddScore(scoreValue);

            int randomChance = Random.Range(0, 100);
            
            if (randomChance < dropSuccessRate)
            {
                int randomPick = Random.Range(0, powerUps.Length);

                Instantiate(powerUps[randomPick], transform.position, quaternion.identity);
            }
            
            Destroy(gameObject);
            Instantiate(deathEffect, transform.position, transform.rotation);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnBecameVisible()
    {
        if(canShoot)
            allowShooting = true;
    }
}
