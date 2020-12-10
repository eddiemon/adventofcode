using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../10.in").Select(n => int.Parse(n)).ToList();

input.Sort();
input.Insert(0, 0);
var diff1 = 0;
var diff3 = 0;
for (int i = 1; i < input.Count; i++)
{
    var diff = input[i] - input[i - 1];
    if (diff == 1) diff1++;
    else if (diff == 3) diff3++;
}
diff3++;

Console.WriteLine(diff1 * diff3);
