using System.Collections.Generic;
using UnityEngine;

public class InventoryDatabase : MonoBehaviour
{
    public static InventoryDatabase Instance { get; private set; }

    [Header("모든 인벤토리 아이템")]
    public List<InventoryItemData> allItems = new(); // 여기에 모든 아이템 등록

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 원한다면
    }

    public InventoryItemData GetItemByID(string id)
    {
        return allItems.Find(item => item.itemID == id);
    }
}