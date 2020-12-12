using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../12.in").Select(l =>
{
    var m = Regex.Match(l, "(?<dir>.)(?<num>\\d+)");
    return new Instruction(m.Groups["dir"].Value[0], int.Parse(m.Groups["num"].Value));
});

int x = 0, y = 0;
int wx = 10, wy = -1;

foreach (var instr in input)
{
    if (instr.direction == 'N')
        wy -= instr.value;
    else if (instr.direction == 'E')
        wx += instr.value;
    else if (instr.direction == 'S')
        wy += instr.value;
    else if (instr.direction == 'W')
        wx -= instr.value;
    else if (instr.direction == 'F')
    {
        y += wy * instr.value;
        x += wx * instr.value;
    }
    else if (instr.direction == 'R')
    {
        for (int i = 0; i < instr.value / 90; i++)
        {
            var oldY = wy;
            wy = wx;
            wx = -oldY;
        }
    }
    else if (instr.direction == 'L')
    {
        for (int i = 0; i < instr.value / 90; i++)
        {
            var oldY = wy;
            wy = -wx;
            wx = oldY;
        }
    }
}

Console.WriteLine(Math.Abs(x + y));

record Instruction(char direction, int value);
