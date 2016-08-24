using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Bidding_Game
{
    class Program
    {
        static int calculate_bid(int player, int pos, int[] first_moves, int[] second_moves)
        {

            int otherPlayerBidTotal = 0;
            int moneyLeft = 100;

            //Your logic to be put here
            if (player == 1)
            {
                for (int i = 0; i < first_moves.Length; i++)
                {
                    if (first_moves[i] == 0)
                        break;

                    moneyLeft -= first_moves[i];
                }

                for (int i = 0; i < second_moves.Length; i++)
                {
                    if (second_moves[i] == 0)
                        break;
                    otherPlayerBidTotal += second_moves[i];
                }

                if (pos == 9)
                    return Math.Min(otherPlayerBidTotal + 1, moneyLeft - 1);
            }
            else
            {
                for (int i = 0; i < second_moves.Length; i++)
                {
                    if (second_moves[i] == 0)
                        break;
                    moneyLeft -= second_moves[i];
                }

                for (int i = 0; i < first_moves.Length; i++)
                {
                    if (first_moves[i] == 0)
                        break;
                    otherPlayerBidTotal += first_moves[i];
                }
            }

            return Math.Min(moneyLeft, 10);
        }

        static void Main(String[] args)
        {
            int player = int.Parse(Console.ReadLine());
            int scotch_pos = int.Parse(Console.ReadLine());
            int bid = 0;                                 //Amount bid by the players
            String first_move = Console.ReadLine();     //Previous moves made by the first player
            String second_move = Console.ReadLine();     //Previous moves made by the second player
            String[] move_1 = first_move.Split(' ');
            String[] move_2 = second_move.Split(' ');
            int[] first_moves = new int[100];
            int[] second_moves = new int[100];
            if (String.Compare(first_move, "") != 0)
            {
                for (int i = 0; i < move_1.Length; i++)
                {
                    first_moves[i] = Convert.ToInt32(move_1[i]);
                    second_moves[i] = Convert.ToInt32(move_2[i]);
                }
            }
            bid = calculate_bid(player, scotch_pos, first_moves, second_moves);
            Console.Write(bid);
            Console.ReadLine();
        }
    }
}
