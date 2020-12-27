using System;
using System.Collections.Generic;

namespace aoc
{
    public class Vector3 : IEquatable<Vector3>
    {
        public int x;
        public int y;
        public int z;

        public Vector3(int x = 0, int y = 0, int z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(Vector3 template)
        {
            this.x = template.x;
            this.y = template.y;
            this.z = template.z;
        }

        public bool Equals(Vector3 other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        public override bool Equals(object other)
        {
            return other is Vector3 v ? Equals(v) : false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }

        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !a.Equals(b);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static implicit operator (int x, int y, int z)(Vector3 v)
        {
            return (v.x, v.y, v.z);
        }

        public static implicit operator Vector3((int x, int y, int z) v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public int DistanceTo(Vector3 other)
        {
            return (int)Math.Sqrt(
                Math.Pow(x - other.x, 2) +
                Math.Pow(y - other.y, 2) +
                Math.Pow(z - other.z, 2)
            );
        }

        public int ManhattanDistanceTo(Vector3 other)
        {
            return Math.Abs(x - other.x) +
                Math.Abs(y - other.y) +
                Math.Abs(z - other.z);
        }

        public IEnumerable<Vector3> GetNeighbours()
        {
            for (int dz = -1; dz <= 1; dz++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        if (dz == 0 && dy == 0 && dx == 0)
                            continue;

                        yield return new Vector3(x + dx, y + dy, z + dz);
                    }
                }
            }
        }

        /// <summary>
        /// Vector3 can be used to describe a coordinate in hex grids using cube coordinates.
        /// This method return the six neighbours.
        /// </summary>
        public IEnumerable<Vector3> GetHexagonalNeighbours()
        {
            yield return new Vector3(x + 0, y + 1, z - 1);
            yield return new Vector3(x + 1, y + 0, z - 1);
            yield return new Vector3(x + 1, y - 1, z + 0);
            yield return new Vector3(x + 0, y - 1, z + 1);
            yield return new Vector3(x - 1, y + 0, z + 1);
            yield return new Vector3(x - 1, y + 1, z + 0);
        }

        public override string ToString() => $"({x},{y},{z})";
    }
}
