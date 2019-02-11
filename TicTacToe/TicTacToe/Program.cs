using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe
{
    public class Solution
    {
        /* Complete the function below to print 2 integers separated by a single space which will be your next move 
        */

        public static Random random = null;
        public const int MAX_RANDOM = 100;
        public static string MaxPlayer = null;
        public const int WinState = int.MaxValue - 10000;
        public const int LoseState = int.MinValue + 10000;

        static void nextMove(string player, string[] board)
        {
            int[] move = new int[2];

            MaxPlayer = player;
            if (board[0] == "___" && board[1] == "___" && board[2] == "___")
            {
                move[0] = 1;
                move[1] = 1;
            }
            else
                getNextMove(player, board, ref move, 0);
            Console.WriteLine(move[0].ToString() + " " + move[1].ToString());
        }
       
        static int getNextMove(string player, string[] board, ref int[] move, int depth)
        {
            string otherPlayer = (player == "X" ? "O" : "X");
            int gameState = getGameState(player, board);
            int bestState = LoseState;

            if (isComplete(board) || gameState == WinState || gameState == LoseState)
                return gameState;

            gameState = LoseState;           

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i][j] == '_')
                    {
                        board[i] = setBordRow(board[i], j, player);
                        gameState = Math.Max(gameState, -getNextMove(otherPlayer, board, ref move, depth + 1));
                        board[i] = setBordRow(board[i], j, "_");

                        if (gameState > bestState && depth == 0)
                        {
                            bestState = gameState;
                            move[0] = i;
                            move[1] = j;
                        }
                    }
                }
            }           
          
            return gameState;
        }

        private static string setBordRow(string row, int x, string player)
        {
            if (x == 0)
                return player + row.Substring(1);
            else if (x == 1)
                return row[0] + player + row.Substring(2);
            else return row.Substring(0, 2) + player;
        }

        private static bool isComplete(string[] board)
        {
            bool draw = true;
            for (int i = 0; i < 3; i++)
            {
                if (board[i].Contains('_'))
                {
                    draw = false;
                    break;
                }
            }
            return draw;
        }

        static int getGameState(string player, string[] board)
        {
            string winState1 = "XXX";
            string winState2 = "OOO";
            int heuristics = 0;

            for (int i = 0; i < 3; i++)
            {
                if (board[i] == winState1 || board[0][i].ToString() + board[1][i].ToString() + board[2][i].ToString() == winState1)
                    return player == "X" ? WinState : LoseState;
                else if (board[i] == winState2 || board[0][i].ToString() + board[1][i].ToString() + board[2][i].ToString() == winState2)
                    return player == "O" ? WinState : LoseState;

                if (board[i] == player + player + "_" || board[i] == "_" + player + player)
                    heuristics += MAX_RANDOM * 2;
            }

            if (board[0][0] != '_' && board[0][0] == board[1][1] && board[1][1] == board[2][2])
                return board[0][0].ToString() == player ? WinState : LoseState;

            if (board[0][2] != '_' && board[0][2] == board[1][1] && board[1][1] == board[2][0])
                return (board[0][0].ToString() == player) ? WinState : LoseState;

            //center square
            if (board[0] == "___" && board[1] == ("_" + player + "_") && board[2] == "___")
                heuristics = 100000;

            //draw 
            bool draw = true;
            for (int i = 0; i < 3; i++)
            {
                if (board[i].Contains('_'))
                {
                    draw = false;
                    break;
                }
            }

            if (draw)
                heuristics += MAX_RANDOM * 10000;

            return heuristics + random.Next(MAX_RANDOM);
        }

        static void Main(String[] args)
        {
            String player;

            random = new Random(Convert.ToInt32(DateTime.Now.Ticks%WinState));

            //If player is X, I'm the first player.
            //If player is O, I'm the second player.
            player = Console.ReadLine().ToUpper();

            //Read the board now. The board is a 3x3 array filled with X, O or _.
            String[] board = new String[3];
            for (int i = 0; i < 3; i++)
            {
                board[i] = Console.ReadLine().ToUpper();
            }

            nextMove(player, board);
            Console.ReadLine();
        }
    }
}