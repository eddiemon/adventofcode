using aoc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc2018
{
    internal class D25
    {
        public object Answer()
        {
            var points = File.ReadAllLines("25.in").Select(l => ToVector4(l)).ToList();

            var unvisited = points;
            var G = new List<List<Vector4>>();

            while (unvisited.Count > 0)
            {
                var p = unvisited[0];
                G.Add(FindCloseBy(p, unvisited));
            }

            return G.Count;
        }

        private static List<Vector4> FindCloseBy(Vector4 p, List<Vector4> unvisited)
        {
            var g = new List<Vector4>();
            g.Add(p);
            unvisited.Remove(p);
            var closeBy = unvisited.Where(pp => p.ManhattanDistanceTo(pp) <= 3).ToList();
            foreach (var pp in closeBy)
            {
                g.Add(pp);
                unvisited.Remove(pp);
            }
            foreach (var pp in closeBy)
            {
                FindCloseBy(pp, unvisited);
            }

            return g;
        }

        private Vector4 ToVector4(string l)
        {
            var s = l.Split(',');
            return new Vector4(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]), int.Parse(s[3]));
        }
    }
}