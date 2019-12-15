using System;
using System.Collections.Generic;
using System.IO;


namespace aoc
{
    public class D101 : ISolution
    {
        public string Answer
        {
            get
            {
                {
                    var lines = File.ReadAllLines("10.1.txt");
                    var asteroids = new List<Vector2>();
                    {
                        var y = 0;
                        foreach (var line in lines)
                        {
                            for (int x = 0; x < line.Length; x++)
                            {
                                if (line[x] == '#') asteroids.Add(new Vector2(x, y));
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
                            var g = Maths.gcd(Math.Abs(d.x), Math.Abs(d.y));
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
    }
}
