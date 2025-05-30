using System;
using UnityEngine;

public class Main : MonoBehaviour
{
    public Board mBoard;

    private bool mXTurn = true;
    private int mTurnCount = 0;
    private void Awake()
    {
        mBoard.Build(this);
    }

    public void Switch()
    {
        mTurnCount++;

        mXTurn = !mXTurn;
    }

    public string GetTurnCharacter()
    {
        if (mXTurn)
        {
            return "X";
        }
        else
        {
            return "O";
        }
    }
}
