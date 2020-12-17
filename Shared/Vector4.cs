using System;
using System.Collections.Generic;

namespace aoc
{
    public class Vector4 : IEquatable<Vector4>
    {
        public int x;
        public int y;
        public int z;
        public int t;

        public Vector4(int x = 0, int y = 0, int z = 0, int t = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.t = t;
        }

        public Vector4(Vector4 template)
        {
            this.x = template.x;
            this.y = template.y;
            this.z = template.z;
            this.t = template.t;
        }

        public bool Equals(Vector4 other)
        {
            return x == other.x && y == other.y && z == other.z && t == other.t;
        }

        public override bool Equals(object other)
        {
            return other is Vector4 v ? Equals(v) : false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z, t);
        }

        public static bool operator ==(Vector4 a, Vector4 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Vector4 a, Vector4 b)
        {
            return !a.Equals(b);
        }

        public static Vector4 operator +(Vector4 a, Vector4 b)
        {
            return new Vector4(a.x + b.x, a.y + b.y, a.z + b.z, a.t + b.t);
        }

        public static Vector4 operator -(Vector4 a, Vector4 b)
        {
            return new Vector4(a.x - b.x, a.y - b.y, a.z - b.z, a.t - b.t);
        }

        public static implicit operator (int x, int y, int z, int t)(Vector4 v)
        {
            return (v.x, v.y, v.z, v.t);
        }

        public static implicit operator Vector4((int x, int y, int z, int t) v)
        {
            return new Vector4(v.x, v.y, v.z, v.t);
        }

        public int DistanceTo(Vector4 other)
        {
            return (int)Math.Sqrt(
                Math.Pow(x - other.x, 2) +
                Math.Pow(y - other.y, 2) +
                Math.Pow(z - other.z, 2) +
                Math.Pow(t - other.t, 2)
            );
        }

        public int ManhattanDistanceTo(Vector4 other)
        {
            return Math.Abs(x - other.x) +
                Math.Abs(y - other.y) +
                Math.Abs(z - other.z) +
                Math.Abs(t - other.t);
        }

        public IEnumerable<Vector4> GetNeighbours()
        {
            for (int dt = -1; dt <= 1; dt++)
            {
                for (int dz = -1; dz <= 1; dz++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            if (dz == 0 && dy == 0 && dx == 0 && dt == 0)
                                continue;

                            yield return new Vector4(x + dx, y + dy, z + dz, t + dt);
                        }
                    }
                }
            }
        }

        public override string ToString() => $"({x},{y},{z},{t})";
    }
}
