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
        private Dictionary<int, Queue<BigInteger>> Q;
        private BigInteger NatX;
        private BigInteger NatY;
        private BigInteger lastY = int.MinValue;

        public object Answer()
        {
            var code = File.ReadAllText("23.in").Split(',').Select(s => BigInteger.Parse(s)).ToArray();

            var computers = new Dictionary<int, IntCodeSync>();
            Q = new Dictionary<int, Queue<BigInteger>>();
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
                    var dest = computer.Run();
                    if (dest == null)
                    {
                        if (q.Count == 0)
                        {
                            computer.Run(-1);
                        }
                        else
                        {
                            DeliverMessages(computer, q);
                        }

                        dest = computer.Run();
                    }

                    ReceiveMessages(dest, computer);
                }

                if (Q.Values.All(q => q.Count == 0))
                {
                    if (lastY == NatY)
                    {
                        Console.WriteLine(NatY);
                        throw new Exception();
                    }
                    Q[0].Enqueue(NatX);
                    Q[0].Enqueue(NatY);
                    lastY = NatY;
                }
            }

            return "error";
        }

        private void ReceiveMessages(BigInteger? dest, IntCodeSync computer)
        {
            while (dest != null)
            {
                if (dest == 255)
                {
                    NatX = computer.Run().Value;
                    NatY = computer.Run().Value;
                }
                else
                {
                    var x = computer.Run().Value;
                    var y = computer.Run().Value;
                    Q[(int)dest].Enqueue(x);
                    Q[(int)dest].Enqueue(y);
                }

                dest = computer.Run();
            }
        }

        private static void DeliverMessages(IntCodeSync computer, Queue<BigInteger> q)
        {
            computer.Run(q);
            //while (q.Count > 0)
            //{
            //    var snapshot = computer.CreateSnapshot();
            //    for (int i = 0; i < 2; i++)
            //    {
            //        var input = q.Peek();
            //        var res = computer.Run(input);
            //        if (res != null)
            //        {
            //            computer.RestoreFromSnapshot(snapshot);
            //            break;
            //        }
            //        q.Dequeue();
            //    }
            //}
        }
    }
}
