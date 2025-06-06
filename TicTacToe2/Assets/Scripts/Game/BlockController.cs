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
}
