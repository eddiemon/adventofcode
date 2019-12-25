using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace aoc
{
    public class D21
    {
        private IntCodeSync computer;

        public object Answer()
        {
            var code = File.ReadAllText("21.in").Split(',').Select(s => BigInteger.Parse(s)).ToArray();

            while (true)
            {
                computer = new IntCodeSync(code.ToArray());
                try
                {
                    while (true)
                    {
                        ReadPrompt();

                        var instructions = new System.Text.StringBuilder();
                        var input = Console.ReadLine().ToUpper();
                        while (!input.Equals("RUN") && !input.Equals(""))
                        {
                            instructions.Append(input);
                            instructions.Append('\n');
                            input = Console.ReadLine().ToUpper();
                        }
                        instructions.Append("RUN");
                        instructions.Append('\n');

                        computer.Run(new Queue<BigInteger>(instructions.ToString().Select(c => (BigInteger)c)));
                    }
                }
                catch (ComputerHaltedException)
                {

                }
            }

            return "error";
        }

        private void ReadPrompt()
        {
            var o = computer.Run();
            while (o is BigInteger c && c < 256)
            {
                Console.Write((char)c);
                o = computer.Run();
            }
            if (o > 255)
            {
                Console.WriteLine("Answer: " + o);
                throw new Exception();
            }
        }
    }
}
