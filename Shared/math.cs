using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

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

    /// <summary>
    /// Evaluates a mathematical expression like "1+2*3+(1*3)+1"
    /// </summary>
    /// <remarks>
    /// Internally converts infix to polish reverse notation using Shunting-yard algorithm.
    /// Handles +, * and groups (parenthesis)<br/>
    /// https://en.wikipedia.org/wiki/Shunting-yard_algorithm
    /// </remarks>
    public static long Evaluate(string expression)
    {
        var numbuilder = new StringBuilder();
        var outputQueue = new Queue<string>();
        var opStack = new Stack<char>();

        for (int i = 0; i < expression.Length; i++)
        {
            if (expression[i] >= '0' && expression[i] <= '9')
            {
                numbuilder.Append(expression[i]);
                continue;
            }

            if (numbuilder.Length > 0)
            {
                outputQueue.Enqueue(numbuilder.ToString());
                numbuilder.Clear();
            }

            if (expression[i] == '+' || expression[i] == '*')
            {
                while (opStack.Count > 0 && opStack.Peek().HasHigherPrecedenceThan(expression[i]))
                {
                    outputQueue.Enqueue(opStack.Pop().ToString());
                }
                opStack.Push(expression[i]);
                continue;
            }

            if (expression[i] == '(')
            {
                opStack.Push('(');
                continue;
            }

            if (expression[i] == ')')
            {
                while (opStack.Count > 0 && opStack.Peek() != '(')
                    outputQueue.Enqueue(opStack.Pop().ToString());

                if (opStack.Count > 0 && opStack.Peek() == '(')
                    opStack.Pop();
            }
        }

        if (numbuilder.Length > 0)
        {
            outputQueue.Enqueue(numbuilder.ToString());
        }

        while (opStack.Count > 0)
            outputQueue.Enqueue(opStack.Pop().ToString());

        var resultStack = new Stack<long>();
        while (outputQueue.Count > 0)
        {
            var x = outputQueue.Dequeue();
            if (x == "+")
            {
                var a = resultStack.Pop();
                var b = resultStack.Pop();
                var sum = a + b;
                resultStack.Push(sum);
            }
            else if (x == "*")
            {
                var a = resultStack.Pop();
                var b = resultStack.Pop();
                var prod = a * b;
                resultStack.Push(prod);
            }
            else
            {
                resultStack.Push(long.Parse(x));
            }
        }

        return resultStack.Pop();
    }
}

public static class OpExtensions
{
    /// <summary>
    /// Operation precedence as used in 2020 day 18 part 2
    /// </summary>
    public static bool HasHigherPrecedenceThan(this char op, char otherOp)
    {
        return op == '+' && otherOp == '*';
    }
}
