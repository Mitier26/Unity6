using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 방에 들어왔을 때 카메라의 타겟을 변경
            CamereController.instance.ChangeTarget(transform);
        }
    }
}
