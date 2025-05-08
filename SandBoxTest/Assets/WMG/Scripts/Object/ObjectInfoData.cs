using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Object/Object Info Data")]
public class ObjectInfoData : ScriptableObject
{
    public string id;
    public string objectName;
    [TextArea] public string description;
    public InteractableType interactableType;
    public Sprite previewImage;
    
    public InventoryItemData inventoryItemData;
}
