using UnityEngine;
using DG.Tweening;

public class ObjectAnimationController : MonoBehaviour
{
    // 트위닝 지속 시간
    public float duration = 1.0f;
    
    // 애니메이션 타입 선택용 열거형
    public enum AnimationType
    {
        Scale,
        Move,
        Rotate,
        Fade,
        Color
    }
    
    // 인스펙터에서 선택할 애니메이션 타입
    public AnimationType animationType;
    
    // 애니메이션 값들
    public Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f);
    public Vector3 targetPosition = new Vector3(0, 2, 0);
    public Vector3 targetRotation = new Vector3(0, 360, 0);
    public float targetAlpha = 1.0f;
    public Color targetColor = Color.red;
    
    // 원래 위치, 회전, 스케일 저장 변수
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    
    // 원래 위치로 돌아갈지 설정
    public bool returnToOriginal = true;
    
    // 이징 타입
    public Ease easeType = Ease.OutBack;
    public Ease returnEaseType = Ease.InBack;
    
    // 컴포넌트 참조
    private CanvasGroup canvasGroup;
    private Renderer objectRenderer;
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        // 필요한 컴포넌트 캐싱
        canvasGroup = GetComponent<CanvasGroup>();
        objectRenderer = GetComponent<Renderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // 초기 위치, 회전, 스케일 저장
        SaveOriginalTransform();
    }
    
    private void SaveOriginalTransform()
    {
        // 초기 상태 저장
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        originalScale = transform.localScale;
    }
    
    private void OnEnable()
    {
        // 오브젝트가 활성화될 때 원래 위치로 리셋 후 애니메이션 실행
        ResetToOriginalTransform();
        PlayAnimation();
    }
    
    private void ResetToOriginalTransform()
    {
        // 원래 위치, 회전, 스케일로 리셋
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
        transform.localScale = originalScale;
    }
    
    public void PlayAnimation()
    {
        // 기존 트윈 취소
        DOTween.Kill(this.gameObject);
        
        // 선택된 애니메이션 타입에 따라 실행
        switch (animationType)
        {
            case AnimationType.Scale:
                // 초기 스케일 설정 (필요시)
                transform.localScale = Vector3.zero;
                // 스케일 애니메이션
                var scaleTween = transform.DOScale(targetScale, duration)
                    .SetEase(easeType)
                    .SetId(this.gameObject);
                
                // 원래 스케일로 돌아가기 (설정된 경우)
                if (returnToOriginal)
                {
                    scaleTween.OnComplete(() => {
                        transform.DOScale(originalScale, duration)
                            .SetEase(returnEaseType)
                            .SetId(this.gameObject);
                    });
                }
                break;
                
            case AnimationType.Move:
                // 이동 애니메이션
                var moveTween = transform.DOLocalMove(targetPosition, duration)
                    .SetEase(easeType)
                    .SetId(this.gameObject);
                
                // 원래 위치로 돌아가기 (설정된 경우)
                if (returnToOriginal)
                {
                    moveTween.OnComplete(() => {
                        transform.DOLocalMove(originalPosition, duration)
                            .SetEase(returnEaseType)
                            .SetId(this.gameObject);
                    });
                }
                break;
                
            case AnimationType.Rotate:
                // 회전 애니메이션
                var rotateTween = transform.DORotate(targetRotation, duration, RotateMode.FastBeyond360)
                    .SetEase(easeType)
                    .SetId(this.gameObject);
                
                // 원래 회전으로 돌아가기 (설정된 경우)
                if (returnToOriginal)
                {
                    rotateTween.OnComplete(() => {
                        transform.DORotate(originalRotation.eulerAngles, duration)
                            .SetEase(returnEaseType)
                            .SetId(this.gameObject);
                    });
                }
                break;
                
            case AnimationType.Fade:
                // 페이드 인 애니메이션 
                if (canvasGroup != null)
                {
                    // UI 요소 페이드
                    canvasGroup.alpha = 0;
                    var fadeTween = canvasGroup.DOFade(targetAlpha, duration)
                        .SetEase(easeType)
                        .SetId(this.gameObject);
                    
                    if (returnToOriginal)
                    {
                        fadeTween.OnComplete(() => {
                            canvasGroup.DOFade(0, duration)
                                .SetEase(returnEaseType)
                                .SetId(this.gameObject);
                        });
                    }
                }
                else if (spriteRenderer != null)
                {
                    // 스프라이트 페이드
                    Color startColor = spriteRenderer.color;
                    float originalAlpha = startColor.a;
                    startColor.a = 0;
                    spriteRenderer.color = startColor;
                    
                    var fadeTween = spriteRenderer.DOFade(targetAlpha, duration)
                        .SetEase(easeType)
                        .SetId(this.gameObject);
                    
                    if (returnToOriginal)
                    {
                        fadeTween.OnComplete(() => {
                            spriteRenderer.DOFade(originalAlpha, duration)
                                .SetEase(returnEaseType)
                                .SetId(this.gameObject);
                        });
                    }
                }
                else if (objectRenderer != null)
                {
                    // 3D 오브젝트 머티리얼 페이드
                    Material mat = objectRenderer.material;
                    Color startColor = mat.color;
                    float originalAlpha = startColor.a;
                    startColor.a = 0;
                    mat.color = startColor;
                    
                    var fadeTween = mat.DOFade(targetAlpha, duration)
                        .SetEase(easeType)
                        .SetId(this.gameObject);
                    
                    if (returnToOriginal)
                    {
                        fadeTween.OnComplete(() => {
                            mat.DOFade(originalAlpha, duration)
                                .SetEase(returnEaseType)
                                .SetId(this.gameObject);
                        });
                    }
                }
                else
                {
                    Debug.LogWarning("Fade animation requires a CanvasGroup, SpriteRenderer, or Renderer component");
                }
                break;
                
            case AnimationType.Color:
                // 색상 변경 애니메이션
                if (objectRenderer != null)
                {
                    Color originalColor = objectRenderer.material.color;
                    var colorTween = objectRenderer.material.DOColor(targetColor, duration)
                        .SetEase(easeType)
                        .SetId(this.gameObject);
                    
                    if (returnToOriginal)
                    {
                        colorTween.OnComplete(() => {
                            objectRenderer.material.DOColor(originalColor, duration)
                                .SetEase(returnEaseType)
                                .SetId(this.gameObject);
                        });
                    }
                }
                else if (spriteRenderer != null)
                {
                    Color originalColor = spriteRenderer.color;
                    float alpha = originalColor.a; // 알파값 보존
                    var colorTween = spriteRenderer.DOColor(targetColor, duration)
                        .SetEase(easeType)
                        .SetId(this.gameObject);
                    
                    if (returnToOriginal)
                    {
                        colorTween.OnComplete(() => {
                            spriteRenderer.DOColor(originalColor, duration)
                                .SetEase(returnEaseType)
                                .SetId(this.gameObject);
                        });
                    }
                }
                else
                {
                    Debug.LogWarning("Color animation requires a Renderer component");
                }
                break;
        }
    }
    
    private void OnDisable()
    {
        // 오브젝트가 비활성화될 때 트윈 취소
        DOTween.Kill(this.gameObject);
    }
}