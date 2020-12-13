using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../13.in");
var estimatedTime = long.Parse(input[0]);
var busIds = input[1].Split(',').Select(c => c == "x" ? -1 : long.Parse(c)).ToArray();

foreach (var n in InfiniteMultiplies(busIds[0]))
{
    var t = n;
    bool cont = false;

    for (int i = 1; i < busIds.Length; i++)
    {
        t++;
        if (busIds[i] == -1)
            continue;

        var mod = t % busIds[i];
        if (mod != 0)
        {
            cont = true;
            break;
        }
    }

    if (!cont)
    {
        Console.WriteLine(n);
        break;
    }
}

IEnumerable<long> InfiniteMultiplies(long n)
{
    long nn = 0;
    while (true)
    {
        nn += n;
        yield return nn;
    }
}
