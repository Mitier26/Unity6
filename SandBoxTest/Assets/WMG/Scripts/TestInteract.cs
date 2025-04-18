using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteract : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("상호작용 실행: TestBox!");
        // 조사 모드 진입 / 퍼즐 시작 등 연결 가능
    }
}
