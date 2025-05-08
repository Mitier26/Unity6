using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("인벤토리 아이템 목록")]
    [SerializeField]
    private List<InventoryItemData> inventoryItems = new();

    public IReadOnlyList<InventoryItemData> Items => inventoryItems.AsReadOnly();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 원할 경우
    }
    
    private void Start()
    {
        // 인벤토리 불러오기
        var ids = SaveSystem.LoadInventory();
        inventoryItems.Clear();

        foreach (var id in ids)
        {
            var item = InventoryDatabase.Instance.GetItemByID(id);
            if (item != null)
                inventoryItems.Add(item);
        }

        Debug.Log($"인벤토리 불러오기 완료 ({inventoryItems.Count}개)");
    }


    /// <summary>
    /// 아이템 추가
    /// </summary>
    public void AddItem(InventoryItemData item)
    {
        if (!inventoryItems.Contains(item))
        {
            inventoryItems.Add(item);
            Debug.Log($"아이템 추가됨: {item.displayName}");
        }
    }

    /// <summary>
    /// 아이템 제거
    /// </summary>
    public void RemoveItem(InventoryItemData item)
    {
        if (inventoryItems.Contains(item))
        {
            inventoryItems.Remove(item);
            Debug.Log($"아이템 제거됨: {item.displayName}");
        }
    }

    /// <summary>
    /// 모든 아이템 반환
    /// </summary>
    public IReadOnlyList<InventoryItemData> GetAllItems()
    {
        return Items;
    }

    /// <summary>
    /// ID로 아이템 검색
    /// </summary>
    public InventoryItemData GetItemByID(string id)
    {
        return inventoryItems.Find(item => item.itemID == id);
    }

    /// <summary>
    /// 특정 프리팹과 연결된 아이템 찾기
    /// </summary>
    public InventoryItemData GetItemByPrefab(GameObject prefab)
    {
        return inventoryItems.Find(item => item.prefab == prefab);
    }

    /// <summary>
    /// 현재 인벤토리 내 아이템 개수
    /// </summary>
    public int Count => inventoryItems.Count;

    /// <summary>
    /// 인벤토리 비우기
    /// </summary>
    public void Clear()
    {
        inventoryItems.Clear();
    }
}