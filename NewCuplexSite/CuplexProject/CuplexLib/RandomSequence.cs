using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace CuplexLib
{
    public class RandomSequence
    {
        private int[] sequence = null;
        private int index = -1;
        public bool SequenceLooped { get; private set; }

        private RandomSequence(int length)
        {
            sequence = new int[length];
            SequenceLooped = false;
        }
        public static RandomSequence GetRandomSequence(int length)
        {
            RandomSequence rs = new RandomSequence(length);
            RandomNumberGenerator rnd = RandomNumberGenerator.Create();
            rs.createRandomSequence(length, rnd);

            return rs;
        }
        public static RandomSequence GetRandomSequence(int length, RandomNumberGenerator rnd)
        {
            RandomSequence rs = new RandomSequence(length);
            rs.createRandomSequence(length, rnd);
            return rs;
        }

        private void createRandomSequence(int length, RandomNumberGenerator rnd)
        {
            byte[] buffer = new byte[length * 4];
            for (int i = 0; i < length; i++)
                sequence[i] = i;

            rnd.GetBytes(buffer);
            int rndPos = 0;

            for (int i = 0; i < length; i++)
            {
                int tmp = sequence[i];
                int iPos = Math.Abs(BitConverter.ToInt32(buffer, rndPos)) % length;
                sequence[i] = sequence[iPos];
                sequence[iPos] = tmp;

                rndPos += 4;
            }
        }
        public int GetNext()
        {
            if (index >= sequence.Length)
            {
                index = -1;
                SequenceLooped = true;
            }
            return sequence[++index];
        }
        public int GetPrevious()
        {
            if (index <= 0)
            {
                index = sequence.Length;
                SequenceLooped = true;
            }
            return sequence[--index];
        }
    }
}