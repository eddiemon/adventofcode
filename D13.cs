using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace aoc2019
{
    public class D13
    {
        const int Left = -1;
        const int Right = 1;
        const int Neutral = 0;

        public string Answer()
        {
            var computerMemory = File.ReadAllText("d13.txt").Split(',').Select(x => BigInteger.Parse(x)).ToArray();
            computerMemory[0] = 2; // Free plays!

            var computer = new IntCodeSync(computerMemory);

            Thread.Sleep(100);

            int score = -1;
            var game = new Dictionary<(int, int), int>();
            var lastBall = (x: 0, y: 0);
            var lastPaddle = (x: 0, y: 0);

            try
            {
                while (true)
                {
                    int ballX = -1;
                    int paddleX = -1;

                    while (true)
                    {
                        var x = computer.Run();
                        if (x == null) break;

                        var y = computer.Run();
                        var t = computer.Run();

                        if (x == -1)
                        {
                            score = (int)t;
                            continue;
                        }
                        else if (t == 4)
                        {
                            ballX = (int)x;
                            game[(lastBall.x, lastBall.y)] = 0;
                            lastBall = ((int)x, (int)y);
                        }
                        else if (t == 3)
                        {
                            paddleX = (int)x;
                            game[(lastPaddle.x, lastPaddle.y)] = 0;
                            lastPaddle = ((int)x, (int)y);
                        }
                        game[((int)x, (int)y)] = (int)t;
                    }

                    //PrintGame(game);

                    int joystick = Neutral;
                    if (paddleX - ballX > 0) joystick = Left;
                    else if (paddleX - ballX < 0) joystick = Right;

                    computer.Run(joystick);
                }
            }
            catch (ComputerHaltedException)
            {

            }


            return score.ToString();
        }

        private void PrintGame(Dictionary<(int, int), int> game)
        {
            foreach (var p in game)
            {
                var (x, y) = p.Key;
                var t = p.Value;

                Console.SetCursorPosition(x, y);
                if (t == 0) Console.Write(' ');
                else if (t == 1) Console.Write('#');
                else if (t == 2) Console.Write('U');
                else if (t == 3) Console.Write('^');
                else if (t == 4) Console.Write('O');
            }
        }
    }
}
