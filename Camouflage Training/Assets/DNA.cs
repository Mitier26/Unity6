using UnityEngine;

public class DNA : MonoBehaviour
{
    // 유전 정보 (색상 및 크기)
    public float r;
    public float g;
    public float b;
    public float s;

    bool dead = false;  // 클릭되어 죽었는지 여부
    public float timeToDie = 0;  // 몇 초 살았는지 저장

    SpriteRenderer sRenderer;   // 색상을 적용할 SpriteRenderer
    Collider2D sCollider;       // 클릭 감지를 위한 Collider

    // 마우스로 클릭했을 때 호출
    void OnMouseDown()
    {
        dead = true;
        timeToDie = PopulationManager.elapsed;  // 현재까지 생존한 시간 저장
        sRenderer.enabled = false;              // 시각적으로 숨김
        sCollider.enabled = false;              // 더 이상 클릭되지 않게 비활성화
    }

    // 초기화
    void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        sCollider = GetComponent<Collider2D>();

        // 색상 적용
        sRenderer.color = new Color(r, g, b);
        // 크기 적용
        this.transform.localScale = new Vector3(s, s, s);
    }
}
