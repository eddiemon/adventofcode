using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using aoc;

var input = File.ReadAllLines("../../../10.in").Select(n => int.Parse(n)).ToList();

input.Sort();
input.Insert(0, 0);
input.Add(input.Max() + 3);

int a = 0, b = 0, c = 0;
int ones = 0;
for (int i = 1; i < input.Count; i++)
{
    if (input[i] - input[i - 1] == 3)
    {
        if (ones == 4) a++;
        else if (ones == 3) b++;
        else if (ones == 2) c++;
        ones = 0;
    }
    else if (input[i] - input[i - 1] == 1)
        ones++;
}

Console.WriteLine(Math.Pow(7, a) * Math.Pow(4, b) * Math.Pow(2, c));
