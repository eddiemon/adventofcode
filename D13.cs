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
        public string Answer()
        {
            var computerMemory = File.ReadAllText("d13.txt").Split(',').Select(x => BigInteger.Parse(x)).ToArray();

            var computer = new IntCode(computerMemory);
            var stdOut = computer.StdOut;

            computer.Run();

            var instructions = new List<(BigInteger x, BigInteger y, BigInteger t)>();
            while (stdOut.Count > 0) {
                stdOut.TryDequeue(out var x);
                stdOut.TryDequeue(out var y);
                stdOut.TryDequeue(out var t);
                instructions.Add((x, y, t));
            }

            return instructions.Where(i => i.t == 2).Count().ToString();
        }
    }
}
