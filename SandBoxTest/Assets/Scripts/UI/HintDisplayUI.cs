using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HintDisplayUI : MonoBehaviour
{
    [SerializeField] private Text hintText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    
    private Coroutine fadeCoroutine;
    
    private void Awake()
    {
        if (hintText == null)
        {
            hintText = GetComponentInChildren<Text>();
        }
        
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }
        
        // 처음에는 숨김
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    
    // 힌트 텍스트 설정 및 표시
    public void ShowHint(string text)
    {
        if (hintText != null)
        {
            hintText.text = text;
        }
        
        // 현재 진행 중인 페이드 코루틴이 있다면 중지
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        
        fadeCoroutine = StartCoroutine(FadeIn());
    }
    
    // 힌트 숨기기
    public void HideHint()
    {
        // 현재 진행 중인 페이드 코루틴이 있다면 중지
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        
        fadeCoroutine = StartCoroutine(FadeOut());
    }
    
    // 페이드인 효과
    private IEnumerator FadeIn()
    {
        gameObject.SetActive(true);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        
        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;
        
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, elapsedTime / fadeInDuration);
            yield return null;
        }
        
        canvasGroup.alpha = 1f;
        fadeCoroutine = null;
    }
    
    // 페이드아웃 효과
    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;
        
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeOutDuration);
            yield return null;
        }
        
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false);
        fadeCoroutine = null;
    }
}