using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineInfo : MonoBehaviour
{
    public bool isHint;
    public bool isImportant;
    public bool isDanger;

    public Color GetOutlineColor()
    {
        if (isHint)
            return Color.yellow;
        else if (isImportant)
            return Color.cyan;
        else if (isDanger)
            return Color.red;
        else
            return Color.clear;
    }


    public bool ShouldHaveOutline() => isHint || isImportant || isDanger;
}
