using System;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool closeWhenEntered, openWhenEnemiesCleared;

    public GameObject[] doors;
    
    public List<GameObject> enemies = new List<GameObject>();

    private bool roomActive;

    private void Update()
    {
        if(enemies.Count > 0 && roomActive && openWhenEnemiesCleared)
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }

            if (enemies.Count == 0)
            {
                // 모든 문을 열음
                foreach (GameObject door in doors)
                {
                    door.SetActive(false);
                    
                    closeWhenEntered = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 방에 들어왔을 때 카메라의 타겟을 변경
            CamereController.instance.ChangeTarget(transform);
            
            // 방에 들어왔을 때 문을 닫는 경우
            if (closeWhenEntered)
            {
                // 모든 문을 닫음
                foreach (GameObject door in doors)
                {
                    door.SetActive(true);
                }
            }

            roomActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            roomActive = false;
        }
    }
}
