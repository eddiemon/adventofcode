using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace aoc
{
    internal class D81
    {
        internal string Answer()
        {
            var input = File.ReadAllText("d8.txt").AsSpan();
            var layers = new List<Layer>();
            int nextReadPos = 0;
            while (nextReadPos < input.Length)
            {
                var s = input.Slice(nextReadPos, width * height);
                layers.Add(Layer.FromString(s));
                nextReadPos += width * height;
            }
            int leastZeroes = int.MaxValue;
            int layerWithLeastZeroes = -1;
            int layer = 0;
            foreach (var l in layers)
            {
                int zeroes = l.CountNumberOfDigits(0);
                if (zeroes < leastZeroes)
                {
                    leastZeroes = zeroes;
                    layerWithLeastZeroes = layer;
                }
                layer++;
            }
            Debug.Assert(layer != 0);
            Debug.Assert(layerWithLeastZeroes != -1);
            return (layers[layerWithLeastZeroes].CountNumberOfDigits(1) *
            layers[layerWithLeastZeroes].CountNumberOfDigits(2)).ToString();
        }
        const int width = 25;
        const int height = 6;
        internal class Layer
        {
            public int[] data;
            public static Layer FromString(ReadOnlySpan<char> s)
            {
                Debug.Assert(s.Length == width * height);
                var l = new Layer();
                l.data = new int[width * height];
                for (int i = 0; i < s.Length; i++)
                {
                    l.data[i] = s[i] - '0';
                }
                return l;
            }
            public int GetData(int x, int y)
            {
                Debug.Assert(y < height);
                Debug.Assert(x < width);
                return data[y * width + x];
            }

            public int CountNumberOfDigits(int digit)
            {
                int count = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == digit) count++;
                }
                return count;
            }
        }
    }
}