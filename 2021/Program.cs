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
// foreach (var s in visitedPaths.Select(p => string.Join(',', p)))
// {
//     System.Console.WriteLine(s);
// }

IEnumerable<string> SmallCaves(IEnumerable<string> caves) {
    return caves.Where(c => c.ToLower() == c);
}

IEnumerable<string> VisitableCaves(IEnumerable<string> adjecent, IEnumerable<string> visited) {
    var g = visited.Except(new[] { "start" }).Distinct().Select(
        v => new { Node = v, Count = visited.Count(vv => vv == v)}
    );

    var excludeList = new List<string> { "start" };
    var smallCaveVisitedTwice = g.Where(kvp => kvp.Node.ToLower() == kvp.Node && kvp.Count > 1).SingleOrDefault()?.Node;
    if (smallCaveVisitedTwice != null)
    {
        excludeList.AddRange(g.Where(gg => gg.Node.ToLower() == gg.Node).Select(gg => gg.Node));
    }
    return adjecent.Except(excludeList);
}

void VisitNode(string node, Stack<string> visited) {
    visited.Push(node);;
    if (node == "end")
    {
        visitedPaths.Add(visited.Reverse().ToList());
    }
    else
    {
        var adjecentCaves = map[node];
        var visitableAdjecentCaves = VisitableCaves(adjecentCaves, visited).ToList();
        foreach (var adjecentCave in visitableAdjecentCaves)
        {
            VisitNode(adjecentCave, visited);
        }
    }
    visited.Pop();
}