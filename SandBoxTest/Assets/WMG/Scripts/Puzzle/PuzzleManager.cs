using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public PuzzleFlowSO puzzleFlow;         // 퍼즐 흐름 데이터
    private int currentStepIndex = -1;

    public PuzzleStepSO CurrentStep => puzzleFlow.steps[currentStepIndex];

    public void AdvanceStep()
    {
        if (currentStepIndex < puzzleFlow.steps.Count - 1)
        {
            currentStepIndex++;
            TriggerStep(CurrentStep);
        }
        else
        {
            Debug.Log("퍼즐 완료!");
        }
    }

    private void TriggerStep(PuzzleStepSO step)
    {
        Debug.Log("현재 스텝: " + step.stepName);
        // 힌트 하이라이트
        // 메시지 출력
        // Timeline 실행 등 처리
    }

    // 테스트용
    [ContextMenu("다음 스텝 테스트")]
    public void TestAdvance()
    {
        AdvanceStep();
    }

    private void Update()
    {
        // 테스트용: 스페이스바로 다음 스텝 진행
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceStep();
        }
    }
}
