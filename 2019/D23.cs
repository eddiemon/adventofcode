using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace aoc
{
    public class D23
    {
        public object Answer()
        {
            var code = File.ReadAllText("23.in").Split(',').Select(s => BigInteger.Parse(s)).ToArray();

            var computers = new Dictionary<int, IntCodeSync>();
            var Q = new Dictionary<int, Queue<BigInteger>>();
            var NAT = new Stack<BigInteger>();
            for (int i = 0; i < 50; i++)
            {
                var computer = new IntCodeSync(code.ToArray());
                computers.Add(i, computer);
                computer.Run(i);
                Q[i] = new Queue<BigInteger>();
            }

            while (true)
            {
                foreach (var computerAndId in computers)
                {
                    var computer = computerAndId.Value;
                    var q = Q[computerAndId.Key];
                    var output = computer.Run();
                    if (output == null)
                    {
                        if (q.Count == 0)
                        {
                            computer.Run(-1);
                            continue;
                        }

                        while (q.Count > 0)
                        {
                            var snapshot = computer.CreateSnapshot();
                            for (int i = 0; i < 2; i++)
                            {
                                var input = q.Peek();
                                var res = computer.Run(input);
                                if (res != null)
                                {
                                    computer.RestoreFromSnapshot(snapshot);
                                    break;
                                }
                                q.Dequeue();
                            }
                        }
                    }
                    else
                    {
                        var dest = output;
                        while (dest != null)
                        {
                            if (dest == 255)
                            {
                                computer.Run();
                                return computer.Run();
                            }
                            var x = computer.Run().Value;
                            var y = computer.Run().Value;
                            Q[(int)dest].Enqueue(x);
                            Q[(int)dest].Enqueue(y);

                            dest = computer.Run();
                        }
                    }
                }
            }

            return "error";
        }
    }
}
