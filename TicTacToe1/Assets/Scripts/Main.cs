using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class Main : MonoBehaviour
{
    public Board mBoard;
    public GameObject mWinner;

    private bool mXTurn = true;
    private int mTurnCount = 0;
    private void Awake()
    {
        mBoard.Build(this);
    }

    public void Switch()
    {
        mTurnCount++;

        bool hasWinner = mBoard.CheckForWinner();

        if (hasWinner || mTurnCount == 9)
        {
            StartCoroutine(EndGame(hasWinner));
            return;
        }
        
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

    private IEnumerator EndGame(bool hasWinner)
    {
        TextMeshProUGUI winnerLabel = mWinner.GetComponentInChildren<TextMeshProUGUI>();

        if (hasWinner)
        {
            winnerLabel.text = GetTurnCharacter() + " " + "Won!";
        }
        else
        {
            winnerLabel.text = "Draw!";
        }
        
        mWinner.SetActive(true);

        WaitForSeconds wait = new WaitForSeconds(5.0f);
        yield return wait;
        
        mBoard.Reset();
        mTurnCount = 0;
        
        mWinner.SetActive(false);
    }
}
