using UnityEngine;
using DG.Tweening;

public class CellController : MonoBehaviour
{
    public Material player1Material;
    public Material player2Material;

    private SpriteRenderer sr;
    
    private Vector3 originalScale; // 0.2f로 저장될 예정
    private Tween hitTween;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    public void HitByBullet(int bulletLayer)
    {
        int myLayer = gameObject.layer;
        int player1 = LayerMask.NameToLayer("Player1");
        int player2 = LayerMask.NameToLayer("Player2");

        // 셀 팀 변경 조건
        if (bulletLayer == player1 && myLayer == player2)
        {
            ChangeCell(player1, "Player1", player1Material);
        }
        else if (bulletLayer == player2 && myLayer == player1)
        {
            ChangeCell(player2, "Player2", player2Material);
        }
    }

    private void ChangeCell(int newLayer, string newTag, Material newMat)
    {
        gameObject.layer = newLayer;
        tag = newTag;
        sr.material = newMat;

        PlayHitEffect();
    }

    private void PlayHitEffect()
    {
        if (hitTween != null && hitTween.IsActive())
            hitTween.Kill();

        transform.localScale = originalScale; // 항상 원래 크기로 복귀

        // 비율 기준으로 살짝 커졌다가 되돌아감
        hitTween = transform.DOScale(originalScale * 1.1f, 0.08f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                transform.DOScale(originalScale, 0.1f)
                    .SetEase(Ease.InCubic);
            });
    }
}