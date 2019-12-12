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

        public string Answer()
        {
            const int numSteps = 1000;
            for (int i = 0; i < numSteps; i++)
            {
                ApplyGravity();
                UpdatePositions();
            }

            int totalEnergy = 0;
            foreach (var moon in moons)
            {
                totalEnergy += (Math.Abs(moon.pos.x) + Math.Abs(moon.pos.y) + Math.Abs(moon.pos.z)) * 
                (Math.Abs(moon.dir.x) + Math.Abs(moon.dir.y) + Math.Abs(moon.dir.z));
            }

            return totalEnergy.ToString();
        }

        private void ApplyGravity()
        {
            for(int p = 0; p < moons.Length - 1; p++) {
                for (int pp = p + 1; pp < moons.Length; pp++) {
                    var moonA = moons[p]; 
                    var moonB = moons[pp];
                    moonA.dir.x += moonA.pos.x == moonB.pos.x ? 0 : moonA.pos.x < moonB.pos.x ? 1 : -1;
                    moonB.dir.x += moonA.pos.x == moonB.pos.x ? 0 : moonA.pos.x < moonB.pos.x ? -1 : 1;
                    moonA.dir.y += moonA.pos.y == moonB.pos.y ? 0 : moonA.pos.y < moonB.pos.y ? 1 : -1;
                    moonB.dir.y += moonA.pos.y == moonB.pos.y ? 0 : moonA.pos.y < moonB.pos.y ? -1 : 1;
                    moonA.dir.z += moonA.pos.z == moonB.pos.z ? 0 : moonA.pos.z < moonB.pos.z ? 1 : -1;
                    moonB.dir.z += moonA.pos.z == moonB.pos.z ? 0 : moonA.pos.z < moonB.pos.z ? -1 : 1;
                }
            }
        }

        private void UpdatePositions()
        {
            for(int p = 0; p < moons.Length; p++) {
                var planet = moons[p];
                planet.pos.x += planet.dir.x;
                planet.pos.y += planet.dir.y;
                planet.pos.z += planet.dir.z;
            }
        }

        public class Moon {
            public Vector3 pos;
            public Vector3 dir = new Vector3();
            public static Moon FromString(string s) {
                var reg = new Regex(@"\<x=(\-?\d*),\sy=(\-?\d*),\sz=(\-?\d*)");
                var matches = reg.Match(s);
                var x = int.Parse(matches.Groups[1].ToString());
                var y = int.Parse(matches.Groups[2].ToString());
                var z = int.Parse(matches.Groups[3].ToString());

                return new Moon() { pos = new Vector3(x, y, z)};
            }

            public override string ToString() => $"pos: {pos.ToString()}, dir: {dir.ToString()}";
        }
    }
}
