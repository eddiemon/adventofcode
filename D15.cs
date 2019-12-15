using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace aoc2019
{
    public class D15
    {
        const int North = 1;
        const int South = 2;
        const int West = 3;
        const int East = 4;

        private Dictionary<int, Vector2> HeadingToVector = new Dictionary<int, Vector2>
        {
            { North, (0, -1) },
            { South, (0, 1) },
            { West, (-1, 0) },
            { East, (1, 0) },
        };

        const int Wall = 0;
        const int Empty = 1;
        const int OxygenTank = 2;
        const int Droid = 3;

        Dictionary<(int x, int y), int> room = new Dictionary<(int, int), int>();
        IntCodeSync computer;

        public string Answer()
        {
            var computerMemory = File.ReadAllText("d15.txt").Split(',').Select(x => BigInteger.Parse(x)).ToArray();
            computer = new IntCodeSync(computerMemory);

            BreadthSearchFrom((0, 0));

            Console.ForegroundColor = ConsoleColor.White;
            PrintRoom(room);

            var grid = ConvertToGrid(room);
            var minX = room.Min(kvp => kvp.Key.x);
            var minY = room.Min(kvp => kvp.Key.y);
            var maxY = room.Max(kvp => kvp.Key.y);

            var origin = new Position(-minX, -minY);
            var oxygenTank = new Position(oxygenPos.x - minX, oxygenPos.y - minY);
            var path = grid.GetPath(origin, oxygenTank, MovementPatterns.LateralOnly);

            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var p in path)
            {
                Console.SetCursorPosition(p.X, p.Y);
                if (p == origin) Console.Write('X');
                else if (p == oxygenTank) { Console.ForegroundColor = ConsoleColor.Cyan; Console.Write('T'); }
                else Console.Write('.');
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, maxY - minY + 1);

            return (path.Length - 1).ToString(); // Remove starting point
        }

        Vector2 droidPos;
        Vector2 oxygenPos;

        public void BreadthSearchFrom(Vector2 pos)
        {
            var unvisitedPositions = new List<(Vector2 pos, int heading)>();
            for (int heading = North; heading < East + 1; heading++)
            {
                var newPos = pos + HeadingToVector[heading];
                if (!room.ContainsKey(newPos)) unvisitedPositions.Add((newPos, heading));
            }

            foreach (var position in unvisitedPositions)
            {
                var computerSnapshot = computer.CreateSnapshot();

                computer.Run(position.heading);
                var status = computer.Run();

                room[position.pos] = (int)status;

                if (status == OxygenTank)
                    oxygenPos = position.pos;

                if (status != Wall)
                {
                    droidPos = position.pos;
                    BreadthSearchFrom(position.pos);
                }

                computer.RestoreFromSnapshot(computerSnapshot);
            }
        }

        private Grid ConvertToGrid(Dictionary<(int x, int y), int> room)
        {
            var minX = room.Min(kvp => kvp.Key.x);
            var maxX = room.Max(kvp => kvp.Key.x);
            var minY = room.Min(kvp => kvp.Key.y);
            var maxY = room.Max(kvp => kvp.Key.y);

            var grid = new Grid(maxX - minX + 1, maxY - minY + 1);

            for (int y = minY; y < maxY + 1; y++)
            {
                for (int x = minX; x < maxX + 1; x++)
                {
                    if (room.TryGetValue((x, y), out var t))
                    {
                        if (t == Wall) grid.BlockCell(new Position(x - minX, y - minY));
                    }
                    else
                        grid.BlockCell(new Position(x - minX, y - minY));
                }
            }
            return grid;
        }

        int prevMinX = 0;
        int prevMinY = 0;
        private void PrintRoom(Dictionary<(int x, int y), int> room)
        {
            var minX = room.Min(kvp => kvp.Key.x);
            var minY = room.Min(kvp => kvp.Key.y);

            if (prevMinX != minX || prevMinY != minY)
            {
                Console.Clear();
                prevMinX = minX;
                prevMinY = minY;
            }

            foreach (var p in room.OrderBy(p => p.Key.y).ThenBy(p => p.Key.x))
            {
                var (x, y) = p.Key;
                var t = p.Value;

                var color = Console.ForegroundColor;
                Console.SetCursorPosition(x - minX, y - minY);
                if (t == Wall) { Console.ForegroundColor = ConsoleColor.Red; Console.Write('#'); }
                else if (p.Key == droidPos) Console.Write('O');
                else if (t == OxygenTank) { Console.ForegroundColor = ConsoleColor.Blue; Console.Write('T'); }
                else if (t == Empty) Console.Write('.');

                Console.ForegroundColor = color;
            }

            Console.WriteLine();
        }
    }
}
