var input = File.ReadLines("5.txt");

var lines = input
    .Select(l => System.Text.RegularExpressions.Regex.Match(l, @"(\d+),(\d+) -> (\d+),(\d+)"))
    .Select(m => new {
        x1 = int.Parse(m.Groups[1].Value),
        y1 = int.Parse(m.Groups[2].Value),
        x2 = int.Parse(m.Groups[3].Value),
        y2 = int.Parse(m.Groups[4].Value),
    })
    .ToList();

var hvlines = lines
    .Where(l => l.x1 == l.x2 || l.y1 == l.y2)
    .ToList();

var maxX = lines.Max(l => Math.Max(l.x1, l.x2)) + 1;
var maxY = lines.Max(l => Math.Max(l.y1, l.y2)) + 1;

var world = new int[maxY*maxX];
foreach (var l in lines)
{
    var startX = Math.Min(l.x1, l.x2);
    var endX = l.x1 + l.x2 - startX;
    var startY = Math.Min(l.y1, l.y2);
    var endY = l.y1 + l.y2 - startY;


    if (startX == endX)
    {
        for (int y = startY; y <= endY; y++)
        {
            world[y * maxX + startX]++;
        }
    }
    else if (startY == endY)
    {
        for (int x = startX; x <= endX; x++)
        {
            world[startY * maxX + x]++;
        }
    } else {

        for (int y = startY, x = startX; y <= endY && x <= endX; y++, x++)
        {
            world[y * maxX + x]++;
        }
    }
}

var result = world.Count(i => i > 1);

for (int y = 0; y < maxY; y++)
{
    for (int x = 0; x < maxX; x++)
    {
        Console.Write("{0} ", world[y * maxX + x]);
    }
    System.Console.WriteLine();
}

System.Console.WriteLine(result);