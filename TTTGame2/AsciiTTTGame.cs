using System;
using static TTTGame;

namespace TTTGame2
{
    public class AsciiTTTGame
    {
        public static void Main2(string[] args)
        {
            // Setting up the game.
            TTTGame curGame = new TTTGame();
            Console.WriteLine("This is a new TTT game");

            while (!curGame.IsGameFinished())
            {
                // Ask for index to place player sign.
                Console.WriteLine("its " + curGame.GetCurrentPlayer() + " turn, please type position: ");
                int.TryParse(Console.ReadLine(), out int x);
                int.TryParse(Console.ReadLine(), out int y);

                // try to place the sign and respond if not available.
                if (!curGame.TryToPlacePiece(x, y))
                {
                    Console.WriteLine("that position is marked / unavailable, please try again.");
                }

                // Print the current game table.
                Console.WriteLine(curGame.GetCurrentTable());
            }

            // Output according to result
            Console.WriteLine(GetWinnerString(curGame.GetWinner()));
        }


        private static string GetWinnerString(Player winner)
        {
            return (winner == Player.NONE) ? "תיקו !" : string.Format("המנצח הוא {0}!", winner);
        }
    }
}
