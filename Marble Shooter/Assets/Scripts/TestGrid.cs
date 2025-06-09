using System;
using UnityEngine;

public class TestGrid : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private int width = 100;
    [SerializeField] private int height = 100;
    [SerializeField] private float cellSize = 0.2f;
    
    public Material player1Material;
    public Material player2Material;
    
    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        Vector2 origin = new Vector2(-width / 2f * cellSize, -height / 2f * cellSize);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 pos = origin + new Vector2(x * cellSize, y * cellSize);
                GameObject cell = Instantiate(cellPrefab, pos, Quaternion.identity, transform);
                
                // 머터리얼 적용
                SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.material = (y < height / 2) ? player1Material : player2Material;
                }
                
                // 태그 및 레이어 설정
                if (y < height / 2)
                {
                    cell.tag = "Player1";    // 유니티 태그 관리자에서 사전 등록 필요
                    cell.layer = 6;          // 레이어 1 = Player1
                }
                else
                {
                    cell.tag = "Player2";    // 유니티 태그 관리자에서 사전 등록 필요
                    cell.layer = 7;          // 레이어 2 = Player2
                }
            }
        }
    }
}
