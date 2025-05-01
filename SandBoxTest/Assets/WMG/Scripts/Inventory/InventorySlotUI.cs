using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    public Image iconImage;

    private InventoryItemData itemData;

    public void Setup(InventoryItemData data)
    {
        itemData = data;
        if (iconImage != null)
            iconImage.sprite = data.icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemData != null && itemData.prefab != null)
        {
            InventoryPreview.Instance.ShowItem(itemData.prefab);
            Debug.Log("슬롯 클릭 → 프리뷰 표시: " + itemData.displayName);
        }
    }
}