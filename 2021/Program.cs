var input = File.ReadLines("12.txt");

var map = input.Select(s => s.Split('-'))
    .GroupBy(l => l[0])
    .ToDictionary(g => g.Key, g => g.Select(x => x[1]).ToHashSet());

var keys = map.Keys.Except(new [] { "start"}).ToList();
foreach (var key in keys)
    foreach (var connection in map[key])
    {
        if (connection == "end") continue;
        if (!map.ContainsKey(connection))
            map[connection] = new HashSet<string>();
        map[connection].Add(key);
    }

var visitedPaths = new List<List<string>>();
VisitNode("start", new Stack<string>());
System.Console.WriteLine(visitedPaths.Count);
// var s = visitedPaths.Select(p => string.Join(',', p)).ToList();
// s.Sort(StringComparer.InvariantCulture);

// foreach (var visitedPath in s)
// {
//     System.Console.WriteLine(visitedPath);
// }

IEnumerable<string> SmallCaves(IEnumerable<string> caves) {
    return caves.Where(c => c.ToLower() == c);
}

void VisitNode(string node, Stack<string> visited) {
    visited.Push(node);
    if (node == "end")
    {
        visitedPaths.Add(visited.Reverse().ToList());
    }
    else
    {
        var adjecentCaves = map[node];
        var adjecentCavesWithoutVisitedSmallCaves = adjecentCaves.Except(SmallCaves(visited));
        foreach (var adjecentCave in adjecentCavesWithoutVisitedSmallCaves)
        {
            VisitNode(adjecentCave, visited);
        }
    }
    visited.Pop();
}