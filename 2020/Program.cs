using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using aoc;

//var input = File.ReadAllLines("../../../22.in");
var cups = "614752839";
for (int i = 0; i < 100; i++)
{
    var currCup = i % cups.Length;
    var cupLabel = cups[currCup];
    Console.WriteLine($"Move {i + 1}");
    Console.WriteLine($"cups: {cups}");
    Console.WriteLine($"current cup: {cups[currCup]}");
    Console.WriteLine();

    string cupsToMove;
    if (currCup + 4 >= cups.Length)
    {
        var lo = cups[0..((currCup + 4) % cups.Length)];
        var hi = cups[^(3 - lo.Length)..];
        if (lo.Length > 0)
            cups = cups.Replace(lo, "");
        if (hi.Length > 0)
            cups = cups.Replace(hi, "");
        cupsToMove = hi + lo;
    }
    else
    {
        cupsToMove = cups[(currCup + 1)..(currCup + 4)];
        cups = cups.Replace(cupsToMove, "");
    }

    var dest = (char)(cupLabel - 1);
    while (true)
    {
        if (dest < '1') dest = '9';

        if (cupsToMove.IndexOf(dest) == -1)
            break;

        dest = (char)(dest - 1);
    }

    cups = cups.Replace(dest.ToString(), dest.ToString() + cupsToMove);
    while (cups[currCup] != cupLabel)
        cups = cups[1..] + cups[0];
}

while (cups[^1] != '1')
    cups = cups[1..] + cups[0];

Console.WriteLine(cups[..^1]);
