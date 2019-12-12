using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc2019
{
    public class D101 : ISolution
    {
        public string Answer
        {
            get
            {
                {
                    var lines = File.ReadAllLines("10.1.txt");
                    var asteroids = new List<Point>();
                    {
                        var y = 0;
                        foreach (var line in lines)
                        {
                            for (int x = 0; x < line.Length; x++)
                            {
                                if (line[x] == '#') asteroids.Add(new Point(x, y));
                            }
                            y++;
                        }
                    }

                    int maxCount = 0;
                    foreach (var a in asteroids)
                    {
                        var set = new HashSet<int>();
                        foreach (var aa in asteroids)
                        {
                            if (a == aa) continue;
                            var d = aa - a;
                            var g = gcd(Math.Abs(d.x), Math.Abs(d.y));
                            if (g != 0)
                            {
                                d.x /= g;
                                d.y /= g;
                            }
                            set.Add(d.y * 10000 + d.x);
                        }
                        if (set.Count > maxCount) maxCount = set.Count;
                    }

                    return maxCount.ToString();
                }
            }
        }

        static int gcd(int a, int b) => a == 0 ? b : gcd(b % a, a);

        public struct Point
        {
            public int x;
            public int y;
            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public static bool operator ==(Point @this, Point other)
            {
                return @this.x == other.x && @this.y == other.y;
            }
            public static bool operator !=(Point @this, Point other)
            {
                return @this.x != other.x || @this.y != other.y;
            }

            public static Point operator -(Point @this, Point other)
            {
                return new Point(@this.x - other.x, @this.y - other.y);
            }
            public static Point operator +(Point @this, Point other)
            {
                return new Point(@this.x + other.x, @this.y + other.y);
            }

            public (int x, int y) AsTuple() => (x, y);

            public override string ToString()
            {
                return $"({x}, {y})";
            }
        }
    }
}
