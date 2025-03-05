using System;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    private void Start()
    {
        transform.DetachChildren();
        Destroy(gameObject);
    }
}
