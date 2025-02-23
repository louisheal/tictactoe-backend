namespace TicTacToe;

public class Game
{
    private int _state;
    private int _currentPlayer = 1;
    private readonly int[][] _board = new int[][]
    {
        [0,0,0],
        [0,0,0],
        [0,0,0],
    };
    private readonly int[][] _magicNums = new int[][]
    {
        [4, 9, 2],
        [3, 5, 7],
        [8, 1, 6],
    };

    public GameDto State()
    {
        return new GameDto(_state, _currentPlayer, _board);
    }

    public void MakeMove(Move move)
    {
        if (_state != 0)
        {
            return;
        }

        if (_board[move.row][move.col] != 0)
        {
            return;
        }

        if (move.player != _currentPlayer)
        {
            return;
        }
        
        _board[move.row][move.col] = _currentPlayer;
        _currentPlayer = _currentPlayer == 1 ? 2 : 1;
        _state = IsGameOver();
    }
    
    // 0 - in progress, 1 - player x, 2 - player o, 3 - draw
    private int IsGameOver()
    {
        // Check top left diagonal
        var sum = 0;
        for (var i = 0; i < 3; i++)
        {
            sum += _board[i][i] * _magicNums[i][i];
        }
        if (sum == 15)
        {
            return 1;
        }
        if (sum == 30)
        {
            return 2;
        }
        
        // Check top right diagonal
        sum = 0;
        for (var i = 0; i < 3; i++)
        {
            sum += _board[i][2-i] * _magicNums[i][2-i];
        }
        if (sum == 15)
        {
            return 1;
        }
        if (sum == 30)
        {
            return 2;
        }
        
        // Check rows
        for (var row = 0; row < 3; row++)
        {
            sum = 0;
            for (var col = 0; col < 3; col++)
            {
                sum += _board[row][col] * _magicNums[row][col];
            }
            if (sum == 15)
            {
                return 1;
            }
            if (sum == 30)
            {
                return 2;
            }
        }
        
        // Check columns
        for (var col = 0; col < 3; col++)
        {
            sum = 0;
            for (var row = 0; row < 3; row++)
            {
                sum += _board[row][col] * _magicNums[row][col];
            }
            if (sum == 15)
            {
                return 1;
            }
            if (sum == 30)
            {
                return 2;
            }
        }

        // Check if all tiles have been filled
        sum = 0;
        for (var row = 0; row < 3; row++)
        {
            for (var col = 0; col < 3; col++)
            {
                sum += _board[row][col];
            }
        }
        
        // If sum < 13, then game is still in progress
        return sum < 13 ? 0 : 3;
    }
}