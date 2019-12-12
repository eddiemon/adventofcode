using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace aoc2019
{
    public struct Vector2 : IEquatable<Vector2>
    {
        public int x;
        public int y;

        public Vector2(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2(Vector2 template)
        {
            this.x = template.x;
            this.y = template.y;
        }

        public bool Equals(Vector2 other)
        {
            return x == other.x && y == other.y;
        }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return !a.Equals(b);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public override string ToString() => $"({x},{y})";
    }
}
