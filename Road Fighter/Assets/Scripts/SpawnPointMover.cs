using UnityEngine;

public class SpawnPointMover : MonoBehaviour
{
    [Header("좌우 이동 단위")]
    public float xStep = 1.8f;

    [Header("이동 제한 (X 값 범위)")]
    public float minX = -5.4f;
    public float maxX = 5.4f;

    private float currentX = 0f;

    void Start()
    {
        currentX = transform.position.x;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) currentX -= xStep;
        if (Input.GetKeyDown(KeyCode.D)) currentX += xStep;

        currentX = Mathf.Clamp(currentX, minX, maxX);
        transform.position = new Vector3(currentX, transform.position.y, 0f);
    }
}