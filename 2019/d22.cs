using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace aoc
{
    internal class D22
    {
        internal object Answer()
        {
            var input = File.ReadAllLines("22.in");

            BigInteger cards = 119315717514047;
            BigInteger shuffles = 101741582076661;
            BigInteger offsetDiff = 0;
            BigInteger incrementMultiplier = 1;

            foreach (var l in input)
            {
                if (l.StartsWith("deal with"))
                {
                    var inc = BigInteger.Parse(l.Substring("deal with increment ".Length));
                    incrementMultiplier *= Maths.ModularMultiplicativeInverse(inc, cards);
                    incrementMultiplier %= cards;
                }
                else if (l.StartsWith("cut"))
                {
                    var c = BigInteger.Parse(l.Substring("cut ".Length));
                    offsetDiff += c * incrementMultiplier;
                    offsetDiff %= cards;
                }
                else
                {
                    incrementMultiplier *= -1;
                    incrementMultiplier %= cards;
                    offsetDiff += incrementMultiplier;
                    offsetDiff %= cards;
                }
            }

            var increment = Maths.ModularExponentiation(incrementMultiplier, shuffles, cards);
            var offset = offsetDiff * (1 - increment) * Maths.ModularMultiplicativeInverse((1 - incrementMultiplier) % cards, cards);
            offset %= cards;

            return (offset + 2020 * increment) % cards;
        }
    }
}
