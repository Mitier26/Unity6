using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    public float shotSpeed = 7f;


    void Update()
    {
        transform.position += new Vector3(shotSpeed * Time.deltaTime, 0, 0);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
    
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
