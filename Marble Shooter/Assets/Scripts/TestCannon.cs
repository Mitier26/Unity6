using System;
using UnityEngine;
using DG.Tweening;

public class TestCannon : MonoBehaviour
{
    [SerializeField] private Transform cannon;
    [SerializeField] private float angleLimit = 50f;
    [SerializeField] private float duration = 2f;

    private float currentDirection; // 1 또는 -1
    private Tween rotationTween;
    
    public enum PlayerType { Player1, Player2, }

    [SerializeField] private PlayerType _playerType;
    [SerializeField] private Material[] _playerMaterials;

    private void Start()
    {
        // 태그, 레이어, 색 설전
        
        // 1. 랜덤 시작 각도
        float startAngle = UnityEngine.Random.Range(-angleLimit, angleLimit);
        cannon.localRotation = Quaternion.Euler(0, 0, startAngle);

        // 2. 랜덤 방향 설정
        currentDirection = UnityEngine.Random.value > 0.5f ? 1 : -1;

        // 3. 회전 시작
        StartRotating();
    }

    private void InitPlayer(PlayerType playerType)
    {
        this.tag = playerType.ToString();
        this.gameObject.layer = LayerMask.NameToLayer(playerType.ToString());
        
        // 색상/재질 설정
        var sr = cannon.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.material = _playerMaterials[(int)playerType];
        }
    }

    private void StartRotating()
    {
        // 현재 각도 가져오기
        float currentZ = cannon.localEulerAngles.z;
        if (currentZ > 180) currentZ -= 360; // -180 ~ 180으로 변환

        // 목표 각도 설정
        float targetZ = currentZ + angleLimit * currentDirection;

        rotationTween = cannon.DOLocalRotate(new Vector3(0, 0, targetZ), duration)
            .SetEase(Ease.Linear)
            .OnUpdate(CheckDirectionChange)
            .OnComplete(StartRotating); // 다음 회전 반복
    }

    private void CheckDirectionChange()
    {
        float currentZ = cannon.localEulerAngles.z;
        if (currentZ > 180) currentZ -= 360; // -180 ~ 180으로 변환

        // 회전 한계에 도달했으면 방향 반전
        if (Mathf.Abs(currentZ) >= angleLimit)
        {
            currentDirection *= -1;
            rotationTween.Kill(); // 현재 트윈 중단
            StartRotating();      // 반대 방향으로 재시작
        }
    }
}