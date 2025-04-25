using UnityEngine;
using UnityEngine.Timeline;

public enum StepTriggerType {
    Manual,
    Auto,
    PlayerInArea,
    InteractObject,
    ItemCollected,
}

[CreateAssetMenu(menuName = "Puzzle/Puzzle Step")]
public class PuzzleStepSO : ScriptableObject
{
    [Header("기본 정보")]
    [Tooltip("퍼즐 스텝의 고유 ID (예: Step01)")]
    public string stepID;

    [Tooltip("이 스텝의 이름 (예: 의사 인사, 상자 획득 등)")]
    public string stepName;

    [Tooltip("개발자용 설명. 이 스텝이 무엇을 의미하는지 간단히 작성")]
    [TextArea]
    public string description;

    [Header("게임 흐름")]
    [Tooltip("스텝의 진행 조건 유형")]
    public StepTriggerType triggerType;

    [Tooltip("이 스텝을 완료하기 위해 상호작용해야 하는 오브젝트")]
    public GameObject requiredObject;

    [Tooltip("힌트 강조 효과를 줄 오브젝트")]
    public GameObject hintTargetObject;

    [Header("UI 출력")]
    [Tooltip("하단 중앙에 출력할 대사 텍스트")]
    public string bottomDialogueText;

    [Tooltip("왼쪽 상단에 출력할 짧은 메시지")]
    public string topLeftMessageText;

    [Header("연출")]
    [Tooltip("이 스텝에서 실행할 Timeline 자산")]
    public TimelineAsset timeline;

    [Header("다음 단계")]
    [Tooltip("타임라인이 끝난 후에만 다음 단계로 진행할지 여부")]
    public bool waitForTimelineEnd;

    [Header("자동 진행")]
    [Tooltip("Auto 타입일 경우, 몇 초 후에 다음 스텝으로 넘어갈지 설정")]
    public float autoAdvanceDelay = 2f;

}
