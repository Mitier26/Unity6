using System;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ActivateHint();
        }
    }

    public void ActivateHint()
    {
        var hintTargets = FindObjectsOfType<InteractableInfo>();

        foreach (var info in hintTargets)
        {
            if (info.isHint)
            {
                var outline = info.GetComponent<OutlineController>();
                if (outline != null)
                    outline.ForceEnableByHint(Color.yellow);
            }
        }
    }

    public void DeactivateHint()
    {
        var outlines = FindObjectsOfType<OutlineController>();
        foreach (var outline in outlines)
        {
            outline.ClearHintForce();
        }
    }
}