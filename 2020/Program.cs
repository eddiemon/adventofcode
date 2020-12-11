using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../11.in").Select(l => l.ToCharArray()).ToArray();

var changes = new List<Action>();
int rounds = -1;
do
{
    rounds++;
    changes = new List<Action>();

    for (int y = 0; y < input.Length; y++)
    {
        for (int x = 0; x < input[0].Length; x++)
        {
            if (input[y][x] == '.')
                continue;

            int occupiedSeats = 0;
            for (int dy = -1; dy < 2; dy++)
            {
                for (int dx = -1; dx < 2; dx++)
                {
                    var yy = y + dy;
                    var xx = x + dx;
                    if (yy < 0 || yy >= input.Length || xx < 0 || xx >= input[0].Length || (yy == y && xx == x))
                        continue;

                    if (input[yy][xx] == '#')
                        occupiedSeats++;
                }
            }

            if (input[y][x] == 'L' && occupiedSeats == 0)
            {
                int ly = y, lx = x;
                changes.Add(() => input[ly][lx] = '#');
            }
            else if (input[y][x] == '#' && occupiedSeats >= 4)
            {
                int ly = y, lx = x;
                changes.Add(() => input[ly][lx] = 'L');
            }
        }
    }

    foreach (var change in changes)
    {
        change.Invoke();
    }
}
while (changes.Count != 0);

var count = 0;
for (int y = 0; y < input.Length; y++)
{
    for (int x = 0; x < input[0].Length; x++)
    {
        if (input[y][x] == '#')
            count++;
    }
}

Console.WriteLine(rounds);
Console.WriteLine(count);
