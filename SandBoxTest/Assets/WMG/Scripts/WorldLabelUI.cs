using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldLabelUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public CanvasGroup canvasGroup;

    public Transform target; // 따라갈 오브젝트
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    private Camera cam;

    void Start() {
        cam = Camera.main;
    }

    void Update() {
        if (target == null) {
            canvasGroup.alpha = 0;
            return;
        }

        canvasGroup.alpha = 1;
        Vector3 worldPos = target.position + offset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        transform.position = screenPos;
    }

    public void SetTarget(Transform newTarget, string nameStr, string descStr) {
        target = newTarget;
        nameText.text = nameStr;
        descriptionText.text = descStr;
    }

    public void ClearTarget() {
        target = null;
    }
}