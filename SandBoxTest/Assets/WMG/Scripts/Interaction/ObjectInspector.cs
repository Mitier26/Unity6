using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class ObjectInspector : MonoBehaviour
{
    public static ObjectInspector Instance;

    [Header("조사용 피봇 위치")]
    public Transform inspectPivot;

    [Header("검사 모드 설정")]
    public float moveDuration = 0.5f;
    public float rotateSpeed = 200f;
    public float scaleMultiply = 0.3f;
    
    [Header("확대/축소 설정")]
    public float zoomSpeed = 0.5f;
    public float minScale = 0.2f;
    public float maxScale = 2.0f;
    
    [Header("연결된 UI")]
    public WorldLabelUI worldLabelUI;
    public InteractionObjectUI interactionUI;
    public CrosshairUIController crosshairUI;


    private GameObject currentTarget;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    private Transform originalParent;
    private bool isInspecting = false;
    public bool IsInspecting => isInspecting;

    private bool isDragging = false;
    private Vector3 lastMousePos;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (!isInspecting || currentTarget == null) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            Vector3 newScale = currentTarget.transform.localScale + Vector3.one * scroll * zoomSpeed;
            float clampedScale = Mathf.Clamp(newScale.x, minScale, maxScale);
            currentTarget.transform.localScale = Vector3.one * clampedScale;
        }

        // 드래그 회전 시작
        if (Input.GetMouseButtonDown(0) && CanRotate(currentTarget))
        {
            isDragging = true;
            lastMousePos = Input.mousePosition;
        }

        // 드래그 중 회전
        if (isDragging && Input.GetMouseButton(0) && CanRotate(currentTarget))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;

            float rotX = delta.x * rotateSpeed * Time.deltaTime * 0.1f;
            float rotY = delta.y * rotateSpeed * Time.deltaTime * 0.1f;

            currentTarget.transform.Rotate(Vector3.up, -rotX, Space.World);
            currentTarget.transform.Rotate(Vector3.right, rotY, Space.Self);
        }

        // 드래그 종료
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // 조사 모드 종료
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            ExitInspectMode();
        }
    }


    public void EnterInspectMode(GameObject target)
    {
        if (isInspecting || target == null) return;
        
        currentTarget = target;
        isInspecting = true;

        originalPosition = target.transform.position;
        originalRotation = target.transform.rotation;
        originalScale = target.transform.localScale;
        originalParent = target.transform.parent;

        target.transform.SetParent(inspectPivot);
        target.transform.DOLocalMove(Vector3.zero, moveDuration).SetEase(Ease.OutQuad);
        target.transform.DOLocalRotate(Vector3.zero, moveDuration).SetEase(Ease.OutQuad);
        target.transform.DOScale(target.transform.localScale * scaleMultiply, moveDuration).SetEase(Ease.OutQuad);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        worldLabelUI.Hide();
        interactionUI.Hide();
        crosshairUI.SetState(false);
    }

    public void ExitInspectMode()
    {
        if (!isInspecting || currentTarget == null) return;

        isInspecting = false;

        currentTarget.transform.SetParent(originalParent);
        currentTarget.transform.DOMove(originalPosition, moveDuration).SetEase(Ease.InOutQuad);
        currentTarget.transform.DORotateQuaternion(originalRotation, moveDuration).SetEase(Ease.InOutQuad);
        currentTarget.transform.DOScale(originalScale, moveDuration).SetEase(Ease.InOutQuad);
        
        currentTarget = null;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    bool CanRotate(GameObject target)
    {
        var info = target.GetComponent<InteractableInfo>();
        if (info == null) return false;

        return info.interactableType == InteractableType.ActionObject
               || info.interactableType == InteractableType.InventoryObject;
    }
    
}
