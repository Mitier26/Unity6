using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableInfo : MonoBehaviour
{
    public ObjectInfoData objectInfoData;

    public InteractableType interactableType => objectInfoData != null
        ? objectInfoData.interactableType
        : InteractableType.DescriptionOnlyObject;

    public string objectName => objectInfoData != null ? objectInfoData.objectName : "이름 없음";
    public string description => objectInfoData != null ? objectInfoData.description : "설명 없음";
    
    public bool isHint = false;
    public bool showLabel = true;
    public bool showInteractionMessage = true;
}
