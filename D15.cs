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
        const int Wall = 0;
        const int Empty = 1;
        const int OxygenTank = 2;
        const int Droid = 3;

        Dictionary<(int x, int y), int> room = new Dictionary<(int, int), int>();

        public string Answer()
        {
            var computerMemory = File.ReadAllText("d15.txt").Split(',').Select(x => BigInteger.Parse(x)).ToArray();

            var computer = new IntCodeSync(computerMemory);

            var droidPos = (x: 0, y: 0);
            (int x, int y)? o2TankPos = null;
            int moveDir = North;
            try
            {
                while (true)
                {
                    var newPos = MoveInDirection(droidPos, moveDir);

                    computer.Run(moveDir);
                    var status = computer.Run(moveDir);

                    if (status == Wall) // Hit wall
                    {
                        room[newPos] = Wall;
                    }
                    else if (status == Empty) // Moved droid
                    {
                        room[newPos] = Droid;
                        room[droidPos] = Empty;
                        droidPos = newPos;
                    }
                    else if (status == OxygenTank) // Found oxygen tank
                    {
                        o2TankPos = newPos;
                        break;
                    }

                    //PrintRoom(room);
                    //Thread.Sleep(10);

                    moveDir = FindNextDirection(droidPos, moveDir);
                }
            }
            catch (ComputerHaltedException)
            {

            }


            Console.ForegroundColor = ConsoleColor.White;
            PrintRoom(room);

            var grid = ConvertToGrid(room);
            var minX = room.Min(kvp => kvp.Key.x);
            var minY = room.Min(kvp => kvp.Key.y);

            var origin = new Position(-minX, -minY);
            var droid = new Position(droidPos.x - minX, droidPos.y - minY);
            var path = grid.GetPath(origin, droid, MovementPatterns.LateralOnly);

            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var p in path)
            {
                Console.SetCursorPosition(p.X, p.Y);
                if (p == origin) Console.Write('X');
                else if (p == droid) Console.Write('O');
                else Console.Write('.');
            }

            return path.Length.ToString();
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

        Random r = new Random();

        int FindNextDirection((int x, int y) pos, int moveDir)
        {
            //var newPos = MoveInDirection(pos, moveDir);
            //if (!room.ContainsKey(newPos)) return moveDir;

            //if (room[newPos] == Wall)
            //{
            //    // Check all positions around it
            //    for (int xx = -1; xx < 2; xx += 2)
            //    {
            //        var testPos = (pos.x + xx, pos.y);
            //        if (room.ContainsKey(testPos) && room[testPos] == Wall) continue;
            //        if (xx == -1) return West;
            //        else return East;
            //    }

            //    for (int yy = -1; yy < 2; yy += 2)
            //    {
            //        var testPos = (pos.x, pos.y + yy);
            //        if (room.ContainsKey(testPos) && room[testPos] == Wall) continue;
            //        if (yy == -1) return North;
            //        else return South;
            //    }

            //    // Check all positions further ahead for another wall
            //    while (true)
            //    {
            //        var nnPos = MoveInDirection(newPos, moveDir);
            //        if (!room.ContainsKey(nnPos)) return moveDir;


            //    }
            //}
            moveDir = 1 + r.Next(0, 4);
            var nextPos = MoveInDirection(pos, moveDir);
            while (room.ContainsKey(nextPos) && room[nextPos] == Wall)
            {
                moveDir = 1 + r.Next(0, 4);
                nextPos = MoveInDirection(pos, moveDir);
            }

            return moveDir;
        }

        private (int x, int y) MoveInDirection((int x, int y) pos, int dir)
        {
            if (dir == North)
                return (pos.x, pos.y - 1);
            else if (dir == South)
                return (pos.x, pos.y + 1);
            else if (dir == West)
                return (pos.x - 1, pos.y);
            else if (dir == East)
                return (pos.x + 1, pos.y);
            throw new Exception();
        }


        private void PrintRoom(Dictionary<(int x, int y), int> room)
        {
            var minX = room.Min(kvp => kvp.Key.x);
            //var maxX = room.Max(kvp => kvp.Key.x);
            var minY = room.Min(kvp => kvp.Key.y);
            //var maxY = room.Max(kvp => kvp.Key.y);

            Console.Clear();

            foreach (var p in room.OrderBy(p => p.Key.y).ThenBy(p => p.Key.x))
            {
                var (x, y) = p.Key;
                var t = p.Value;

                Console.SetCursorPosition(x - minX, y - minY);
                if (t == Wall) Console.Write('#'); // Wall
                else if (t == Droid) Console.Write('O'); // Droid
                else if (t == OxygenTank) Console.Write('T'); // Oxygen tank
                else if (t == Empty) Console.Write('.'); // Visited
            }

            Console.WriteLine();
        }
    }
}
