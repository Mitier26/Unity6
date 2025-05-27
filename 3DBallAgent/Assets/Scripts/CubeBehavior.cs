using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehavior : MonoBehaviour
{
    public float cubeSpeed = 0.1f;

    public ObstacleGenerator obstacleGenerator;
    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;
        obstacleGenerator = FindAnyObjectByType<ObstacleGenerator>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 position = transform.position;
            position.x -= cubeSpeed;
            transform.position = position;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 position = transform.position;
            position.x += cubeSpeed;
            transform.position = position;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Reset();
        }
    }

    private void Reset()
    {
        transform.position = startingPosition;
        obstacleGenerator.RemoveObstacles();
    }
}

