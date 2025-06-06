using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class PopupController : Singleton<PopupController>
{
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI confirmButtonText;

    [SerializeField] private RectTransform panelRectTransform;

    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        Hide(false);
    }

    public void Show(string content, string confirmButtonText, bool isAnimation, Action confirmAction)
    {
        gameObject.SetActive(true);

        _canvasGroup.alpha = 0;
        panelRectTransform.localScale = Vector3.zero;

        if (isAnimation)
        {
            panelRectTransform.DOScale(1f, 0.2f);
            _canvasGroup.DOFade(1f, 0.2f).SetEase(Ease.OutBack);
        }
        else
        {
            panelRectTransform.localScale = Vector3.one;
            _canvasGroup.alpha = 1;
        }

        contentText.text = content;
        this.confirmButtonText.text = confirmButtonText;
        
        confirmButton.onClick.AddListener(() =>
        {
            confirmAction();
            Hide(true);
        });
    }

    public void Hide(bool isAnimation)
    {
        if (isAnimation)
        {
            panelRectTransform.DOScale(0f, 0.2f).OnComplete(() =>
            {
                contentText.text = "";
                this.confirmButtonText.text = "";
                confirmButton.onClick.RemoveAllListeners();
                
                gameObject.SetActive(false);
            });
            _canvasGroup.DOFade(0f, 0.2f).SetEase(Ease.InBack);
        }
        else
        {
            contentText.text = "";
            this.confirmButtonText.text = "";
            confirmButton.onClick.RemoveAllListeners();
            
            gameObject.SetActive(false);
        }
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }
}
