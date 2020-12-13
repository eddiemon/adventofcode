using System;
using System.Numerics;

public static class Maths
{
    public static int gcd(int a, int b) => a == 0 ? b : gcd(b % a, a);

    public static int lcm(int a, int b) => System.Math.Abs(a * b) / gcd(a, b);

    public static long LongBinarySearch(long lo, long hi, Func<long, bool> predicate)
    {
        while (hi - lo > 1)
        {
            var mid = lo + (hi - lo) / 2;
            if (predicate(mid)) hi = mid;
            else lo = mid;
        }
        return lo;
    }

    public static int BinarySearch(int lo, int hi, Func<int, bool> predicate)
    {
        while (hi - lo > 1)
        {
            var mid = lo + (hi - lo) / 2;
            if (predicate(mid)) hi = mid;
            else lo = mid;
        }
        return lo;
    }

    public static BigInteger ModularMultiplicativeInverse(BigInteger n, BigInteger mod)
    {
        return ModularExponentiation(n, mod - 2, mod);
    }

    public static BigInteger ModularExponentiation(BigInteger @base, BigInteger exp, BigInteger mod)
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
