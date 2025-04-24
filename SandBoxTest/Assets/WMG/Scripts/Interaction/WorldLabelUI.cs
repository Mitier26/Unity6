using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    void LateUpdate() {
        if (target == null) {
            return;
        }
        
        Vector3 worldPos = target.position + offset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        transform.position = screenPos;
    }

    public void SetTarget(Transform newTarget, string nameStr, string descStr) {
        target = newTarget;
        nameText.text = nameStr;
        descriptionText.text = descStr;
        
        canvasGroup.DOFade(1f, 0.25f).SetEase(Ease.OutQuad);
    }

    public void ClearTarget() {
        target = null;
        canvasGroup.DOFade(0f, 0.25f).SetEase(Ease.OutQuad);
    }
    
    public void Hide()
    {
        canvasGroup.DOFade(0f, 0.25f).SetEase(Ease.OutQuad);
    }
}