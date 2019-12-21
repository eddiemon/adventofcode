using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace aoc
{
    public class D17
    {
        const int North = 1;
        const int South = 2;
        const int West = 3;
        const int East = 4;

        private Dictionary<int, Vector2> HeadingToVector = new Dictionary<int, Vector2>
        {
            { North, (0, -1) },
            { South, (0, 1) },
            { West, (-1, 0) },
            { East, (1, 0) },
        };

        public object Answer()
        {
            var code = File.ReadAllText("17.in").Split(',').Select(s => BigInteger.Parse(s)).ToArray();
            var computer = new IntCodeSync(code);

            var scaffolds = new HashSet<Vector2>();
            int x = 0, y = 0;
            try
            {
                var o = computer.Run().Value;
                while (true)
                {
                    Console.Write((char)o);
                    //if (o == (int)'^' || o == (int)'<' || o == (int)'>' || o == (int)'v')
                    //{

                    //}

                    if (o == (int)'#' || o == (int)'^' || o == (int)'<' || o == (int)'>' || o == (int)'v')
                    {
                        scaffolds.Add((x++, y));
                    }
                    else if (o == 10) // newline
                    {
                        ++y; x = 0;
                    }
                    else
                        ++x;

                    o = computer.Run().Value;
                }
            } catch (ComputerHaltedException)
            {

            }
            x = 0; y = 0;
            var intersections = new List<Vector2>();
            foreach (var p in scaffolds)
            {
                var isIntersect = HeadingToVector.Values.Select(d => p + d).All(pd => scaffolds.Contains(pd));
                if (isIntersect)
                {
                    intersections.Add(p);
                    Console.SetCursorPosition(p.x, p.y);
                    Console.Write('O');
                }
            }
            Console.SetCursorPosition(0, 100);

            return intersections.Select(p => p.x * p.y).Sum();
        }
    }
}
