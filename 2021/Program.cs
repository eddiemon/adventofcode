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

var maxX = lines.Max(l => Math.Max(l.x1, l.x2)) + 1;
var maxY = lines.Max(l => Math.Max(l.y1, l.y2)) + 1;

var world = new int[maxY*maxX];
foreach (var l in lines)
{
    var xs = Range(l.x1, l.x2);
    var ys = Range(l.y1, l.y2);
    var coords = PadZip(xs, ys).ToList();
    foreach (var (x,y) in coords)
    {
        world[y * maxX + x]++;
    }
}

var result = world.Count(i => i > 1);

// for (int y = 0; y < maxY; y++)
// {
//     for (int x = 0; x < maxX; x++)
//     {
//         Console.Write("{0} ", world[y * maxX + x]);
//     }
//     System.Console.WriteLine();
// }

System.Console.WriteLine(result);

IEnumerable<int> Range(int start, int end)
{
    if (start <= end)
    {
        for (int i = start; i <= end; i++)
            yield return i;
    }
    else
    {
        for (int i = start; i >= end; i--)
            yield return i;
    }
}

IEnumerable<(int, int)> PadZip(IEnumerable<int> a, IEnumerable<int> b) {
    var enumA = a.GetEnumerator();
    var enumB = b.GetEnumerator();
    var currentA = 0;
    var currentB = 0;
    var hasMore = true;
    do
    {
        var hasMoreA = enumA.MoveNext();
        if (hasMoreA)
            currentA = enumA.Current;
        var hasMoreB = enumB.MoveNext();
        if (hasMoreB)
            currentB = enumB.Current;
        hasMore = hasMoreA || hasMoreB;
        if (!hasMore)
            break;
        yield return (currentA, currentB);
    } while (true);
}