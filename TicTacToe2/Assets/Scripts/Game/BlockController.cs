using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField] private Block[] blocks;

    public delegate void OnBlockClicked(int row, int col);

    public OnBlockClicked OnBlockClickedDelegate;

    public void InitBlocks()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].InitMarker(i, blockIndex =>
            {
                var clickedRow = blockIndex / 3;
                var clickedCol = blockIndex % 3;
                
                OnBlockClickedDelegate?.Invoke(clickedRow, clickedCol);
            });
        }
    }

    public void PlaceMarker(Block.MarkerType markerType, int row, int col)
    {
        var markerIndex = row * 3 + col;
        
        blocks[markerIndex].SetMarker(markerType);
    }
    
    public void SetBlockColor(GameManager.PlayerType playerType, (int row, int col)[] blockPositions)
    {
        if (playerType == GameManager.PlayerType.None) return;

        foreach (var blockPosition in blockPositions)
        {
            var blockIndex = blockPosition.row * 3 + blockPosition.col;
            Color markerColor;
            if (playerType == GameManager.PlayerType.PlayerA)
                markerColor = new Color32(0, 166, 255, 255);
            else if(playerType == GameManager.PlayerType.PlayerB)
                markerColor = new Color32(255, 0, 94, 255);
            else
                markerColor = Color.black;
            
            blocks[blockIndex].SetColor(markerColor);
            
        }
    }
}
