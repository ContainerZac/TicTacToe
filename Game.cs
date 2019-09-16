using System;
using System.Collections.Generic;


namespace tictactoe
{
    class Game
    {
        private Square[][] _board =
        {
            new Square[3],
            new Square[3],
            new Square[3]
        };

        //This method requires indexing of [x, y]
        //Multidimensional arrays must be even, not jagged

        //private Square[,] _board = new Square[3, 3]; 

        public void PlayGame()
        {
            Player player = Player.Crosses;

            //Removed bool to continue and instead included break statement
            while(true)
            {
                DisplayBoard();
                string victory = PlayMove(player);
                if (victory.Length > 1)
                {
                    DisplayBoard();
                    Console.WriteLine($"Game over! {victory} wins!");
                    break;
                }
                player = 3 - player;
            }
        }

        private void DisplayBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    Console.Write(" " + _board[i][j]);
                Console.WriteLine();
            }
        }

        private string PlayMove(Player player)
        {
            bool actioned = false;

            while (!actioned)
            {
                Console.WriteLine("Enter 'r' to restart");
                Console.Write($"{player}: Enter row comma column, eg. 3,3 > ");
                string input = Console.ReadLine();
                if (input == "r")
                {
                    return "Noone";
                }

                string[] parts = input.Split(',');

                if (parts.Length != 2)
                {
                    Console.WriteLine("Invalid input");
                    continue;
                }
                int.TryParse(parts[0], out int row);
                int.TryParse(parts[1], out int column);

                if (row < 1 || row > 3 || column < 1 || column > 3)
                {
                    Console.WriteLine("Invalid input");
                    continue;
                }
                    

                if (_board[row - 1][column - 1].Owner != Player.Noone)
                {
                    Console.WriteLine("Square is already occupied");
                    continue;
                }

                _board[row - 1][column - 1] = new Square(player);
                actioned = true;
            }

            return CheckWinner();

        }

        public string CheckWinner()
        {
            //Create a list of tuples for each team in order to store owned coordinates
            var crossList = new List<Tuple<int, int>>();
            var naughtList = new List<Tuple<int, int>>();

            //iterate through the board through nested loop, one for x coord, one for y
            for (int x = 0; x < _board.Length; x++)
            {

                for (int  y = 0; y < _board.Length; y++)
                {
                    //add crosses to crosslist
                    if (_board[x][y].ToString() == "X")
                        crossList.Add(Tuple.Create(x, y));
                    //add noughts to noughtlist
                    else if (_board[x][y].ToString() == "0")
                        naughtList.Add(Tuple.Create(x, y));
                }

                
            }

            //Check if either team won
            if (CheckGradients(crossList))
                return "Crosses";
            else if (CheckGradients(naughtList))
                return "Noughts";
            //check if noone won
            else if (CheckFilled())
                return "Noone";
            //return empty string if no winner
            return "";
        }

        public bool CheckFilled()
        {


            for (int x = 0; x < _board.Length; x++)
            {

                for (int y = 0; y < _board.Length; y++)
                {
                    //add crosses to crosslist
                    if (_board[x][y].ToString() == ".")
                        return false;

                }
            }

            return true;
        }

        public bool CheckGradients(List<Tuple<int, int>> tuples)
        {
            //iterate through tuples twice nested to compare to other tuples in list
            foreach (var tuple1 in tuples)
            {
                //Console.WriteLine($"The coordinate is {tuple.Item1}, {tuple.Item2}");
                foreach (var tuple2 in tuples)
                {
                    //Don't waste time testing same Tuple else will wrongly detect win
                    if (tuple1 == tuple2)
                        continue;

                    //All wins in tic tac toe require at least one coordinate to be 0, no point starting 
                    //from other tuples, simplifies gradient tests for later
                    if (tuple1.Item1 != 0 && tuple1.Item2 != 0)
                        continue;

                    //get the delta of the x and y coords between two tuples
                    int xdif = tuple1.Item1 - tuple2.Item1;
                    int ydif = tuple1.Item2 - tuple2.Item2;

                    //Check if the next coordinate pair in the sequence is in the list of tuples
                    var nextTuple = Tuple.Create(tuple2.Item1 - xdif, tuple2.Item2 - ydif);

                    if (tuples.Contains(nextTuple))
                        return true;


                }
            }

            return false;
        }

    }
}
