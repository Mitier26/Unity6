using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom;
    public Color startColor, endColor; // 시작 지점와 끝 지점을 확인 하기 위한 색상

    public int distanceToEnd; // 몇 개의 방을 생성할 것인가.

    public Transform generatorPoint; // 맵 생성 위치

    public enum Direction
    {
        up,
        right,
        down,
        left
    }; // 맵이 생성 될 방향

    public Direction selectDirection;

    public float xOffset = 18f, yOffset = 10f;

    public LayerMask whatIsRoom;

    private GameObject endRoom;

    private List<GameObject> layoutRoomObjects = new List<GameObject>();

    public RoomPrefabs rooms;

    private List<GameObject> generatedOutlines = new List<GameObject>();

    public RoomCenter centerStart, centerEnd;
    public RoomCenter[] potentialCenter;

    private void Start()
    {
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color =
            startColor;

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

                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);

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

        // 아웃라인 방
        CreateRoomOutline(Vector3.zero);

        foreach (GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }

        CreateRoomOutline(endRoom.transform.position);

        // 마지막 방을 생성하고 색을 빨간색으로 한다.
        /*Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = endColor;

        selectDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();*/

        foreach (GameObject outline in generatedOutlines)
        {
            bool generateCenter = true;
            
            // 시작점인지 
            if (outline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                generateCenter = false;
            }

            if (outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                generateCenter = false;
            }

            // 코드의 순서상 처음과 마지막 지점이 아닐 때 만 생성한다.
            if (generateCenter)
            {
                // 현재는 적이 있는 방과 적이 없는 방 2개
                int centerSelect = Random.Range(0, potentialCenter.Length);

                Instantiate(potentialCenter[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

            }
            
            
            
        }
        
    }

    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        #endif
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

    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), 0.2f, whatIsRoom);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), 0.2f, whatIsRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), 0.2f, whatIsRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), 0.2f, whatIsRoom);

        int directionCount = 0;
        if (roomAbove)
        {
            directionCount++;
        }

        if (roomBelow)
        {
            directionCount++;
        }

        if (roomLeft)
        {
            directionCount++;
        }

        if (roomRight)
        {
            directionCount++;
        }

        switch (directionCount)
        {
            case 0:
                break;
            case 1:
                if (roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
                }

                if (roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
                }

                if (roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                }

                if (roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                }

                break;
            case 2:

                if (roomAbove && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
                }

                if (roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation));
                }

                if (roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
                }

                if (roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleRightDown, roomPosition, transform.rotation));
                }

                if (roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleDownLeft, roomPosition, transform.rotation));
                }

                if (roomLeft && roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftUp, roomPosition, transform.rotation));
                }

                break;
            case 3:

                if (roomAbove && roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpRightDown, roomPosition, transform.rotation));
                }

                if (roomRight && roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleRightDownLeft, roomPosition, transform.rotation));
                }

                if (roomBelow && roomLeft && roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleDownLeftUp, roomPosition, transform.rotation));
                }

                if (roomLeft && roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleLeftUpRight, roomPosition, transform.rotation));
                }

                break;
            case 4:

                if (roomAbove && roomBelow && roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.fourway, roomPosition, transform.rotation));
                }

                break;
        }

    }
}

[Serializable]
public class RoomPrefabs
{
    // 문 위치에 따른 오브젝트 정의
    public GameObject singleUp, singleDown, singleRight, singleLeft,
        doubleUpDown, doubleLeftRight, doubleUpRight, doubleRightDown, doubleDownLeft, doubleLeftUp,
        tripleUpRightDown, tripleRightDownLeft, tripleDownLeftUp, tripleLeftUpRight,
        fourway;
}
