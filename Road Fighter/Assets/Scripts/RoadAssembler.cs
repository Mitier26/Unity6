using UnityEngine;

public class RoadAssembler : MonoBehaviour
{
    [Header("중앙 도로 SpriteRenderer (Draw Mode = Tiled)")]
    public SpriteRenderer centerRoad;

    [Header("좌/우 벽 Transform")]
    public Transform leftWall;
    public Transform rightWall;

    [Header("도로 기본 높이 (선택 사항)")]
    public float height = 1f;
    
    public SpriteRenderer backgroundRenderer;
    public Sprite[] backgroundSprites;

    /// <summary>
    /// 도로 전체의 너비를 설정하고 벽 위치를 자동 정렬함
    /// </summary>
    /// <param name="width">중앙 도로의 가로 길이</param>
    public void SetRoadWidth(float width)
    {
        // 중앙 도로 넓이 설정
        if (centerRoad != null)
        {
            float actualHeight = centerRoad.sprite.bounds.size.y;
            centerRoad.size = new Vector2(width, actualHeight);
        }

        // 좌우 벽 위치 조절
        float halfWidth = width / 2f;
        if (leftWall != null)
        {
            leftWall.localPosition = new Vector3(-halfWidth, 0f, 0f);
        }
        if (rightWall != null)
        {
            rightWall.localPosition = new Vector3(halfWidth, 0f, 0f);
        }
        
        SetRandomBackground();
    }
    
    public void SetRandomBackground()
    {
        if (backgroundRenderer == null || backgroundSprites.Length == 0)
            return;

        backgroundRenderer.sprite = backgroundSprites[Random.Range(0, backgroundSprites.Length)];
    }
}
