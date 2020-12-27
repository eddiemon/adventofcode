using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using aoc;

Console.OutputEncoding = Encoding.UTF8;

var input = File.ReadAllLines("../../../24.in");
var blackTiles = new HashSet<Vector3>();
foreach (var l in input)
{
    int index = 0;
    int x = 0, y = 0, z = 0;
    while (index < l.Length)
    {
        if (index + 1 < l.Length && l[index..(index + 2)] == "nw")
        {
            z--;
            y++;
            index += 2;
        }
        else if (index + 1 < l.Length && l[index..(index + 2)] == "ne")
        {
            x++;
            z--;
            index += 2;
        }
        else if (index + 1 < l.Length && l[index..(index + 2)] == "sw")
        {
            x--;
            z++;
            index += 2;
        }
        else if (index + 1 < l.Length && l[index..(index + 2)] == "se")
        {
            z++; y--;
            index += 2;
        }
        else if (l[index..(index + 1)] == "w")
        {
            x--;
            y++;
            index++;
        }
        else if (l[index..(index + 1)] == "e")
        {
            x++;
            y--;
            index++;
        }
    }

    if (!blackTiles.Contains((x, y, z)))
        blackTiles.Add((x, y, z));
    else
        blackTiles.Remove((x, y, z));
}

for (int i = 0; i < 100; i++)
{
    var transforms = new List<Action>();
    var tilesToCheck = new HashSet<Vector3>();
    foreach (var v in blackTiles)
    {
        tilesToCheck.Add(v);
        foreach (var vv in v.GetHexagonalNeighbours())
        {
            tilesToCheck.Add(vv);
        }
    }

    foreach (var v in tilesToCheck)
    {
        int blacks = CountBlacks(v);
        transforms.AddRange(AddTransforms(v, blacks));
    }

    transforms.ForEach(t => t());
}

Console.WriteLine(blackTiles.Count);

int CountBlacks(Vector3 v)
{
    int blacks = 0;
    foreach (var vv in v.GetHexagonalNeighbours())
    {
        if (blackTiles.Contains(vv))
            blacks++;
    }

    return blacks;
}

List<Action> AddTransforms(Vector3 v, int blacks)
{
    var transforms = new List<Action>();
    if (blackTiles.Contains(v))
    {
        if (blacks == 0 || blacks > 2)
            transforms.Add(() => blackTiles.Remove(v));
    }
    else
    {
        // tile is white, check for black neighbours
        if (blacks == 2)
            transforms.Add(() => blackTiles.Add(v));
    }

    return transforms;
}
