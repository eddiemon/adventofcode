using System;
using System.Collections.Generic;
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

        Dictionary<Moon, HashSet<Vector3>> VisitedPositions = new Dictionary<Moon, HashSet<Vector3>>();
        Dictionary<Moon, HashSet<Vector3>> VisitedVelocities = new Dictionary<Moon, HashSet<Vector3>>();
        HashSet<Moon> HasVisitedAllPosAndVel = new HashSet<Moon>();

        public string Answer()
        {
            foreach (var moon in moons)
            {
                VisitedPositions[moon] = new HashSet<Vector3>();
                VisitedVelocities[moon] = new HashSet<Vector3>();
            }

            int numSteps = 0;
            while (true)
            {
                ApplyGravity();
                UpdatePositions();

                SavePositionsAndDirs();

                if (AllMoonsVisitedSamePosAndVel()) break;

                numSteps++;
            }

            return numSteps.ToString();
        }

        private bool AllMoonsVisitedSamePosAndVel()
        {
            return HasVisitedAllPosAndVel.Count == moons.Length;
        }

        private void SavePositionsAndDirs()
        {
            foreach (var moon in moons)
            {
                if (HasVisitedAllPosAndVel.Contains(moon)) continue;

                if (VisitedPositions.TryGetValue(moon, out var visitedPos) && visitedPos.Contains(moon.pos) && VisitedVelocities.TryGetValue(moon, out var visitedVel) && visitedVel.Contains(moon.vel)) {
                    HasVisitedAllPosAndVel.Add(moon);
                    continue;
                }

                VisitedPositions[moon].Add(moon.pos);
                VisitedVelocities[moon].Add(moon.vel);
            }
        }

        private void ApplyGravity()
        {
            for(int p = 0; p < moons.Length - 1; p++) {
                for (int pp = p + 1; pp < moons.Length; pp++) {
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
            for(int p = 0; p < moons.Length; p++) {
                var planet = moons[p];
                planet.pos.x += planet.vel.x;
                planet.pos.y += planet.vel.y;
                planet.pos.z += planet.vel.z;
            }
        }

        public class Moon {
            public Vector3 pos;
            public Vector3 vel = new Vector3();
            public static Moon FromString(string s) {
                var reg = new Regex(@"\<x=(\-?\d*),\sy=(\-?\d*),\sz=(\-?\d*)");
                var matches = reg.Match(s);
                var x = int.Parse(matches.Groups[1].ToString());
                var y = int.Parse(matches.Groups[2].ToString());
                var z = int.Parse(matches.Groups[3].ToString());

                return new Moon() { pos = new Vector3(x, y, z)};
            }

            public override string ToString() => $"pos: {pos.ToString()}, dir: {vel.ToString()}";
        }
    }
}
