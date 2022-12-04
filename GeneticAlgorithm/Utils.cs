using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public static class Utils
    {
        public static BitArray Append(this BitArray current, BitArray after)
        {
            var bools = new bool[current.Count + after.Count];
            current.CopyTo(bools, 0);
            after.CopyTo(bools, current.Count);
            return new BitArray(bools);
        }

        public static void Reverse(BitArray array)
        {
            int length = array.Length;
            int mid = (length / 2);

            for (int i = 0; i < mid; i++)
            {
                bool bit = array[i];
                array[i] = array[length - i - 1];
                array[length - i - 1] = bit;
            }
        }

        public static Tuple<BitArray, BitArray> Split(BitArray array, int index, int positionsCount)
        {
            BitArray array1 = new BitArray((positionsCount - 1) - index);
            BitArray array2 = new BitArray(index + 1);

            for (int i = 0; i < (positionsCount - 1) - index; ++i)
            {
                array1[i] = array[(positionsCount - 1) - i];
            }
            Reverse(array1);

            for (int i = 0; i < index + 1; ++i)
            {
                array2[i] = array[index - i];
            }
            Reverse(array2);

            return new Tuple<BitArray, BitArray>(array2, array1);
        }

        public static double GetRandomDoubleInRange(double min, double max)
        {
            Random random = new Random();
            return random.NextDouble() * (max - min) + min;
        }
    }
}
