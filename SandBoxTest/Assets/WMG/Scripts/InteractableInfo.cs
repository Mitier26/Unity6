using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableInfo : MonoBehaviour
{
    [Header("정보")]
    
    public InteractableType interactableType = InteractableType.DescriptionOnlyObject;
    public bool isHint = false;
    
    public string objectName = "이름 없는 물체";
    [TextArea]
    public string description = "설명이 없습니다.";

    public bool showLabel = true;
    public bool showInteractionMessage = true;
}
