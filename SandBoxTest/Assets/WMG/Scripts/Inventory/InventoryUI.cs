using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform slotParent;
    public GameObject slotPrefab;

    private void OnEnable()
    {
        var items = InventoryManager.Instance.GetAllItems();
        RefreshUI(items);
    }

    public void RefreshUI(IReadOnlyList<InventoryItemData> items)
    {
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        foreach (var item in items)
        {
            var slot = Instantiate(slotPrefab, slotParent);
            var slotUI = slot.GetComponent<InventorySlotUI>();
            slotUI.Setup(item);
        }
    }
}

