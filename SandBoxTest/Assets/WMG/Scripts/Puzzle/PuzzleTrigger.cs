using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PuzzleTrigger : MonoBehaviour
{
    public Vector3 moveToPosition;     // 이동할 위치 (Inspector에서 설정)
    public Vector3 moveToRotationEuler; // 이동할 회전 (Inspector에서 설정)
    
    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered) return; // 중복 방지
        if (other.CompareTag("Player") == false) return;

        
        Debug.Log("트리거 엔터");
        var step = PuzzleManager.Instance.CurrentStep;
        if (step.triggerType == StepTriggerType.PlayerInArea)
        {
            PuzzleManager.Instance.SetCutsceneState(true);
            isTriggered = true;
            StartCoroutine(MovePlayerAndAdvance(other.transform));
        }
    }

    private IEnumerator MovePlayerAndAdvance(Transform playerTransform)
    {
        // 1. 페이드 아웃
        var fade = FindObjectOfType<FadeController>();
        yield return fade.FadeOut();

        // 2. 순간 이동
        playerTransform.position = moveToPosition;
        playerTransform.rotation = Quaternion.Euler(moveToRotationEuler);
        
        // 2-1. 플레이어 하위 메인 카메라 회전 초기화
        var camera = Camera.main;
        if (camera != null)
        {
            camera.transform.localRotation = Quaternion.identity; // (0,0,0)
        }

        // 3. 페이드 인
        yield return fade.FadeIn();
        
        // 4. 이동이 끝난 후 AdvanceStep
        PuzzleManager.Instance.AdvanceStep();
    }
}