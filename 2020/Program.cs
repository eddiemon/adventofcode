using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using aoc;

var input = File.ReadAllLines("../../../20.in");

int tileId = 0;
var tileBuffer = new List<string>();
var unplacedTiles = new List<Tile>();
foreach (var l in input)
{
    if (string.IsNullOrEmpty(l))
    {
        unplacedTiles.Add(new Tile(tileId, tileBuffer.ToArray()));
        tileBuffer.Clear();
    }
    else if (l.StartsWith("Tile"))
    {
        tileId = int.Parse(l[^5..^1]);
    }
    else
    {
        tileBuffer.Add(l);
    }
}

var adjecentTileCandidates = new Dictionary<Tile, List<List<Tile>>>();
foreach (var tile in unplacedTiles)
{
    adjecentTileCandidates.Add(tile, FindAdjecentTileCandidates(tile));
}

var edgeTiles = adjecentTileCandidates.Where(kvp => kvp.Value.Any(adjTiles => adjTiles.Count == 0)).Select(kvp => kvp.Key).ToList();
var cornerTiles = edgeTiles.Where(t => adjecentTileCandidates[t].Where(adjTiles => adjTiles.Count == 0).Count() == 2).ToList();
Console.WriteLine(edgeTiles.Count);
Console.WriteLine(cornerTiles.Select(t => t.Id).Aggregate((a, b) => a * b));

//var placedTiles = new Dictionary<Vector2, Tile>();

//PlaceAdjecentTiles(cornerTiles.First(), 0, 0);

//var minX = placedTiles.Keys.Min(u => u.x);
//var maxX = placedTiles.Keys.Max(u => u.x);
//var minY = placedTiles.Keys.Min(u => u.y);
//var maxY = placedTiles.Keys.Max(u => u.y);

//for (int y = minY; y <= maxY; y++)
//{
//    for (int x = minX; x <= maxX; x++)
//    {
//        var tileKey = placedTiles.Keys.FirstOrDefault(t => t.x == x && t.y == y);
//        var tId = placedTiles[tileKey].Id.ToString() ?? "9999";
//        Console.Write($" {tId} ");
//    }
//    Console.WriteLine();
//}

//var topLeftTile = placedTiles[(minX, minY)];
//var topRightTile = placedTiles[(maxX, minY)];
//var bottomLeftTile = placedTiles[(minX, maxY)];
//var bottomRightTile = placedTiles[(maxX, maxY)];

//Console.WriteLine(topLeftTile.Id * topRightTile.Id * bottomLeftTile.Id * bottomRightTile.Id);

List<List<Tile>> FindAdjecentTileCandidates(Tile tile)
{
    var north = new List<Tile>();
    var south = new List<Tile>();
    var east = new List<Tile>();
    var west = new List<Tile>();
    var result = new List<List<Tile>> { north, south, east, west };

    var dir = tile.GetNorth();
    foreach (var t in UnplacedTiles(tile))
    {
        if (t.GetWest() == dir)
            north.Add(t.RotateCCW());
        else if (t.GetWestReverse() == dir)
            north.Add(t.RotateCCW().FlipHorizontal());
        else if (t.GetEast() == dir)
            north.Add(t.RotateCW().FlipVertical());
        else if (t.GetEastReverse() == dir)
            north.Add(t.RotateCW());
        else if (t.GetNorth() == dir)
            north.Add(t.FlipHorizontal());
        else if (t.GetNorthReverse() == dir)
            north.Add(t.FlipHorizontal().FlipVertical());
        else if (t.GetSouth() == dir)
            north.Add(t);
        else if (t.GetSouthReverse() == dir)
            north.Add(t.FlipVertical());
    }

    dir = tile.GetSouth();
    foreach (var t in UnplacedTiles(tile))
    {
        if (t.GetWest() == dir)
            south.Add(t.RotateCW().FlipVertical());
        else if (t.GetWestReverse() == dir)
            south.Add(t.RotateCW());
        else if (t.GetEast() == dir)
            south.Add(t.RotateCCW());
        else if (t.GetEastReverse() == dir)
            south.Add(t.RotateCCW().FlipVertical());
        else if (t.GetNorth() == dir)
            south.Add(t);
        else if (t.GetNorthReverse() == dir)
            south.Add(t.FlipVertical());
        else if (t.GetSouth() == dir)
            south.Add(t.FlipHorizontal());
        else if (t.GetSouthReverse() == dir)
            south.Add(t.FlipHorizontal().FlipVertical());
    }

    dir = tile.GetEast();
    foreach (var t in UnplacedTiles(tile))
    {
        if (t.GetWest() == dir)
            east.Add(t);
        else if (t.GetWestReverse() == dir)
            east.Add(t.FlipHorizontal());
        else if (t.GetEast() == dir)
            east.Add(t.FlipVertical());
        else if (t.GetEastReverse() == dir)
            east.Add(t.FlipVertical().FlipHorizontal());
        else if (t.GetNorth() == dir)
            east.Add(t.RotateCCW().FlipVertical());
        else if (t.GetNorthReverse() == dir)
            east.Add(t.RotateCCW());
        else if (t.GetSouth() == dir)
            east.Add(t.RotateCW());
        else if (t.GetSouthReverse() == dir)
            east.Add(t.RotateCW().FlipVertical());
    }

    dir = tile.GetWest();
    foreach (var t in UnplacedTiles(tile))
    {
        if (t.GetWest() == dir)
            west.Add(t.FlipVertical());
        else if (t.GetWestReverse() == dir)
            west.Add(t.FlipVertical().FlipHorizontal());
        else if (t.GetEast() == dir)
            west.Add(t);
        else if (t.GetEastReverse() == dir)
            west.Add(t.FlipHorizontal());
        else if (t.GetNorth() == dir)
            west.Add(t.RotateCW());
        else if (t.GetNorthReverse() == dir)
            west.Add(t.RotateCW().FlipVertical());
        else if (t.GetSouth() == dir)
            west.Add(t.RotateCCW().FlipVertical());
        else if (t.GetSouthReverse() == dir)
            west.Add(t.RotateCCW());
    }

    return result;
}

void PlaceAdjecentTiles(Tile tile, int x, int y)
{
    //if (placedTiles.Keys.Contains((x, y)))
    //    throw new Exception();

    //var northCandidates = adjecentTileCandidates[tile][0];
    //var southCandidates = adjecentTileCandidates[tile][1];
    //var eastCandidates = adjecentTileCandidates[tile][2];
    //var westCandidates = adjecentTileCandidates[tile][3];

    ////var dir = tile.GetNorth();
    //while (northCandidates.Count > 1)
    //{
    //    var candidateTile = northCandidates.First();
    //    try
    //    {
    //        PlaceAdjecentTiles(candidateTile, x, y - 1);
    //    }
    //    catch
    //    {
    //        northCandidates.Remove(candidateTile);
    //        placedTiles.Remove((x, y - 1));
    //    }
    //}

    //placedTiles.Add((x, y), tile);
    //unplacedTiles.Remove(tile);

    //foreach (var t in UnplacedTiles())
    //{
    //    if (t.GetWest() == dir)
    //    {
    //        PlaceAdjecentTiles(t.RotateCCW(), x, y - 1);
    //        //break;
    //    }
    //    else if (t.GetWestReverse() == dir)
    //    {
    //        PlaceAdjecentTiles(t.RotateCCW().FlipHorizontal(), x, y - 1);
    //        //break;
    //    }
    //    else if (t.GetEast() == dir)
    //    {
    //        PlaceAdjecentTiles(t.RotateCW().FlipVertical(), x, y - 1);
    //        //break;
    //    }
    //    else if (t.GetEastReverse() == dir)
    //    {
    //        PlaceAdjecentTiles(t.RotateCW(), x, y - 1);
    //        //break;
    //    }
    //    else if (t.GetNorth() == dir)
    //    {
    //        PlaceAdjecentTiles(t.FlipHorizontal(), x, y - 1);
    //        //break;
    //    }
    //    else if (t.GetNorthReverse() == dir)
    //    {
    //        PlaceAdjecentTiles(t.FlipHorizontal().FlipVertical(), x, y - 1);
    //        //break;
    //    }
    //    else if (t.GetSouth() == dir)
    //    {
    //        PlaceAdjecentTiles(t, x, y - 1);
    //        //break;
    //    }
    //    else if (t.GetSouthReverse() == dir)
    //    {
    //        PlaceAdjecentTiles(t.FlipVertical(), x, y - 1);
    //        //break;
    //    }
    //}

    //dir = tile.GetSouth();
    //foreach (var t in UnplacedTiles())
    //{
    //    if (t.GetWest() == dir)
    //    {
    //        PlaceAdjecentTiles(t.RotateCW().FlipVertical(), x, y + 1);
    //        //break;
    //    }
    //    else if (t.GetWestReverse() == dir)
    //    {
    //        PlaceAdjecentTiles(t.RotateCW(), x, y + 1);
    //        //break;
    //    }
    //    else if (t.GetEast() == dir)
    //    {
    //        PlaceAdjecentTiles(t.RotateCCW(), x, y + 1);
    //        //break;
    //    }
    //    else if (t.GetEastReverse() == dir)
    //    {
    //        PlaceAdjecentTiles(t.RotateCCW().FlipVertical(), x, y + 1);
    //        //break;
    //    }
    //    else if (t.GetNorth() == dir)
    //    {
    //        PlaceAdjecentTiles(t, x, y + 1);
    //        //break;
    //    }
    //    else if (t.GetNorthReverse() == dir)
    //    {
    //        PlaceAdjecentTiles(t.FlipVertical(), x, y + 1);
    //        //break;
    //    }
    //    else if (t.GetSouth() == dir)
    //    {
    //        PlaceAdjecentTiles(t.FlipHorizontal(), x, y + 1);
    //        //break;
    //    }
    //    else if (t.GetSouthReverse() == dir)
    //    {
    //        PlaceAdjecentTiles(t.FlipHorizontal().FlipVertical(), x, y + 1);
    //        //break;
    //    }
    //}

    //dir = tile.GetEast();
    //foreach (var t in UnplacedTiles())
    //{
    //    if (t.GetWest() == dir)
    //    {
    //        PlaceAdjecentTiles(t, x + 1, y);
    //        //break;
    //    }
    //    else if (t.GetWestReverse() == dir)
    //    {
    //        PlaceAdjecentTiles(t.FlipHorizontal(), x + 1, y);
    //        //break;
    //    }
    //    else if (t.GetEast() == dir)
    //    {
    //        PlaceAdjecentTiles(t.FlipVertical(), x + 1, y);
    //        //break;
    //    }
    //    else if (t.GetEastReverse() == dir)
    //    {
    //        PlaceAdjecentTiles(t.FlipVertical().FlipHorizontal(), x + 1, y);
    //        //break;
    //    }
    //    else if (t.GetNorth() == dir)
    //    {
    //        PlaceAdjecentTiles(t.RotateCCW().FlipVertical(), x + 1, y);
    //        //break;
    //    }
    //    else if (t.GetNorthReverse() == dir)
    //    {
    //        PlaceAdjecentTiles(t.RotateCCW(), x + 1, y);
    //        //break;
    //    }
    //    else if (t.GetSouth() == dir)
    //    {
    //        PlaceAdjecentTiles(t.RotateCW(), x + 1, y);
    //        //break;
    //    }
    //    else if (t.GetSouthReverse() == dir)
    //    {
    //        PlaceAdjecentTiles(t.RotateCW().FlipVertical(), x + 1, y);
    //        //break;
    //    }
    //}

    //dir = tile.GetWest();
    //foreach (var t in UnplacedTiles())
    //{
    //    if (t.GetWest() == dir)
    //    {
    //        PlaceAdjecentTiles(t.FlipVertical(), x - 1, y);
    //        //break;
    //    }
    //    else if (t.GetWestReverse() == dir)
    //    {
    //        PlaceAdjecentTiles(t.FlipVertical().FlipHorizontal(), x - 1, y);
    //        //break;
    //    }
    //    else if (t.GetEast() == dir)
    //    {
    //        PlaceAdjecentTiles(t, x - 1, y);
    //        //break;
    //    }
    //    else if (t.GetEastReverse() == dir)
    //    {
    //        PlaceAdjecentTiles(t.FlipHorizontal(), x - 1, y);
    //        //break;
    //    }
    //    else if (t.GetNorth() == dir)
    //    {
    //        PlaceAdjecentTiles(t.RotateCW(), x - 1, y);
    //        //break;
    //    }
    //    else if (t.GetNorthReverse() == dir)
    //    {
    //        PlaceAdjecentTiles(t.RotateCW().FlipVertical(), x - 1, y);
    //        //break;
    //    }
    //    else if (t.GetSouth() == dir)
    //    {
    //        PlaceAdjecentTiles(t.RotateCCW().FlipVertical(), x - 1, y);
    //        //break;
    //    }
    //    else if (t.GetSouthReverse() == dir)
    //    {
    //        PlaceAdjecentTiles(t.RotateCCW(), x - 1, y);
    //        //break;
    //    }
    //}
}

IEnumerable<Tile> UnplacedTiles(Tile? excluded = null)
{
    for (int i = 0; i < unplacedTiles.Count; i++)
    {
        if (excluded != null && excluded == unplacedTiles[i])
            continue;
        yield return unplacedTiles[i];
    }
}

internal class Tile
{
    public Tile(int id, string[] rows)
    {
        Rows = rows;
        Cols = new string[rows[0].Length];
        for (int x = 0; x < Cols.Length; x++)
        {
            Cols[x] = new string(rows.Select(r => r[x]).ToArray());
        }
        Id = id;
    }

    public string[] Rows { get; private set; }
    public string[] Cols { get; private set; }
    public long Id { get; }

    public override string ToString() => Id.ToString();

    public string GetNorth() => Rows[0];

    public string GetNorthReverse() => new string(Rows[0].Reverse().ToArray());

    public string GetEast() => Cols[^1];

    public string GetEastReverse() => new string(Cols[^1].Reverse().ToArray());

    public string GetSouth() => Rows[^1];

    public string GetSouthReverse() => new string(Rows[^1].Reverse().ToArray());

    public string GetWest() => Cols[0];

    public string GetWestReverse() => new string(Cols[0].Reverse().ToArray());

    [System.Diagnostics.DebuggerStepThrough]
    public Tile FlipHorizontal()
    {
        var newCols = new string[Cols.Length];
        for (int i = 0; i < Cols.Length; i++)
        {
            newCols[i] = Cols[^(i + 1)];
        }
        Cols = newCols;

        var newRows = new string[Rows.Length];
        for (int i = 0; i < Rows.Length; i++)
        {
            newRows[i] = new string(Rows[i].Reverse().ToArray());
        }
        Rows = newRows;

        return this;
    }

    [System.Diagnostics.DebuggerStepThrough]
    public Tile FlipVertical()
    {
        var newCols = new string[Cols.Length];
        for (int i = 0; i < Cols.Length; i++)
        {
            newCols[i] = new string(Cols[i].Reverse().ToArray());
        }
        Cols = newCols;

        var newRows = new string[Rows.Length];
        for (int i = 0; i < Rows.Length; i++)
        {
            newRows[i] = Rows[^(i + 1)];
        }
        Rows = newRows;

        return this;
    }

    [System.Diagnostics.DebuggerStepThrough]
    public Tile RotateCW()
    {
        var newRows = new string[Rows.Length];
        for (int i = 0; i < Rows.Length; i++)
        {
            newRows[i] = new string(Cols[i].Reverse().ToArray());
        }

        var newCols = new string[Cols.Length];
        for (int i = Cols.Length - 1; i >= 0; i--)
        {
            newCols[i] = Rows[Rows.Length - i - 1];
        }

        Rows = newRows;
        Cols = newCols;

        return this;
    }

    [System.Diagnostics.DebuggerStepThrough]
    public Tile RotateCCW()
    {
        var newRows = new string[Rows.Length];
        for (int i = Rows.Length - 1; i >= 0; i--)
        {
            newRows[i] = Cols[Cols.Length - i - 1];
        }

        var newCols = new string[Cols.Length];
        for (int i = 0; i < Cols.Length; i++)
        {
            newCols[i] = new string(Rows[i].Reverse().ToArray());
        }

        Rows = newRows;
        Cols = newCols;

        return this;
    }

    public void Print()
    {
        foreach (var r in Rows)
        {
            Console.WriteLine(r);
        }
    }
}
