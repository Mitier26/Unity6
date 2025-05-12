using System;
using UnityEngine;

public class CamareController : MonoBehaviour
{
    public static CamareController instance;

    public float moveSpeed;

    public Transform target;

    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }
    }
}
