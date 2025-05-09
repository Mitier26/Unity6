using UnityEngine;
using UnityEngine.EventSystems;

public class TestClick : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("슬롯 클릭됨");
    }
}
