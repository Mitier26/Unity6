using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    public float shotSpeed = 7f;
    public GameObject impactEffect;

    void Update()
    {
        transform.position -= new Vector3(shotSpeed * Time.deltaTime, 0, 0);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        
        Instantiate(impactEffect, transform.position, transform.rotation);

        if(other.gameObject.CompareTag("Player"))
        {
            HealthManager.instance.HurtPlayer();
        }
        
        Destroy(gameObject);
    }
    
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
