using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom;
    public Color startColor, endColor;  // 시작 지점와 끝 지점을 확인 하기 위한 색상
    
    public int distanceToEnd;   // 몇 개의 방을 생성할 것인가.

    public Transform generatorPoint;    // 맵 생성 위치

    public enum Direction { up, right, down, left };    // 맵이 생성 될 방향
    public Direction selectDirection;

    public float xOffset = 18f, yOffset = 10f;

    public LayerMask whatIsRoom;

    private GameObject endRoom;

    private List<GameObject> layoutRoomObjects = new List<GameObject>();
    
    private void Start()
    {
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;

        selectDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();

        for (int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

            layoutRoomObjects.Add(newRoom);

            // 마지막 방을
            if (i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count  - 1);
                
                endRoom = newRoom;
            }
            
            selectDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            // 겹치는 것이 있으면 겹치는 것이 없을 때 까지 반복한다.
            while (Physics2D.OverlapCircle(generatorPoint.position, 2f, whatIsRoom))
            {
                MoveGenerationPoint();
            }
            
        }
        
        // 마지막 방을 생성하고 색을 빨간색으로 한다.
        /*Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = endColor;

        selectDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();*/
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>
    /// 맵이 생성 위치를 이동시킨다.
    /// </summary>
    public void MoveGenerationPoint()
    {
        switch (selectDirection)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0);
                break;
            case Direction.down:
                generatorPoint.position -= new Vector3(0f, yOffset, 0);
                break;
            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0, 0);
                break;
            case Direction.left:
                generatorPoint.position -= new Vector3(xOffset, 0, 0);
                break;
        }
    }
}
