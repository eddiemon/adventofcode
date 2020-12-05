using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../5.in");

var seats = new bool[128 * 8];
Array.Fill(seats, false);

foreach (var l in input)
{
    var yl = 0;
    var yh = 127;
    for (int i = 0; i < 7; i++)
    {
        var c = l[i];
        if (c == 'F')
            yh -= (yh - yl + 1) / 2;
        else if (c == 'B')
            yl += (yh - yl + 1) / 2;
    }

    var xl = 0;
    var xh = 7;
    for (int i = 0; i < 3; i++)
    {
        var c = l[7 + i];
        if (c == 'L')
            xh -= (xh - xl + 1) / 2;
        else if (c == 'R')
            xl += (xh - xl + 1) / 2;
    }

    seats[yl * 8 + xl] = true;
}

for (int y = 1; y < 127; y++)
{
    for (int x = 1; x < 7; x++)
    {
        if (!seats[y * 8 + x] && seats[y * 8 + x - 1] && seats[y * 8 + x + 1])
        {
            Console.Write(y * 8 + x);
            return;
        }
    }
}
