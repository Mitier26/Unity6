using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float moveSpeed;

    void Update()
    {
        transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);  
    }
}
