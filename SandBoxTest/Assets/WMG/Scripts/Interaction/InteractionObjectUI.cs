using TMPro;
using UnityEngine;
using DG.Tweening;

public class InteractionObjectUI : MonoBehaviour
{
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI interactionText;

    private bool isVisible = false;
    private Tween fadeTween;
    
    void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = interactionPanel.GetComponent<CanvasGroup>();
    }


    public void Show(string message)
    {
        interactionText.text = message;

        // ✅ 현재 트윈이 진행 중이면 강제 종료
        if (fadeTween != null && fadeTween.IsActive())
            fadeTween.Kill();

        interactionPanel.SetActive(true);
        fadeTween = canvasGroup.DOFade(1f, 0.25f).SetEase(Ease.OutQuad);
        isVisible = true;
    }

    public void Hide()
    {
        if (!isVisible) return;

        // ✅ 트윈 정리
        if (fadeTween != null && fadeTween.IsActive())
            fadeTween.Kill();

        fadeTween = canvasGroup.DOFade(0f, 0.25f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            interactionPanel.SetActive(false);
            isVisible = false;
        });
    }

}