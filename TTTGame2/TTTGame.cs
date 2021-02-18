/// <summary>
/// 
/// @author Ben Sabah.
/// 
/// TTTGame - this class make and handle an ascii Tic-Tac-Toe game.
///           Happy cow says: "Muuuuuuu.."
///           
/// </summary>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

public class TTTGame
{
    public const int BOARD_SIZE = 3;
    public enum Player
    {
        X, O, NONE
    }

    /**
	 * All the needed fields.
	 */
    private Player winner;
    private bool gameOver;
    private Point[] winIndices;
    private Player currentPlayer;
    private Player[,] gameTable;

    public TTTGame()
    {
        ResetGame();
    }

    public void ResetGame()
    {
        winIndices = null;
        gameTable = null;
        gameOver = false;
        winner = Player.NONE;
        currentPlayer = Player.X;
        gameTable = new Player[BOARD_SIZE, BOARD_SIZE];

        // Set the player's game table.
        for (int y = 0; y < BOARD_SIZE; y++)
        {
            for (int x = 0; x < BOARD_SIZE; x++)
            {
                SetPosition(x, y, Player.NONE);
            }
        }
    }

    /**
	 * try to place a piece at the given indexes.
	 * 
	 * @param xPos
	 * @param yPos
	 * @return true if placed player successfully, false if failed.
	 */
    // Check if the cell input is valid and return if able to place.
    public bool TryToPlacePiece(int x, int y)
    {
        if (IsSelectionValid(x, y))
        {
            SetPosition(x, y, currentPlayer);
            SwitchPlayer();
            return true;
        }
        return false;
    }

    public bool IsGameFinished()
    {
        // check if game already ended.
        if (gameOver || isAllOccupied())
        {
            winner = Player.NONE;
            gameOver = true;

            return true;
        }

        HashSet<Pattern> patterns = Pattern.GetDefaultPattern();
        HashSet<Point> winningIndicesSet = new HashSet<Point>();

        for (int y = 0; y < BOARD_SIZE; y++)
        {
            for (int x = 0; x < BOARD_SIZE; x++)
            {
                foreach (Pattern pattern in patterns)
                {
                    if (isPatternValid(pattern, x, y))
                    {
                        Pattern movedPattern = Pattern.addXYToCurrentPattern(pattern, x, y);
                        HashSet<Point> indices = movedPattern.GetPoints();

                        bool isXWon = areAllIndicesFullyOccupied(indices, Player.X);
                        bool isOWon = areAllIndicesFullyOccupied(indices, Player.O);

                        if (isXWon || isOWon)
                        {
                            winningIndicesSet.UnionWith(indices);
                        }
                    }
                }
            }
        }

        if (winningIndicesSet.Count != 0)
        {
            winIndices = new Point[winningIndicesSet.Count];
            winningIndicesSet.CopyTo(winIndices);
            winner = GetPosition(winIndices[0]);
            gameOver = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool isAllOccupied()
    {
        for (int y = 0; y < BOARD_SIZE; y++)
        {
            for (int x = 0; x < BOARD_SIZE; x++)
            {
                if (!IsOccupied(y, x))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool areAllIndicesFullyOccupied(HashSet<Point> points, Player player)
    {
        foreach (Point point in points)
        {
            if (gameTable[point.X, point.Y] != player)
            {
                return false;
            }
        }

        return true;
    }

    private bool isPatternValid(Pattern pattern, int x, int y)
    {
        HashSet<Point> points = pattern.GetPoints();
        foreach (Point point in points)
        {
            int calcX = x + point.X;
            int calcY = y + point.Y;

            if (calcX < 0 || calcY < 0 || calcX >= BOARD_SIZE || calcY >= BOARD_SIZE)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsOccupied(int x, int y)
    {
        return GetPosition(x, y) != Player.NONE;
    }

    private bool IsSamePlayer(int x1, int y1, int x2, int y2)
    {
        return GetPosition(x1, y1) == GetPosition(x2, y2);
    }

    // Return the player's enum if asked.
    public Player GetWinner()
    {
        return winner;
    }

    public Player GetCurrentPlayer()
    {
        return currentPlayer;
    }

    /**
     * return the current player in given index.
     * 
     * @param x
     * @param y
     * @return the player of the given indexes.
     */
    public Player GetPlayerInIndex(int x, int y)
    {
        // Check if valid indexes.
        if (x < 0 || x >= BOARD_SIZE || y < 0 || y >= BOARD_SIZE)
        {
            throw new ArgumentOutOfRangeException(string.Format("Index ({0},{1}), doesn't exist", x, y));
        }
        return GetPosition(x, y);
    }

    // Return a array of points that represent the winner's cells.
    public Point[] GetWinnerIndexes()
    {
        if (winIndices == null)
        {
            return null;
        }

        return (Point[])winIndices.Clone();
    }

    // Return a string that present the current game.
    public string GetCurrentTable()
    {
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < BOARD_SIZE; y++)
        {
            sb.Append('|');

            for (int x = 0; x < BOARD_SIZE; x++)
            {
                switch (GetPosition(x, y))
                {
                    case Player.X:
                        sb.Append('x');
                        break;
                    case Player.O:
                        sb.Append('o');
                        break;
                    case Player.NONE:
                    default:
                        sb.Append('-');
                        break;
                }
                sb.Append("|");
            }
            sb.Append("\n");
        }
        sb.Append("_______");
        return sb.ToString();
    }

    // Switch player.
    private void SwitchPlayer()
    {
        currentPlayer = (currentPlayer == Player.X) ? Player.O : Player.X;
    }

    private bool IsSelectionValid(int x, int y)
    {
        // Check if game over
        if (gameOver) return false;

        // Check if valid indexes.
        if (x < 0 || x >= BOARD_SIZE || y < 0 || y >= BOARD_SIZE) return false;

        // Check if empty cell.
        if (GetPosition(x, y) != Player.NONE) return false;

        return true;
    }

    private void SetPosition(int x, int y, Player player)
    {
        gameTable[x, y] = player;
    }

    private Player GetPosition(Point p)
    {
        return GetPosition(p.X, p.Y);
    }

    private Player GetPosition(int x, int y)
    {
        return gameTable[x, y];
    }
}
