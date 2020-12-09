using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../9.in").Select(n => long.Parse(n)).ToArray();
const long number = 138879426;
int l = 0, h = 1;
long s = input[l] + input[h];

while (s != number)
{
    if (s < number)
        s += input[++h];
    else if (s > number)
        s -= input[l++];
}

var ii = input[l..(h + 1)];
Console.WriteLine(ii.Min() + ii.Max());
