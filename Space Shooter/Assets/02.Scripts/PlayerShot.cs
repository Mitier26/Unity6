using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    public float shotSpeed = 7f;
    public GameObject impactEffect;

    public GameObject effectObject;

    void Update()
    {
        transform.position += new Vector3(shotSpeed * Time.deltaTime, 0, 0);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        
        Instantiate(impactEffect, transform.position, transform.rotation);

        if (other.CompareTag("SpaceObject"))
        {
            Instantiate(effectObject, other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);
            
            GameManager.instance.AddScore(50);
        }

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().HurtEnemy();
        }

        if (other.CompareTag("Boss"))
        {
            BossManager.instance.HurtBoss();
        }
        
        Destroy(gameObject);
    }
    
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
