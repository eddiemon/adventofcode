var input = File.ReadLines("15.txt");

var cost = new Dictionary<(int x, int y), int> {
    [(0,0)] = 1,
    [(1,0)] = 1,
    [(0,1)] = 2,
    [(1,1)] = 1,
};

var maxX = cost.Keys.MaxBy(k => k.x).x;
var maxY = cost.Keys.MaxBy(k => k.y).y;

var dist = cost.Keys.ToDictionary(k => k, _ => int.MaxValue);
var prev = cost.Keys.ToDictionary(k => k, _ => ((int,int)?) null);
dist[(0,0)] = 0;

var q = new PriorityQueue<(int x, int y), int>();
q.Enqueue((0,0), 0);

var neighboursDelta = new List<(int x, int y)> {
    (-1,1),(0,1),(1, 1),
    (-1,0),      (1, 0),
    ( 1,1),(1,1),(1,-1),
};

while (q.Count > 0) {
    var u = q.Dequeue();
    var neighbours = neighboursDelta
        .Select(nd => (x: u.x + nd.x, y: u.y + nd.y))
        .Where(n => n.x >= 0 && n.x <= maxX && n.y >= 0 && n.y <= maxY)
        .ToList();

    foreach (var v in neighbours)
    {
        var alt = dist[u] + cost[v];
        if (alt < dist[v]) {
            dist[v] = alt;
            prev[v] = u;
            if (!q.Contains(v))
        }
    }
}