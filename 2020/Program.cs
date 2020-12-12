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
var d = new int[] { 0, 1, 0, -1 };
int dx = 1, dy = 0;

foreach (var instr in input)
{
    if (instr.direction == 'N')
        y -= instr.value;
    else if (instr.direction == 'E')
        x += instr.value;
    else if (instr.direction == 'S')
        y += instr.value;
    else if (instr.direction == 'W')
        x -= instr.value;
    else if (instr.direction == 'F')
    {
        y += d[dy] * instr.value;
        x += d[dx] * instr.value;
    }
    else if (instr.direction == 'B')
    {
        y -= d[dy] * instr.value;
        x -= d[dx] * instr.value;
    }
    else if (instr.direction == 'R')
    {
        for (int i = 0; i < instr.value / 90; i++)
        {
            dx = mod(dx + 1, d.Length);
            dy = mod(dy + 1, d.Length);
        }
    }
    else if (instr.direction == 'L')
    {
        for (int i = 0; i < instr.value / 90; i++)
        {
            dx = mod(dx - 1, d.Length);
            dy = mod(dy - 1, d.Length);
        }
    }
}

Console.WriteLine(Math.Abs(x + y));

int mod(int x, int m)
{
    return (x % m + m) % m;
}

record Instruction(char direction, int value);
