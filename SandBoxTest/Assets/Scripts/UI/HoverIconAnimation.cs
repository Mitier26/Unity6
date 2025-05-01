using UnityEngine;
using UnityEngine.EventSystems;

public class HoverIconAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverIcon; // 마우스 오버 시 표시될 아이콘
    public float floatSpeed = 1.0f; // 움직임 속도
    public float floatAmount = 0.2f; // 움직임 정도
    
    private Vector3 startPos;
    private bool isHovering = false;
    
    private void Start()
    {
        // 시작 시 아이콘 숨기기
        if (hoverIcon != null)
        {
            startPos = hoverIcon.transform.localPosition;
            hoverIcon.SetActive(false);
        }
    }
    
    private void Update()
    {
        if (isHovering && hoverIcon != null && hoverIcon.activeSelf)
        {
            // 부드러운 위아래 움직임 애니메이션
            float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
            hoverIcon.transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
        }
    }
    
    // 마우스가 오브젝트 위에 올라갔을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        if (hoverIcon != null)
        {
            hoverIcon.SetActive(true);
        }
    }
    
    // 마우스가 오브젝트를 벗어났을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        if (hoverIcon != null)
        {
            hoverIcon.SetActive(false);
        }
    }
}