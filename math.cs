using System;

namespace aoc2019 {
    public static class Maths {
        
        public static int gcd(int a, int b) => a == 0 ? b : gcd(b % a, a);
        public static int lcm(int a, int b) => System.Math.Abs(a * b) / gcd(a, b);

        public static long LongBinarySearch(long lo, long hi, Func<long, bool> predicate)
        {
            while (hi - lo > 1)
            {
                var val = lo + (hi - lo) / 2;
                if (predicate(val)) hi = val;
                else lo = val;
            }
            return lo;
        }

        public static int BinarySearch(int lo, int hi, Func<int, bool> predicate)
        {
            while (hi - lo > 1)
            {
                var val = lo + (hi - lo) / 2;
                if (predicate(val)) hi = val;
                else lo = val;
            }
            return lo;
        }
    }
}