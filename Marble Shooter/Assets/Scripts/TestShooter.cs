using System;
using UnityEngine;

public class TestShooter : MonoBehaviour
{
    [SerializeField] private Transform shooter;
    [SerializeField] private GameObject bullet;
    
    // 총알을 공용으로 사용 할 것
    // 총알이 누구의 것인지 알 필요가 있다.
    // CannonManager에서 대포와 총알을 관리하는 것이 편할 것 같다.
    // 지금은 대포마다 각각의 스크립트를 가지고 있다.
    
    // 총알을 발사 할 때 총알의 색, 태그, 레이어 변경해야 함.

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
        }
    }

}
