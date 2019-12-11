﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace aoc2019
{
    public class D11
    {
        public static Vector Up = new Vector(0, -1);
        public static Vector Down = new Vector(0, 1);
        public static Vector Left = new Vector(-1, 0);
        public static Vector Right = new Vector(1, 0);

        public string Answer()
        {
            var computerMemory = File.ReadAllText("d11.txt").Split(',').Select(x => BigInteger.Parse(x)).ToArray();

            var computer = new IntCode(computerMemory);
            var halted = false;
            computer.Halted += () => halted = true;
            var stdOut = computer.StdOut;
            var stdIn = computer.StdIn;

            Task.Run(() => computer.Run());

            var colorByPosition = new Dictionary<Vector, bool>();
            var robotDirection = new Vector(Up);
            var robot = new Vector();

            colorByPosition.Add(robot, true);
            while (!halted)
            {
                var isWhite = colorByPosition.GetValueOrDefault(robot);
                stdIn.Enqueue(isWhite ? 1 : 0);
                //computer.InputGiven.Set();
                //Console.WriteLine("Input given");

                while (stdOut.Count < 2) Thread.Sleep(1);
                stdOut.TryDequeue(out var newColorIsWhite);
                //Console.WriteLine("Output received");

                colorByPosition[robot] = newColorIsWhite == 1;

                stdOut.TryDequeue(out var direction);
                if (robotDirection == Up) robotDirection = direction == 0 ? Left : Right;
                else if (robotDirection == Left) robotDirection = direction == 0 ? Down : Up;
                else if (robotDirection == Down) robotDirection = direction == 0 ? Right : Left;
                else if (robotDirection == Right) robotDirection = direction == 0 ? Up : Down;

                robot += robotDirection;
            }

            var xMin = colorByPosition.Keys.Min(x => x.x);
            var xMax = colorByPosition.Keys.Max(x => x.x);
            var yMin = colorByPosition.Keys.Min(x => x.y);
            var yMax = colorByPosition.Keys.Max(x => x.y);

            for (int y = 0; y <= yMax; y++)
            {
                for (int x = xMin; x <= xMax; x++)
                {
                    if (colorByPosition.GetValueOrDefault(new Vector(x, y))) Console.Write("#");
                    else Console.Write(".");
                }
                Console.WriteLine();
            }

            return colorByPosition.Count.ToString();
        }
    }
}
