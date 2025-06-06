using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private BlockController blockController;
    public enum PlayerType {None, PlayerA, PlayerB}

    private PlayerType[,] _board;

    private void Start()
    {
        InitGame();

        blockController.OnBlockClickedDelegate = (row, col) =>
        {
            Debug.Log("Row : " + row + ", Col : " + col);
        };
    }

    public void InitGame()
    {
        _board = new PlayerType[3, 3];
        
        blockController.InitBlocks();
    }


    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }
}
