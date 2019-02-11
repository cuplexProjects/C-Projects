using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Hip
{
    class Program
    {
        struct Point
        {
            public int row;
            public int col;
        }

        struct FileDate
        {
            public string player;
            public string[] board;
        }
        /* Head ends here */
        static void nextMove(String player, String[] board)
        {
            Random r = new Random();
            List<string> emptyPositions = new List<string>();
            List<Point> emptyPositionsAsPoints = new List<Point>();

            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[i].Length; j++)
                {
                    if (board[i][j] == (char)45)
                    {
                        emptyPositions.Add(i + " " + j);
                        emptyPositionsAsPoints.Add(new Point { row = i, col = j });
                    }
                }
            }

            //Randomize a move to evaluate        
            int index = 0;
            while (emptyPositions.Count() > 0)
            {
                index = r.Next(emptyPositions.Count() - 1);
                int row = emptyPositionsAsPoints[index].row;
                int col = emptyPositionsAsPoints[index].col;
                if (!FormsSquareInAnyDirection(ref board, row, col, player[0]))
                {
                    Console.WriteLine(emptyPositions[index]);
                    break;
                }
                else
                    emptyPositions.RemoveAt(index);

            }
            //if (emptyPositions.Count()>0)    	
            //Console.WriteLine(emptyPositions[r.Next(emptyPositions.Count()-1)]);
        }
        static bool FormsSquareInAnyDirection(ref String[] board, int row, int col, char player)
        {
            // Test all possible directions          
            int permutation = 1;
            int stepRow = 0;
            int stepCol = 0;

            do
            {
                for (int i = -8; i < 9; i++)
                {
                    if (stepRow + row + i > 8)
                        break;
                    if (stepCol + col + i > 8)
                        break;

                    if (col + i >= 0 && row + i >= 0)
                    {
                        if (board[row][col + i] == player && board[row + i][col + i] == player && board[row + i][col] == player)
                            return false;
                    }
                }

                if (permutation == 1)
                    stepRow++;
                else if (permutation == 2)
                    stepCol++;
                if (permutation == 3)
                {
                    stepRow++;
                    stepCol++;
                }

                if (stepCol > 8 || stepRow > 8)
                {
                    stepCol = 0;
                    stepRow = 0;
                    permutation++;
                }

            } while (permutation <= 3);

            return false;
        }
        /* Tail starts here */
        static void Main(String[] args)
        {
            String player;
            String[] board = new String[9];

            if (File.Exists("board.txt"))
            {
                FileDate fd = parseBoardFile();
                board = fd.board;
                player = fd.player;
            }
            else
            {
                player = Console.ReadLine();
                for (int i = 0; i < 9; i++)
                {
                    board[i] = Console.ReadLine();
                }
            }
            
            nextMove(player, board);
        }

        static FileDate parseBoardFile()
        {
            FileDate fd = new FileDate();
            fd.board = new String[9];
            FileStream fs = null;

            try
            {
                fs = File.OpenRead("board.txt");
                StreamReader sr = new StreamReader(fs);
                fd.player = sr.ReadLine();
                
                for (int i = 0; i < 9; i++)
                {
                    fd.board[i] = sr.ReadLine();
                }
                fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                Environment.Exit(0);
            }
            return fd;
        }
    }
}
