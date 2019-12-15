using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace aoc
{
    public class D32
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

                int minSteps = int.MaxValue;
                int w1Steps = 0;
                foreach (var seg1 in w1)
                {
                    w1Steps += seg1.Length;
                    int w2Steps = 0;
                    foreach (var seg2 in w2)
                    {
                        w2Steps += seg2.Length;
                        var intersection1 = seg1.IntersectingPoint(seg2);
                        var intersection2 = seg2.IntersectingPoint(seg1);
                        var intersection = intersection1.HasValue ? intersection1 : intersection2.HasValue ? intersection2 : null;
                        if (intersection != null)
                        {
                            var totalSteps = w1Steps + w2Steps - seg1.LengthFromPoint(intersection.Value) - seg2.LengthFromPoint(intersection.Value);
                            if (totalSteps < minSteps) {
                                minSteps = totalSteps;
                            }
                        }
                    }
                }

                return minSteps.ToString();
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

            public int LengthFromPoint(Point point) {
                return Segment.LengthBetweenPoints(point, End);
            }

            public int Length => Segment.LengthBetweenPoints(Start, End);

            private static int LengthBetweenPoints(Point p1, Point p2) {
                var p = Subtract(p2, p1);
                return Math.Abs(p.X) + Math.Abs(p.Y);
            }

            private static Point Subtract(Point p1, Point p2) {
                return new Point(p2.X - p1.X, p2.Y - p1.Y);
            }

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