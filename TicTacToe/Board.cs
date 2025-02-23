namespace TicTacToe;

public class Board
{
    private char[][] _board = new char[][]
    {
        [' ', ' ', ' '],
        [' ', ' ', ' '],
        [' ', ' ', ' '],
    };

    public void Set(int row, int col, char c)
    {
        _board[row][col] = c;
    }

    public char Get(int row, int col)
    {
        return _board[row][col];
    }
}