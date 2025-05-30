using System;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject mCellPrefab;

    private Cell[] mCells = new Cell[9];

    public void Build(Main main)
    {
        for (int i = 0; i <= 8; i++)
        {
            GameObject newCell = Instantiate(mCellPrefab, transform);

            mCells[i] = newCell.GetComponent<Cell>();
            mCells[i].mMain = main;
        }
    }
}
