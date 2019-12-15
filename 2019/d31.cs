using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace aoc
{
    public class D31
    {
        public string Answer
        {
            get
            {
                var directions = File.ReadAllLines("d3.txt");
                var w1Dirs = directions[0].Split(',');
                var w2Dirs = directions[1].Split(',');

                var w1 = CreateSegmentsFromDirections(w1Dirs);
                var w2 = CreateSegmentsFromDirections(w2Dirs);

                int minDistance = int.MaxValue;
                foreach (var seg1 in w1)
                {
                    foreach (var seg2 in w2)
                    {
                        var intersection = seg1.IntersectingPoint(seg2);
                        if (intersection != null)
                        {
                            var intersectionDistance = Math.Abs(intersection.Value.X) + Math.Abs(intersection.Value.Y);
                            if (intersectionDistance < minDistance)
                                minDistance = intersectionDistance;
                        }
                    }
                }

                return minDistance.ToString();
            }
        }

        private List<Segment> CreateSegmentsFromDirections(string[] directions)
        {
            var segments = new List<Segment>();
            int x = 0, y = 0;
            foreach (var dir in directions)
            {
                int newX = x, newY = y;
                if (dir.StartsWith('R'))
                    newX += int.Parse(dir.Substring(1));
                else if (dir.StartsWith('L'))
                    newX -= int.Parse(dir.Substring(1));
                else if (dir.StartsWith('U'))
                    newY -= int.Parse(dir.Substring(1));
                else if (dir.StartsWith('D'))
                    newY += int.Parse(dir.Substring(1));
                segments.Add(new Segment { Start = new Point(x, y), End = new Point(newX, newY) });
                x = newX;
                y = newY;
            }

            return segments;
        }

        private class Segment
        {
            public Point Start;
            public Point End;

            internal Point? IntersectingPoint(Segment other)
            {
                var leftMostThisX = Math.Min(Start.X, End.X);
                var rightMostThisX = Math.Max(Start.X, End.X);
                var topMostThisY = Math.Min(Start.Y, End.Y);
                var bottomMostThisY = Math.Max(Start.Y, End.Y);

                var leftMostThatX = Math.Min(other.Start.X, other.End.X);
                var rightMostThatX = Math.Max(other.Start.X, other.End.X);
                var topMostThatY = Math.Min(other.Start.Y, other.End.Y);
                var bottomMostThatY = Math.Max(other.Start.Y, other.End.Y);

                if (leftMostThisX <= leftMostThatX && rightMostThisX >= rightMostThatX)
                {
                    if (topMostThatY <= topMostThisY && bottomMostThatY >= bottomMostThisY)
                    {
                        if (other.Start.X != 0 && Start.Y != 0)
                            return new Point(other.Start.X, Start.Y);
                    }
                }
                return null;
            }
        }
    }
}