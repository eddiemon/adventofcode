using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace aoc
{
    public class D24
    {
        //Each minute, The bugs live and die based on the number of bugs in the four adjacent tiles:
        // A bug dies (becoming an empty space) unless there is exactly one bug adjacent to it.
        // An empty space becomes infested with a bug if exactly one or two bugs are adjacent to it.
        public object Answer()
        {
            var input = File.ReadAllLines("24.in");
            var Y = input.Length;
            var X = input[0].Length;
            var G = input.SelectMany(s => s.Select(c => c == '#')).ToArray();
            var DY = new int[] { 0, 0, -1, 1 };
            var DX = new int[] { 1, -1, 0, 0 };

            PrintGame(G, X, Y);

            var gs = new List<bool[]>();
            gs.Add(G);
            while (true)
            {
                var GG = new bool[Y * X];
                for (int y = 0; y < Y; y++)
                {
                    for (int x = 0; x < X; x++)
                    {
                        var count = 0;
                        for (int d = 0; d < 4; d++)
                        {
                            var yy = y + DY[d];
                            var xx = x + DX[d];
                            if (yy < 0 || yy >= Y) continue;
                            if (xx < 0 || xx >= X) continue;

                            if (G[(yy) * X + (xx)]) ++count;
                        }

                        if (G[y * X + x])
                        {
                            if (count == 1) GG[y * X + x] = true;
                            else GG[y * X + x] = false;
                        }
                        else
                        {
                            if (count == 1 || count == 2) GG[y * X + x] = true;
                        }
                    }
                }
                if (gs.Any(g => Equals(GG, g)))
                {
                    var totalRating = 0L;
                    var rating = 1L;
                    for (int y = 0; y < Y; y++)
                    {
                        for (int x = 0; x < X; x++)
                        {
                            if (GG[y * X + x]) totalRating += rating;
                            rating <<= 1;
                        }
                    }
                    return totalRating;
                }
                gs.Add(GG);
                G = GG;
            }

            return "error";
        }

        public static bool Equals(bool[] g, bool[] gg)
        {
            for (int i = 0; i < g.Length; i++)
            {
                if (g[i] != gg[i]) return false;
            }
            return true;
        }

        public static void PrintGame(bool[] g, int X, int Y)
        {
            for (int y = 0; y < Y; y++)
            {
                for (int x = 0; x < X; x++)
                {
                    if (g[y * X + x]) Console.Write('#');
                    else Console.Write('.');
                }
                Console.WriteLine();
            }
        }
    }
}
