using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../3.in").ToList();
var treesPerSlope = new List<BigInteger>(5);

treesPerSlope.Add(CountTreesForSlope(1, 1));
treesPerSlope.Add(CountTreesForSlope(3, 1));
treesPerSlope.Add(CountTreesForSlope(5, 1));
treesPerSlope.Add(CountTreesForSlope(7, 1));
treesPerSlope.Add(CountTreesForSlope(1, 2));

Console.WriteLine(treesPerSlope.Aggregate((a, b) => a * b));

int CountTreesForSlope(int dx, int dy)
{
    var xx = dx;
    var trees = 0;
    for (int y = dy; y < input.Count; y += dy, xx += dx)
    {
        var x = xx % 31;
        if (input[y][x] == '#')
            trees++;
    }
    return trees;
}
