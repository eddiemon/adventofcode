using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using aoc;

var input = File.ReadAllLines("../../../17.in");

var activeStates = new HashSet<Vector3>();
for (int y = 0; y < input.Length; y++)
{
    for (int x = 0; x < input[y].Length; x++)
    {
        if (input[y][x] == '#')
            activeStates.Add(new Vector3(x, y, 0));
    }
}

for (int i = 0; i < 6; i++)
{
    //print(activeStates);

    var minX = activeStates.Min(k => k.x) - 1;
    var maxX = activeStates.Max(k => k.x) + 1;
    var minY = activeStates.Min(k => k.y) - 1;
    var maxY = activeStates.Max(k => k.y) + 1;
    var minZ = activeStates.Min(k => k.z) - 1;
    var maxZ = activeStates.Max(k => k.z) + 1;
    var transforms = new List<Action>();

    for (int z = minZ; z <= maxZ; z++)
    {
        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                var v = new Vector3(x, y, z);
                var neighbours = new HashSet<Vector3>(v.GetNeighbours());

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

    foreach (var transform in transforms)
    {
        transform.Invoke();
    }
}

Console.WriteLine(activeStates.Count());

void print(HashSet<Vector3> activeStates)
{
    var minX = activeStates.Min(k => k.x);
    var maxX = activeStates.Max(k => k.x);
    var minY = activeStates.Min(k => k.y);
    var maxY = activeStates.Max(k => k.y);
    var minZ = activeStates.Min(k => k.z);
    var maxZ = activeStates.Max(k => k.z);
    for (int z = minZ; z <= maxZ; z++)
    {
        Console.WriteLine($"z={z}");
        Console.Write("  ");
        for (int xx = minX; xx <= maxX; xx++)
        {
            Console.Write(xx);
        }
        Console.WriteLine();
        for (int y = minY; y <= maxY; y++)
        {
            Console.Write($"{y} ");
            for (int x = minX; x <= maxX; x++)
            {
                var v = new Vector3(x, y, z);
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
