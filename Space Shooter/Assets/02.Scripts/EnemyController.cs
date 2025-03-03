using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;

    public Vector2 startDirection;
    
    public bool shouldChangeDirection;
    public float changeDirectionXPoint;
    public Vector2 changeDirection;

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
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
