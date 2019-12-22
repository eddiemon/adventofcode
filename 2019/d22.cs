using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace aoc
{
    class D22
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
                    incrementMultiplier *= inv(inc, cards);
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

            var increment = ModPow(incrementMultiplier, shuffles, cards);
            var offset = offsetDiff * (1 - increment) * inv((1 - incrementMultiplier) % cards, cards);
            offset %= cards;

            return (offset + 2020 * increment) % cards;
        }

        BigInteger inv(BigInteger n, BigInteger mod)
        {
            return ModPow(n, mod - 2, mod);
        }

        BigInteger ModPow(BigInteger @base, BigInteger exp, BigInteger mod)
        {
            if (mod == 1) return 0;

            BigInteger res = 1;
            @base %= mod;
            while (exp > 0)
            {
                if (exp % 2 == 1)
                {
                    res *= @base;
                    res %= mod;
                }

                exp >>= 1;
                @base *= @base;
                @base %= mod;
            }
            return res;
        }
    }
}
