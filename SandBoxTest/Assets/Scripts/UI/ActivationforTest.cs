using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ActivationforTest : MonoBehaviour
{
    public GameObject targetPanel;
    public Transform rotatingChild;
    private CanvasGroup panelCanvasGroup;
    public float rotationSpeed = 360f; // 회전 속도(초당 각도)
    private Tween rotationTween;

    void Start()
    {
        // 캔버스 그룹 자동 생성
        panelCanvasGroup = targetPanel.GetComponent<CanvasGroup>() ?? targetPanel.AddComponent<CanvasGroup>();
        StartCoroutine(PanelActivationCycle());
    }

    IEnumerator PanelActivationCycle()
    {
        while (true)
        {
            // 초기화
            panelCanvasGroup.alpha = 0;
            rotatingChild.localRotation = Quaternion.identity;
            targetPanel.SetActive(true);

            // 페이드 인
            panelCanvasGroup.DOFade(1, 0.3f).SetEase(Ease.OutQuad);
            
            // 활성화 상태일 때 무한 회전 시작
            rotationTween = rotatingChild.DORotate(new Vector3(0, 0, -360), 360f / rotationSpeed, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart); // -1은 무한 반복을 의미

            // 3초 대기 (활성화 상태 유지)
            yield return new WaitForSeconds(3f);

            // 회전 중지
            if (rotationTween != null)
            {
                rotationTween.Kill();
                rotationTween = null;
            }

            // 페이드 아웃 시작
            panelCanvasGroup.DOFade(0, 0.3f).SetEase(Ease.InQuad);
            yield return new WaitForSeconds(1f);

            // 완전 비활성화
            targetPanel.SetActive(false);
            
            yield return new WaitForSeconds(7f);
        }
    }

    private void OnDestroy()
    {
        // 스크립트가 제거될 때 모든 트윈 정리
        if (rotationTween != null)
        {
            rotationTween.Kill();
        }
    }
}