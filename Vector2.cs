using System;

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

        public static implicit operator (int x, int y)(Vector2 v)
        {
            return (v.x, v.y);
        }

        public static implicit operator Vector2((int x, int y) v)
        {
            return new Vector2(v.x, v.y);
        }

        public override string ToString() => $"({x},{y})";

        public override bool Equals(object obj)
        {
            return obj is Vector2 v ? Equals(v) : false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }
    }
}
