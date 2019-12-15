using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace aoc
{
    public class D11
    {
        public static Vector2 Up = new Vector2(0, -1);
        public static Vector2 Down = new Vector2(0, 1);
        public static Vector2 Left = new Vector2(-1, 0);
        public static Vector2 Right = new Vector2(1, 0);

        public string Answer()
        {
            var computerMemory = File.ReadAllText("d11.txt").Split(',').Select(x => BigInteger.Parse(x)).ToArray();

            var computer = new IntCode(computerMemory);
            var halted = false;
            computer.Halted += () => halted = true;
            var stdOut = computer.StdOut;
            var stdIn = computer.StdIn;

            Task.Run(() => computer.Run());

            var colorByPosition = new Dictionary<Vector2, bool>();
            var robotDirection = new Vector2(Up);
            var robot = new Vector2();

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
                    if (colorByPosition.GetValueOrDefault(new Vector2(x, y))) Console.Write("#");
                    else Console.Write(".");
                }
                Console.WriteLine();
            }

            return colorByPosition.Count.ToString();
        }
    }
}
