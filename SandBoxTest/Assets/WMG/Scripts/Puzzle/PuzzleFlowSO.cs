using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Puzzle/Puzzle Flow")]
public class PuzzleFlowSO : ScriptableObject
{
    public string flowName = "튜토리얼 방 퍼즐";

    [Header("퍼즐 단계 순서")]
    public List<PuzzleStepSO> steps = new List<PuzzleStepSO>();
}