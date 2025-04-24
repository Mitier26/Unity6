using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float detectDistance = 5f;
    public LayerMask interactableLayer;

    [Header("References")]
    public CrosshairUIController crosshairUIController;
    public WorldLabelUI worldLabelUI;

    private Camera cam;
    private IInteractable currentTarget;
    private HighlightController lastHighlight;
    private OutlineController lastOutline;
    
    [Header("UI")]
    public InteractionObjectUI interactionPromptUI;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        HandleDetection();
        HandleInteraction();
    }

    private void HandleDetection()
    {
        if (ObjectInspector.Instance != null && ObjectInspector.Instance.IsInspecting)
            return;
        
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit, detectDistance, interactableLayer))
        {
            GameObject hitObject = hit.collider.gameObject;
            GameObject rootObject = hit.collider.transform.root.gameObject;

            // 상호작용 대상
            currentTarget = hitObject.GetComponent<IInteractable>();
            crosshairUIController.SetState(currentTarget != null);

            InteractableInfo info = hitObject.GetComponent<InteractableInfo>();
            IInteractable interactable = hitObject.GetComponent<IInteractable>();

            // 1. 기본 라벨 (설명형)
            if (info != null && info.showLabel)
            {
                worldLabelUI.SetTarget(hitObject.transform, info.objectName, info.description);
            }
            else if (info != null && interactable != null && info.showInteractionMessage)
            {
                // 2. 상호작용 대상은 최소한 이름만 보여줌
                worldLabelUI.SetTarget(hitObject.transform, info.objectName, "");
            }
            else
            {
                worldLabelUI.ClearTarget();
            }

            // 3. 프롬프트 메시지
            if (info != null && interactable != null && info.showInteractionMessage && info.interactableType != InteractableType.DescriptionOnlyObject)
            {
                interactionPromptUI.Show("[" + Keyboard.current.eKey.displayName + "] 키를 눌러 상호작용");

            }
            else
            {
                interactionPromptUI.Hide();
            }


            // 하이라이트 처리
            HighlightController hc = rootObject.GetComponentInChildren<HighlightController>();
            if (lastHighlight != null && lastHighlight != hc)
                lastHighlight.DisableHighlight();

            if (hc != null)
            {
                hc.EnableHighlight();
                lastHighlight = hc;
            }

            // 아웃라인 처리
            OutlineController oc = rootObject.GetComponentInChildren<OutlineController>();
            OutlineInfo outlineInfo = hitObject.GetComponentInParent<OutlineInfo>();
            if (lastOutline != null && lastOutline != oc)
                lastOutline.DisableOutline();

            if (oc != null)
            {
                oc.EnableOutline(outlineInfo.GetOutlineColor());
                lastOutline = oc;
            }
        }
        else
        {
            // 조준 해제
            currentTarget = null;
            crosshairUIController.SetState(false);
            worldLabelUI.ClearTarget();
            interactionPromptUI.Hide();

            if (lastHighlight != null)
            {
                lastHighlight.DisableHighlight();
                lastHighlight = null;
            }

            if (lastOutline != null)
            {
                lastOutline.DisableOutline();
                lastOutline = null;
            }
        }
    }

    private void HandleInteraction()
    {
        if (currentTarget != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            var info = (currentTarget as MonoBehaviour)?.GetComponent<InteractableInfo>();

            if (info != null)
            {
                if (info.interactableType == InteractableType.DescriptionOnlyObject)
                    return;
            }

            currentTarget.Interact();
        }
    }
    
}
