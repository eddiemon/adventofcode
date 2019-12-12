using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace aoc2019
{
    public class D12
    {

        Moon[] moons = File.ReadAllLines("d12.txt").Select(l => Moon.FromString(l)).ToArray();

        // Dictionary<Moon, HashSet<(Vector3, Vector3)>> Visited = new Dictionary<Moon, HashSet<(Vector3, Vector3)>>();
        // HashSet<int> HasVisitedAllPosAndVel = new HashSet<int>();
        // List<int> visited = new List<int>();

        public string Answer()
        {
            var sw = new Stopwatch();
            sw.Start();
            long numSteps = 0;
            var originalHash = (
                moons[0].pos, moons[0].vel,
                moons[1].pos, moons[1].vel,
                moons[2].pos, moons[2].vel,
                moons[3].pos, moons[3].vel
            );

            long lastNumSteps = 0;
            while (true)
            {
                ApplyGravity();
                UpdatePositions();

                // var hash = GetHash(moons);
                if (originalHash == (
                    moons[0].pos, moons[0].vel,
                    moons[1].pos, moons[1].vel,
                    moons[2].pos, moons[2].vel,
                    moons[3].pos, moons[3].vel
                ))
                    break;

                // visited.Add(hash);
                if (sw.ElapsedMilliseconds > 5000)
                {
                    sw.Restart();
                    var diff = numSteps - lastNumSteps;
                    System.Console.WriteLine($"Steps/s: {(diff / 5.0)}");
                    lastNumSteps = numSteps;
                }
                numSteps++;
            }
            sw.Stop();
            System.Console.WriteLine(sw.ElapsedMilliseconds);

            return numSteps.ToString();
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
