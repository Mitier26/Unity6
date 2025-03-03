using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    // 배경 스크롤링 구현
    public Transform BG1, BG2;
    public float scrollSpeed;

    private float bgWidth;

    void Start()
    {
        bgWidth = BG1.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
    }


    void Update()
    {
        BG1.position = new Vector3(BG1.position.x - scrollSpeed * Time.deltaTime, BG1.position.y, BG1.position.z);
        BG2.position -= new Vector3(scrollSpeed * Time.deltaTime, 0, 0);

        if(BG1.position.x <= -bgWidth -1)
        {
            BG1.position += new Vector3(bgWidth * 2, 0, 0);
        }

        if(BG2.position.x <= -bgWidth -1)
        {
            BG2.position += new Vector3(bgWidth * 2, 0, 0);
        }
    }
}
