using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../13.in");
var estimatedTime = long.Parse(input[0]);
var busIds = input[1].Split(',').Select(c => c == "x" ? -1 : long.Parse(c)).ToArray();

var maxBus = busIds.Select((id, n) => (id, n)).OrderByDescending(x => x.id).First();

foreach (var n in InfiniteMultiplies(maxBus.id))
{
    var t = n - maxBus.n - 1;
    bool cont = false;

    for (int i = 0; i < busIds.Length; i++)
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
        Console.WriteLine(n - maxBus.n);
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
