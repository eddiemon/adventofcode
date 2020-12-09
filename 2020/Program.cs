using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../9.in").Select(n => long.Parse(n)).ToArray();
const int preamble = 25;
for (int i = preamble; i < input.Length; i++)
{
    var inp = input[(i - preamble)..(i)];
    var sums = new List<long>();
    for (int x = 0; x < inp.Length - 1; x++)
    {
        for (int y = x + 1; y < inp.Length; y++)
        {
            sums.Add(inp[x] + inp[y]);
        }
    }
    if (!sums.Contains(input[i]))
    {
        Console.WriteLine(input[i]);
        return;
    }
}
