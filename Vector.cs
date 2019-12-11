using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace aoc2019
{
    public struct Vector : IEquatable<Vector>
    {
        public int x;
        public int y;

        public Vector(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public Vector(Vector template)
        {
            this.x = template.x;
            this.y = template.y;
        }

        public bool Equals(Vector other)
        {
            return x == other.x && y == other.y;
        }

        public static bool operator ==(Vector a, Vector b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Vector a, Vector b)
        {
            return !a.Equals(b);
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.x + b.x, a.y + b.y);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.x - b.x, a.y - b.y);
        }

        public override string ToString() => $"({x},{y})";
    }
}
