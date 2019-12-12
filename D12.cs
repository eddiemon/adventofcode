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
        
        Planet[] planets = File.ReadAllLines("d12.1.txt").Select(l => Planet.FromString(l)).ToArray();

        public string Answer()
        {
            const int numSteps = 10;
            for (int i = 0; i < numSteps; i++)
            {
                ApplyGravity();
                UpdatePositions();
            }


            return string.Empty;
        }

        private void ApplyGravity()
        {
            for(int p = 0; p < planets.Length - 1; p++) {
                for (int pp = p + 1; pp < planets.Length; pp++) {
                    var planetA = planets[p];
                    var planetB = planets[pp];
                    planetA.dir.x += planetA.pos.x < planetB.pos.x ? 1 : -1;
                    planetB.dir.x += planetA.pos.x < planetB.pos.x ? -1 : 1;
                    planetA.dir.y += planetA.pos.y < planetB.pos.y ? 1 : -1;
                    planetB.dir.y += planetA.pos.y < planetB.pos.y ? -1 : 1;
                    planetA.dir.z += planetA.pos.z < planetB.pos.z ? 1 : -1;
                    planetB.dir.z += planetA.pos.z < planetB.pos.z ? -1 : 1;
                }
            }
        }

        private void UpdatePositions()
        {
            for(int p = 0; p < planets.Length; p++) {
                var planet = planets[p];
                planet.pos.x += planet.dir.x;
                planet.pos.y += planet.dir.y;
                planet.pos.z += planet.dir.z;
            }
        }

        public class Planet {
            public Vector3 pos;
            public Vector3 dir = new Vector3();
            public static Planet FromString(string s) {
                var reg = new Regex(@"\<x=(\-?\d*),\sy=(\-?\d*),\sz=(\-?\d*)");
                var matches = reg.Match(s);
                var x = int.Parse(matches.Groups[1].ToString());
                var y = int.Parse(matches.Groups[2].ToString());
                var z = int.Parse(matches.Groups[3].ToString());

                return new Planet() { pos = new Vector3(x, y, z)};
            }

            public override string ToString() => $"pos: {pos.ToString()}, dir: {dir.ToString()}";
        }
    }
}
