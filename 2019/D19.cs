using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace aoc
{
    internal class D19
    {
        BigInteger val(int x, int y)
        {
            var computer = new IntCodeSync(code.ToArray());
            computer.Run(x);
            computer.Run(y);
            return computer.Run().Value;
        }

        //static char[,] g;
        //static D19()
        //{
        //    var lines = File.ReadAllLines("19.1");
        //    g = new char[lines.Length, lines[0].Length];
        //    for (int r = 0; r < lines.Length; r++)
        //    {
        //        for (int c = 0; c < lines[0].Length; c++)
        //        {
        //            g[r, c] = lines[r][c];
        //        }
        //    }
        //}

        //BigInteger val(int x, int y)
        //{
        //    if (x < 0 || x >= g.GetLength(1) || y < 0 || y >= g.GetLength(0)) return 0;

        //    return g[y, x] == '#' ? 1 : 0;
        //}

        BigInteger[] code;

        internal object Answer()
        {
            code = File.ReadAllText("19.in").Split(',').Select(s => BigInteger.Parse(s)).ToArray();

            const int size = 100;
            int x = 0;
            for (int y = 1000; ; y++)
            {
                for (;; x++)
                {
                    if (val(x, y) == 1)
                    {
                        if (val(x + size - 1, y) == 1 && val(x + size - 1, y - size + 1) == 1)
                        {
                            Console.WriteLine($"({x}, {y - size + 1})");
                            Console.WriteLine($"({x + size - 1}, {y - size + 1})");
                            Console.WriteLine($"({x}, {y})");
                            return x * 10000 + (y - size + 1);
                        }
                        else
                        {
                            x -= 5;
                            break;
                        }
                    }
                }
            }
        }
    }
}