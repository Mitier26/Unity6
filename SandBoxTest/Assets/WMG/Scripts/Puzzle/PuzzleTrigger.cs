using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == false)
            return;

        var step = PuzzleManager.Instance.CurrentStep;
        if (step.triggerType == StepTriggerType.PlayerInArea)
        {
            PuzzleManager.Instance.AdvanceStep();
        }
    }
}
