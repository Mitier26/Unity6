using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleGenerator : MonoBehaviour
{
    public Transform[] spawnPoints;                 // 장애물, 점수 생성될 위치
    public GameObject obstaclePrefab;               // 장애물 프리팹
    public GameObject scorePrefab;                  // 점수 프리팹
    public GameObject agent;                        // 에이전트
    public float forwardForce = 15f;                // 장애물 속도
    public float timeBetweenWaves = 1f;             // 장애물 생성 간격
    private List<GameObject> obstacles = new List<GameObject>();

    private void Start()
    {
        InvokeRepeating("GenerateObstacle", 0f, timeBetweenWaves);
    }

    public void GenerateObstacle()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (randomIndex != i)
            {
                GameObject ob = Instantiate(obstaclePrefab, spawnPoints[i].position, Quaternion.identity);
                ob.tag = "Obstacle";
                obstacles.Add(ob);
            }
            else
            {
                GameObject ob = Instantiate(scorePrefab, spawnPoints[i].position, Quaternion.identity);
                ob.tag = "Score";
                obstacles.Add(ob);
            }
            
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < obstacles.Count; i++)
        {
            if (obstacles[i])
            {
                obstacles[i].GetComponent<Rigidbody>().AddForce(0, 0, -forwardForce * Time.fixedDeltaTime, ForceMode.VelocityChange);
                if (obstacles[i].transform.position.z < agent.transform.position.z - 10)
                {
                    Destroy(obstacles[i]);
                }
            }
        }
    }

    public void RemoveObstacles()
    {
        for (int i = 0; i < obstacles.Count; i++)
        {
            if (obstacles[i])
            {
                Destroy(obstacles[i]);
            }
        }
    }
}
