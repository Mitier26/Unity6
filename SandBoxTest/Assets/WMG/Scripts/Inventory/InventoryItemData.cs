using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class InventoryItemData : ScriptableObject
{
    public string itemID;
    public string displayName;
    public Sprite icon;
    public GameObject prefab;
    public string description;
}

