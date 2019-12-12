namespace aoc2019 {
    public static class Maths {
        
        public static int gcd(int a, int b) => a == 0 ? b : gcd(b % a, a);
        public static int lcm(int a, int b) => System.Math.Abs(a * b) / gcd(a, b);
    }
}