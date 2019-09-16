using System;

namespace tictactoe
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("\n\n\nA new game has begun!");
                Game game = new Game();
                game.PlayGame();
            }


        }
    }
}
