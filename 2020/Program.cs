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
var tiles = new List<Tile>();
foreach (var l in input)
{
    if (string.IsNullOrEmpty(l))
    {
        tiles.Add(new Tile(tileId, tileBuffer.ToArray()));
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

Tile topLeftCorner = null;
foreach (var tile in tiles)
{
    var adjecentTiles = FindAdjecentTiles(tile);
    var adjTiles = new[] { adjecentTiles.North, adjecentTiles.East, adjecentTiles.South, adjecentTiles.West };

    topLeftCorner = adjecentTiles switch
    {
        (Tile, Tile, null, null) => tile.FlipHorizontal(),
        (Tile, null, null, Tile) => tile.FlipVertical().FlipHorizontal(),
        (null, Tile, Tile, null) => tile,
        (null, null, Tile, Tile) => tile.FlipVertical(),
        _ => null
    };

    if (topLeftCorner != null)
        break;
}

//var temp = FindAdjecentTiles(topLeftCorner);
//topLeftCorner.Print();

//topLeftCorner.Print();

var borderImageLength = tiles[0].Rows.Length;
var borderLessImageLength = (borderImageLength - 2);
var fullImageLength = borderLessImageLength * ((int)Math.Sqrt(tiles.Count));
var fullImage = new char[fullImageLength * fullImageLength];

var edgeTile = topLeftCorner;
var colTile = topLeftCorner;
for (int row = 0; row < fullImageLength; row += borderLessImageLength)
{
    for (int col = 0; col < fullImageLength; col += borderLessImageLength)
    {
        for (int y = 1; y < borderImageLength - 1; y++)
        {
            for (int x = 1; x < borderImageLength - 1; x++)
            {
                fullImage[(row + y - 1) * fullImageLength + (col + x - 1)] = colTile.Rows[y][x];
            }
        }
        var adjs = FindAdjecentTiles(colTile);
        colTile = adjs.East;
    }
    var adj = FindAdjecentTiles(edgeTile);
    edgeTile = colTile = adj.South;
}

var fullImageArray = new string[fullImageLength];

for (int y = 0; y < fullImageLength; y++)
{
    var arr = fullImage.Skip(fullImageLength * y).Take(fullImageLength).ToArray();
    fullImageArray[y] = new string(arr);
}

var fullImageTile = new Tile(0, fullImageArray);
//fullImageTile.Print();

var seamonster = new string[] {
    "                  # ",
    "#    ##    ##    ###",
    " #  #  #  #  #  #   "
};

var noOfSeamonster = 0;
for (int i = 0; i < 12; i++)
{
    for (int row = 0; row < fullImageTile.Rows.Length - seamonster.Length; row++)
    {
        for (int col = 0; col < fullImageTile.Cols.Length - seamonster[0].Length; col++)
        {
            var foundSeamonster = true;
            for (int sy = 0; sy < seamonster.Length; sy++)
            {
                for (int sx = 0; sx < seamonster[0].Length; sx++)
                {
                    if (seamonster[sy][sx] == '#' && fullImageTile.Rows[row + sy][col + sx] != '#')
                    {
                        foundSeamonster = false;
                        break;
                    }
                }
                if (!foundSeamonster)
                    break;
            }

            if (foundSeamonster)
                noOfSeamonster++;
        }
    }

    if (noOfSeamonster > 0)
        break;

    if (i == 4) fullImageTile = fullImageTile.FlipHorizontal();
    else if (i == 8) fullImageTile = fullImageTile.FlipVertical();
    else if (i % 4 == 0) fullImageTile = fullImageTile.RotateCW();
    else if (i % 4 == 1) fullImageTile = fullImageTile.RotateCW();
    else if (i % 4 == 2) fullImageTile = fullImageTile.RotateCW();
    else if (i % 4 == 3) fullImageTile = fullImageTile.RotateCW();
    //else if (i == 4) fullImageTile = fullImageTile.FlipVertical();
}

Console.WriteLine(noOfSeamonster);

var seamonsterHashes = noOfSeamonster * 15;
int hashes = 0;
foreach (var l in fullImageTile.Rows)
{
    for (int i = 0; i < l.Length; i++)
    {
        if (l[i] == '#') hashes++;
    }
}

Console.WriteLine(hashes - seamonsterHashes);

(Tile North, Tile East, Tile South, Tile West) FindAdjecentTiles(Tile tile)
{
    Tile north = null, east = null, south = null, west = null;

    var dir = tile.GetNorth();
    foreach (var t in GetTilesExcept(tile))
    {
        if (t.GetWest() == dir)
            north = t.RotateCCW();
        else if (t.GetWestReverse() == dir)
            north = t.RotateCCW().FlipHorizontal();
        else if (t.GetEast() == dir)
            north = t.RotateCW().FlipVertical();
        else if (t.GetEastReverse() == dir)
            north = t.RotateCW();
        else if (t.GetNorth() == dir)
            north = t.FlipHorizontal();
        else if (t.GetNorthReverse() == dir)
            north = t.FlipHorizontal().FlipVertical();
        else if (t.GetSouth() == dir)
            north = t;
        else if (t.GetSouthReverse() == dir)
            north = t.FlipVertical();

        if (north != null)
            break;
    }

    dir = tile.GetSouth();
    foreach (var t in GetTilesExcept(tile))
    {
        if (t.GetWest() == dir)
            south = t.RotateCW().FlipVertical();
        else if (t.GetWestReverse() == dir)
            south = t.RotateCW();
        else if (t.GetEast() == dir)
            south = t.RotateCCW();
        else if (t.GetEastReverse() == dir)
            south = t.RotateCCW().FlipVertical();
        else if (t.GetNorth() == dir)
            south = t;
        else if (t.GetNorthReverse() == dir)
            south = t.FlipVertical();
        else if (t.GetSouth() == dir)
            south = t.FlipHorizontal();
        else if (t.GetSouthReverse() == dir)
            south = t.FlipVertical().FlipHorizontal();

        if (south != null)
            break;
    }

    dir = tile.GetEast();
    foreach (var t in GetTilesExcept(tile))
    {
        if (t.GetWest() == dir)
            east = t;
        else if (t.GetWestReverse() == dir)
            east = t.FlipHorizontal();
        else if (t.GetEast() == dir)
            east = t.FlipVertical();
        else if (t.GetEastReverse() == dir)
            east = t.FlipVertical().FlipHorizontal();
        else if (t.GetNorth() == dir)
            east = t.RotateCCW().FlipHorizontal();
        else if (t.GetNorthReverse() == dir)
            east = t.RotateCCW();
        else if (t.GetSouth() == dir)
            east = t.RotateCW();
        else if (t.GetSouthReverse() == dir)
            east = t.RotateCW().FlipHorizontal();

        if (east != null)
        {
            System.Diagnostics.Debug.Assert(dir == east.GetWest());
            break;
        }
    }

    dir = tile.GetWest();
    foreach (var t in GetTilesExcept(tile))
    {
        if (t.GetWest() == dir)
            west = t.FlipVertical();
        else if (t.GetWestReverse() == dir)
            west = t.FlipVertical().FlipHorizontal();
        else if (t.GetEast() == dir)
            west = t;
        else if (t.GetEastReverse() == dir)
            west = t.FlipHorizontal();
        else if (t.GetNorth() == dir)
            west = t.RotateCW();
        else if (t.GetNorthReverse() == dir)
            west = t.RotateCW().FlipVertical();
        else if (t.GetSouth() == dir)
            west = t.RotateCCW().FlipVertical();
        else if (t.GetSouthReverse() == dir)
            west = t.RotateCCW();

        if (west != null)
            break;
    }

    return (north, east, south, west);
}

IEnumerable<Tile> GetTilesExcept(Tile excluded)
{
    for (int i = 0; i < tiles.Count; i++)
    {
        if (excluded.Id == tiles[i].Id)
            continue;
        yield return tiles[i];
    }
}

internal class Tile
{
    public Tile(long id, string[] rows)
    {
        Rows = rows;
        Cols = new string[rows[0].Length];
        for (int x = 0; x < Cols.Length; x++)
        {
            Cols[x] = new string(rows.Select(r => r[x]).ToArray());
        }
        Id = id;
    }

    public string[] Rows { get; }
    public string[] Cols { get; }
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
        var newRows = new string[Rows.Length];
        for (int i = 0; i < Rows.Length; i++)
        {
            newRows[i] = Rows[^(i + 1)];
        }

        return new Tile(Id, newRows);
    }

    [System.Diagnostics.DebuggerStepThrough]
    public Tile FlipVertical()
    {
        var newRows = new string[Rows.Length];
        for (int i = 0; i < Rows.Length; i++)
        {
            newRows[i] = new string(Rows[i].Reverse().ToArray());
        }

        return new Tile(Id, newRows);
    }

    [System.Diagnostics.DebuggerStepThrough]
    public Tile RotateCW()
    {
        var newRows = new string[Rows.Length];
        for (int i = 0; i < Rows.Length; i++)
        {
            newRows[i] = new string(Cols[i].Reverse().ToArray());
        }

        return new Tile(Id, newRows);
    }

    [System.Diagnostics.DebuggerStepThrough]
    public Tile RotateCCW()
    {
        var newRows = new string[Rows.Length];
        for (int i = Rows.Length - 1; i >= 0; i--)
        {
            newRows[i] = Cols[Cols.Length - i - 1];
        }

        return new Tile(Id, newRows);
    }

    public void Print()
    {
        foreach (var r in Rows)
        {
            Console.WriteLine(r);
        }
    }
}
