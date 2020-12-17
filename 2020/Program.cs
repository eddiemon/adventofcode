using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using aoc;

var input = File.ReadAllLines("../../../17.in");

var activeStates = new HashSet<Vector4>();
for (int y = 0; y < input.Length; y++)
{
    for (int x = 0; x < input[y].Length; x++)
    {
        if (input[y][x] == '#')
            activeStates.Add(new Vector4(x, y));
    }
}

var stopwatch = new System.Diagnostics.Stopwatch();
stopwatch.Start();

for (int i = 0; i < 6; i++)
{
    //print(activeStates);

    var minX = activeStates.Min(k => k.x) - 1;
    var maxX = activeStates.Max(k => k.x) + 1;
    var minY = activeStates.Min(k => k.y) - 1;
    var maxY = activeStates.Max(k => k.y) + 1;
    var minZ = activeStates.Min(k => k.z) - 1;
    var maxZ = activeStates.Max(k => k.z) + 1;
    var minT = activeStates.Min(k => k.t) - 1;
    var maxT = activeStates.Max(k => k.t) + 1;
    var transforms = new List<Action>();

    for (int t = minT; t <= maxT; t++)
    {
        for (int z = minZ; z <= maxZ; z++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var v = new Vector4(x, y, z, t);
                    var neighbours = v.GetNeighbours();

                    var activeNeighbours = neighbours.Where(n => activeStates.Contains(n)).Count();
                    if (activeStates.Contains(v))
                    {
                        if (activeNeighbours < 2 || activeNeighbours > 3)
                            transforms.Add(() => activeStates.Remove(v));
                    }
                    else if (activeNeighbours == 3)
                    {
                        transforms.Add(() => activeStates.Add(v));
                    }
                }
            }
        }
    }

    foreach (var transform in transforms)
    {
        transform.Invoke();
    }
}

stopwatch.Stop();
Console.WriteLine(stopwatch.ElapsedMilliseconds);
Console.WriteLine(activeStates.Count());

void print(HashSet<Vector4> activeStates)
{
    var minX = activeStates.Min(k => k.x);
    var maxX = activeStates.Max(k => k.x);
    var minY = activeStates.Min(k => k.y);
    var maxY = activeStates.Max(k => k.y);
    var minZ = activeStates.Min(k => k.z);
    var maxZ = activeStates.Max(k => k.z);
    var minT = activeStates.Min(k => k.t) - 1;
    var maxT = activeStates.Max(k => k.t) + 1;
    for (int t = minT; t <= maxT; t++)
    {
        for (int z = minZ; z <= maxZ; z++)
        {
            Console.WriteLine($"z={z},t={t}");
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var v = new Vector4(x, y, z, t);
                    if (activeStates.Contains(v))
                        Console.Write('#');
                    else
                        Console.Write('.');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
