using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace aoc
{
    public class D17
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

        public object Answer()
        {
            var code = File.ReadAllText("17.in").Split(',').Select(s => BigInteger.Parse(s)).ToArray();
            code[0] = 2;

            var computer = new IntCodeSync(code);

            int rows = 1;
            while (rows < 36)
            {
                var val = (char)computer.Run().Value;
                Console.Write(val);
                if (val == '\n') rows++;
            }
            try
            {
                // hand-made
                var main = "A,A,B,C,B,C,B,C,C,A\n";
                var a = "R,8,L,4,R,4,R,10,R,8\n";
                var b = "L,12,L,12,R,8,R,8\n";
                var c = "R,10,R,4,R,4\n";
                foreach (var ch in main.ToCharArray())
                {
                    var res = computer.Run(ch);
                }
                var o = computer.Run();
                while (o != null)
                {
                    Console.Write((char)o);
                    o = computer.Run();
                }
                foreach (var ch in a.ToCharArray())
                {
                    computer.Run(ch);
                }
                o = computer.Run();
                while (o != null)
                {
                    Console.Write((char)o);
                    o = computer.Run();
                }
                foreach (var ch in b.ToCharArray())
                {
                    computer.Run(ch);
                }
                o = computer.Run();
                while (o != null)
                {
                    Console.Write((char)o);
                    o = computer.Run();
                }
                o = computer.Run();
                while (o != null)
                {
                    Console.Write((char)o);
                    o = computer.Run();
                }
                foreach (var ch in c.ToCharArray())
                {
                    computer.Run(ch);
                }
                o = computer.Run();
                while (o != null)
                {
                    Console.Write((char)o);
                    o = computer.Run();
                }
                computer.Run('n');
                computer.Run('\n');

                Console.Clear();
                Console.SetCursorPosition(0, 0);

                computer.Run();

                rows = 1;
                while (true)
                {
                    var val = (long?)computer.Run().GetValueOrDefault(0);
                    if (val > 0xff) return val;

                    if (val == '\n')
                        rows++;

                    Console.Write((char)val);

                    if (rows % 34 == 0)
                    {
                        Console.ReadLine();
                        Console.Clear();
                        Console.SetCursorPosition(0, 0);
                        val = (int?)computer.Run();
                        if (val > 0xFF) return val;
                        rows = 1;
                    }
                }
            }
            catch (ComputerHaltedException)
            {

            }

            return "error";
        }
    }
}
