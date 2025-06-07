using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private BlockController blockController;

    [SerializeField] private PanelManager panelManager;
    [SerializeField] private GameUIController gameUIController;
    
    public enum PlayerType {None, PlayerA, PlayerB}

    private PlayerType[,] _board;

    private enum TurnType
    {
        PlayerA,
        PlayerB
    }
    private enum GameResult {None, Win, Lose, Draw}
    
    private void Start()
    {
        InitGame();
    }

    public void InitGame()
    {
        _board = new PlayerType[3, 3];
        
        blockController.InitBlocks();
        
        panelManager.ShowPanel(PanelManager.PanelType.StartPanel);
        
        gameUIController.SetGameUIMode(GameUIController.GameUIMode.Init);
    }

    public void StartGame()
    {
        SetTurn(TurnType.PlayerA);
    }

    private void EndGame(GameResult gameResult)
    {
        gameUIController.SetGameUIMode(GameUIController.GameUIMode.GameOver);
        
        switch (gameResult)
        {
            case GameResult.Win:
                Debug.Log("You win!");
                break;
            case GameResult.Lose:
                Debug.Log("You lose!");
                break;
            case GameResult.Draw:
                Debug.Log("Draw!");
                break;
        }
    }

    private bool SetNewBoardValue(PlayerType playerType, int row, int col)
    {
        if (playerType == PlayerType.PlayerA && _board[row, col] == PlayerType.None)
        {
            _board[row, col] = playerType;
            blockController.PlaceMarker(Block.MarkerType.O, row, col);
            return true;
        }
        else if (playerType == PlayerType.PlayerB && _board[row, col] == PlayerType.None) 
        {
            _board[row, col] = playerType;
            blockController.PlaceMarker(Block.MarkerType.X, row, col);
            return true;
        }

        return false;
    }

    private void SetTurn(TurnType turnType)
    {
        switch (turnType)
        {
            case TurnType.PlayerA:
                gameUIController.SetGameUIMode(GameUIController.GameUIMode.TurnA);
                
            blockController.OnBlockClickedDelegate = (row, col) =>
            {
                if (SetNewBoardValue(PlayerType.PlayerA, row, col))
                {
                    var gameResult = CheckGameResult();
                    if(gameResult == GameResult.None)
                        SetTurn(TurnType.PlayerB);
                    else
                        EndGame(gameResult);
                }
                else
                {
                    // TODO: 이미 있는 곳을 터치했을 때 처리
                    Debug.Log("이미 있는 곳");
                }
            };
                
                break;
            case TurnType.PlayerB:
                gameUIController.SetGameUIMode(GameUIController.GameUIMode.TurnB);
                blockController.OnBlockClickedDelegate = (row, col) =>
                {
                    if (SetNewBoardValue(PlayerType.PlayerB, row, col))
                    {
                        var gameResult = CheckGameResult();
                        if(gameResult == GameResult.None)
                            SetTurn(TurnType.PlayerA);
                        else
                            EndGame(gameResult);
                    }
                    else
                    {
                        // TODO: 이미 있는 곳을 터치했을 때 처리
                        Debug.Log("이미 있는 곳");
                    }
                };
                break;
        }

        switch (CheckGameResult())
        {
            case GameResult.Win:
                break;
            case GameResult.Lose:
                break;
            case GameResult.Draw:
                break;
            case GameResult.None:
                var nextTurn = turnType == TurnType.PlayerA ? TurnType.PlayerB : TurnType.PlayerA;
                SetTurn(nextTurn);
                break;
        }
    }

    private GameResult CheckGameResult()
    {
        if (CheckGameWin(PlayerType.PlayerA)) { return GameResult.Win; }
        if (CheckGameWin(PlayerType.PlayerB)) { return GameResult.Lose; }
        if (IsAllBlocksPlaced()) { return GameResult.Draw; }

        return GameResult.None;
    }

    private bool IsAllBlocksPlaced()
    {
        for (var row = 0; row < _board.GetLength(0); row++)
        {
            for (var col = 0; col < _board.GetLength(1); col++)
            {
                if (_board[row, col] == PlayerType.None)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool CheckGameWin(PlayerType playerType)
    {
        // 가로로 마커가 일치 하는지 확인
        for(var row = 0; row <_board.GetLength(0); row++)
        {
            if(_board[row, 0] == playerType && _board[row,1] == playerType && _board[row,2] == playerType)
            {
                (int, int)[] blocks = { (row, 0), (row, 1), (row, 2) };
                blockController.SetBlockColor(playerType, blocks);
                return true;
            }
        }
        
        // 세로로 마커가 일치하는지 확인
        for (var col = 0; col < _board.GetLength(1); col++)
        {
            if (_board[0, col] == playerType && _board[1, col] == playerType && _board[2, col] == playerType)
            {
                (int, int)[] blocks = { (0, col), (1, col), (2, col) };
                blockController.SetBlockColor(playerType, blocks);
                return true;
            }
        }
        
        // 대각선 마커가 일치하는지 확인
        if (_board[0, 0] == playerType && _board[1, 1] == playerType && _board[2, 2] == playerType)
        {
            (int, int)[] blocks = { (0, 0), (1, 1), (2, 2) };
            blockController.SetBlockColor(playerType, blocks);
            return true;
        }

        if (_board[0, 2] == playerType && _board[1, 1] == playerType && _board[2, 0] == playerType)
        {
            
            (int, int)[] blocks = { (0, 2), (1, 1), (2, 0) };
            blockController.SetBlockColor(playerType, blocks);
            return true;
        }

        return false;
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }
}
