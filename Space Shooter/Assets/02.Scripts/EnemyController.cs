using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;

    private void Update()
    {
        transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
