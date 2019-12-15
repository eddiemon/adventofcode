using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace aoc
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
        const int Oxygen = 2;

        int width, height;
        int[] grid;

        public object Answer()
        {
            var input = File.ReadAllLines("d15.2.txt");
            width = int.Parse(input[0]);
            height = int.Parse(input[1]);
            oxygenPos = (int.Parse(input[2]), int.Parse(input[3]));
            grid = new int[height * width];
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    grid[row * width + col] = input[4 + row][col] == '#' ? Wall : Empty;
                }
            }
            grid[oxygenPos.y * width + oxygenPos.x] = Oxygen;
            PrintGrid();

            BreadthSearchFrom(oxygenPos);


            return maxSteps;
        }

        Vector2 oxygenPos;
        int maxSteps = 0;

        public void BreadthSearchFrom(Vector2 pos, int steps = 0)
        {
            var unvisitedPositions = new List<Vector2>();
            for (int heading = North; heading < East + 1; heading++)
            {
                var newPos = pos + HeadingToVector[heading];
                if (newPos.x < 0 || newPos.x > width || newPos.y < 0 || newPos.y > height) continue;
                if (grid[newPos.y * width + newPos.x] == Wall) continue;
                if (grid[newPos.y * width + newPos.x] == Oxygen) continue;

                unvisitedPositions.Add(newPos);
            }

            foreach (var position in unvisitedPositions)
            {
                grid[position.y * width + position.x] = Oxygen;
            }

            //PrintGrid();
            //Thread.Sleep(100);

            foreach (var position in unvisitedPositions)
            {
                BreadthSearchFrom(position, steps + 1);
            }
            if (steps > maxSteps) maxSteps = steps;
        }

        void PrintGrid()
        {
            Console.Clear();

            var color = Console.ForegroundColor;
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    var t = grid[row * width + col];
                    if (t == Wall) { Console.ForegroundColor = ConsoleColor.Red; Console.Write('#'); }
                    else if (t == Empty) { Console.ForegroundColor = ConsoleColor.White; Console.Write('.'); }
                    else if (t == Oxygen) { Console.ForegroundColor = ConsoleColor.Cyan; Console.Write('O'); }
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = color;
        }
    }
}
