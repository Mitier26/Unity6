using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour, IInteractable
{
    public InteractableInfo info;
    public InventoryItemData inventoryData;
    
    private void Awake()
    {
        if (info == null)
            info = GetComponent<InteractableInfo>();
    }
    
    public void Interact()
    {
        if (info == null)
        {
            return;
        }
        
        switch (info.interactableType)
        {
            case InteractableType.InventoryObject:
                break;

            case InteractableType.CollectibleObject:
                CollectItem();
                break;

            case InteractableType.ActionObject:
                ObjectInspector.Instance.EnterInspectMode(gameObject);
                break;
        }
    }
    
    private void CollectItem()
    {
        inventoryData = info.objectInfoData.inventoryItemData;
        
        if (inventoryData == null)
        {
            Debug.LogWarning("inventoryData 가 비어 있습니다.");
            return;
        }

        InventoryManager.Instance.AddItem(inventoryData);
        Debug.Log($"인벤토리 추가됨: {inventoryData.displayName}");

        // 오브젝트 제거
        gameObject.SetActive(false); // 또는 Destroy(gameObject);
    }
}

