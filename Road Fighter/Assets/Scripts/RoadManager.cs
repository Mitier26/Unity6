using System;
using UnityEngine;
using System.Collections.Generic;

public class RoadManager : MonoBehaviour
{
    // 프리팹으로 로도 생성
    public GameObject roadPrefab;
    public GameObject goalLinePrefab;
    public GameObject startLinePrefab;
    public Transform spawnPoint;
    public Transform roadContainer;
    
    public int roadCount = 20;
    public float roadWidth = 2f;
    public int startLineIndex = 9;

    public float scrollSpeed = 1f;
    public float replacementDistance = -2f;

    private void Start()
    {
        for (int i = 0; i < roadCount; i++)
        {
            if (i == startLineIndex)
            {
                var startLine = Instantiate(startLinePrefab, new Vector3(0, i , 0), Quaternion.identity, roadContainer);
                startLine.GetComponent<RoadAssembler>().SetRoadWidth(roadWidth);
            }
            
            var road = Instantiate(roadPrefab, new Vector3(0, i , 0), Quaternion.identity, roadContainer);
            road.GetComponent<RoadAssembler>().SetRoadWidth(roadWidth);
        }
    }
    
    private void Update()
    {
        float moveAmount = scrollSpeed * Time.deltaTime;

        foreach (Transform road in roadContainer)
        {
            road.position += new Vector3(0, -moveAmount, 0);
            
            if(road.position.y < replacementDistance)
            {
                if (road.CompareTag("StartLine"))
                {
                    Destroy(road.gameObject);
                }
                road.position += spawnPoint.position;
            }
        }
    }
}