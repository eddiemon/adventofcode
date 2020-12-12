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
        y += dy * instr.value;
        x += dx * instr.value;
    }
    else if (instr.direction == 'B')
    {
        y -= dy * instr.value;
        x -= dx * instr.value;
    }
    else if (instr.direction == 'R')
    {
        for (int i = 0; i < instr.value / 90; i++)
        {
            if (dx == 1)
            {
                dx = 0;
                dy = 1;
            }
            else if (dy == 1)
            {
                dx = -1;
                dy = 0;
            }
            else if (dx == -1)
            {
                dx = 0;
                dy = -1;
            }
            else if (dy == -1)
            {
                dx = 1;
                dy = 0;
            }
        }
    }
    else if (instr.direction == 'L')
    {
        for (int i = 0; i < instr.value / 90; i++)
        {
            if (dx == 1)
            {
                dx = 0;
                dy = -1;
            }
            else if (dy == 1)
            {
                dx = 1;
                dy = 0;
            }
            else if (dx == -1)
            {
                dx = 0;
                dy = 1;
            }
            else if (dy == -1)
            {
                dx = -1;
                dy = 0;
            }
        }
    }
}

Console.WriteLine(Math.Abs(x + y));

record Instruction(char direction, int value);
