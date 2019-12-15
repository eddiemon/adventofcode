using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc
{
    public class D12
    {
        Moon[] moons = File.ReadAllLines("d12.txt").Select(l => Moon.FromString(l)).ToArray();

        public string Answer()
        {
            var sw = new Stopwatch();
            sw.Start();

            int numSteps = 0;
            var originalX = new int[] { moons[0].pos.x, moons[1].pos.x, moons[2].pos.x, moons[3].pos.x };
            var originalY = new int[] { moons[0].pos.y, moons[1].pos.y, moons[2].pos.y, moons[3].pos.y };
            var originalZ = new int[] { moons[0].pos.z, moons[1].pos.z, moons[2].pos.z, moons[3].pos.z };
            int xRepeat = 0, yRepeat = 0, zRepeat = 0;
            while (xRepeat == 0 || yRepeat == 0 || zRepeat == 0)
            {
                ApplyGravity();
                UpdatePositions();

                if (xRepeat == 0 && axisRepeats(0, originalX))
                 xRepeat = numSteps;
                if (yRepeat == 0 && axisRepeats(1, originalY))
                 yRepeat = numSteps;
                if (zRepeat == 0 && axisRepeats(2, originalZ))
                 zRepeat = numSteps;

                numSteps++;
            }
            sw.Stop();
            System.Console.WriteLine(sw.ElapsedMilliseconds);

            return Maths.lcm(xRepeat + 1, Maths.lcm(yRepeat + 1, zRepeat + 1)).ToString();
        }

        private bool axisRepeats(int axis, int[] originalAxis)
        {
            int[] compareAxis = null;
            if (axis == 0) compareAxis = new int[] { moons[0].pos.x, moons[1].pos.x, moons[2].pos.x, moons[3].pos.x };
            else if (axis == 1) compareAxis = new int[] { moons[0].pos.y, moons[1].pos.y, moons[2].pos.y, moons[3].pos.y };
            else if (axis == 2) compareAxis = new int[] { moons[0].pos.z, moons[1].pos.z, moons[2].pos.z, moons[3].pos.z };
            for (int i = 0; i < 4; i++)
            {
                if (originalAxis[i] != compareAxis[i]) return false;
            }
            return true;
        }

        private int GetHash(Moon m)
        {
            return HashCode.Combine(m.pos, m.vel);
        }

        private int GetHash(Moon[] moons)
        {
            int hash = 0;
            foreach (var m in moons)
            {
                hash = HashCode.Combine(hash, GetHash(m));
            }
            return hash;
        }

        private void ApplyGravity()
        {
            for (int p = 0; p < moons.Length - 1; p++)
            {
                for (int pp = p + 1; pp < moons.Length; pp++)
                {
                    var moonA = moons[p];
                    var moonB = moons[pp];
                    moonA.vel.x += moonA.pos.x == moonB.pos.x ? 0 : moonA.pos.x < moonB.pos.x ? 1 : -1;
                    moonB.vel.x += moonA.pos.x == moonB.pos.x ? 0 : moonA.pos.x < moonB.pos.x ? -1 : 1;
                    moonA.vel.y += moonA.pos.y == moonB.pos.y ? 0 : moonA.pos.y < moonB.pos.y ? 1 : -1;
                    moonB.vel.y += moonA.pos.y == moonB.pos.y ? 0 : moonA.pos.y < moonB.pos.y ? -1 : 1;
                    moonA.vel.z += moonA.pos.z == moonB.pos.z ? 0 : moonA.pos.z < moonB.pos.z ? 1 : -1;
                    moonB.vel.z += moonA.pos.z == moonB.pos.z ? 0 : moonA.pos.z < moonB.pos.z ? -1 : 1;
                }
            }
        }

        private void UpdatePositions()
        {
            Parallel.ForEach(moons, (moon) =>
            {
                moon.pos.x += moon.vel.x;
                moon.pos.y += moon.vel.y;
                moon.pos.z += moon.vel.z;
            });
        }

        public class Moon : IEquatable<Moon>
        {
            private static int id = 0;

            public Vector3 pos;
            public Vector3 vel = new Vector3();
            private int MoonId;

            public Moon()
            {
                MoonId = id++;
            }

            public Moon(Moon template)
            {
                pos = template.pos;
                vel = template.vel;
                MoonId = id++;
            }

            public static Moon FromString(string s)
            {
                var reg = new Regex(@"\<x=(\-?\d*),\sy=(\-?\d*),\sz=(\-?\d*)");
                var matches = reg.Match(s);
                var x = int.Parse(matches.Groups[1].ToString());
                var y = int.Parse(matches.Groups[2].ToString());
                var z = int.Parse(matches.Groups[3].ToString());

                return new Moon() { pos = new Vector3(x, y, z) };
            }

            public bool Equals(Moon other)
            {
                return other != null && pos == other.pos && vel == other.vel;
            }

            public override bool Equals(object obj)
            {
                return obj is Moon m ? Equals(m) : false;
            }

            public override int GetHashCode()
            {
                return MoonId;
            }

            public override string ToString() => $"pos: {pos.ToString()}, dir: {vel.ToString()}";
        }
    }
}
