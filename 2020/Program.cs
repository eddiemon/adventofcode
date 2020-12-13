using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../13.in");

var estimatedTime = long.Parse(input[0]);
var busIds = input[1].Split(',').Select((c, idx) => (n: long.TryParse(c, out var l) ? l : -1, idx: idx)).Where(b => b.n != -1).ToArray();

// Use Chinese Remainder Theorem
var N = busIds.Select(b => b.n).Aggregate((n1, n2) => n1 * n2);
var result = busIds.Select(b =>
{
    var ni = b.n;
    var Ni = N / ni;
    var bi = (ni - b.idx) % ni;
    if (bi == 0)
        return 0;

    var x = Maths.ModularMultiplicativeInverse(Ni, ni);

    return bi * Ni * x;
}).Aggregate((n1, n2) => n1 + n2);

Console.WriteLine(result % N);
