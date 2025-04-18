using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float detectDistance = 5f;
    public LayerMask interactableLayer;

    [Header("References")]
    public CrosshairUIController uiController;
    public WorldLabelUI labelUI;

    private Camera cam;
    private IInteractable currentTarget;
    private HighlightController lastHighlight;
    private OutlineController lastOutline;

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
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit, detectDistance, interactableLayer))
        {
            GameObject hitObject = hit.collider.gameObject;
            GameObject rootObject = hit.collider.transform.root.gameObject;

            // 🎯 상호작용 대상
            currentTarget = hitObject.GetComponent<IInteractable>();
            uiController.SetState(currentTarget != null);

            // ℹ️ 정보 UI
            InteractableInfo info = hitObject.GetComponent<InteractableInfo>();
            if (info != null)
                labelUI.SetTarget(hitObject.transform, info.objectName, info.description);
            else
                labelUI.ClearTarget();

            // ✨ 하이라이트 처리
            HighlightController hc = rootObject.GetComponentInChildren<HighlightController>();
            if (lastHighlight != null && lastHighlight != hc)
                lastHighlight.DisableHighlight();

            if (hc != null)
            {
                hc.EnableHighlight();
                lastHighlight = hc;
            }

            // 🟡 아웃라인 처리
            OutlineController oc = rootObject.GetComponentInChildren<OutlineController>();
            if (lastOutline != null && lastOutline != oc)
                lastOutline.DisableOutline();

            if (oc != null)
            {
                oc.EnableOutline();
                lastOutline = oc;
            }
        }
        else
        {
            // 🔻 조준 해제
            currentTarget = null;
            uiController.SetState(false);
            labelUI.ClearTarget();

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
            currentTarget.Interact();
        }
    }
}
