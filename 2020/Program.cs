using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using aoc;

var input = File.ReadAllLines("../../../18.in");

var sum = input.Select(i => RecursiveForwardEvaluate(i).result).Sum();

Console.WriteLine(sum);

(int eaten, long result) RecursiveForwardEvaluate(ReadOnlySpan<char> v)
{
    var nums = new Queue<long>();
    var ops = new Queue<char>();
    var numBuilder = new StringBuilder();
    int i = 0;
    for (; i < v.Length; i++)
    {
        if (v[i] >= '0' && v[i] <= '9')
        {
            numBuilder.Append(v[i]);
            continue;
        }

        if (numBuilder.Length > 0)
        {
            nums.Enqueue(long.Parse(numBuilder.ToString()));
            numBuilder.Clear();
        }

        if (v[i] == '+' || v[i] == '*')
        {
            ops.Enqueue(v[i]);
            continue;
        }

        if (v[i] == '(')
        {
            var (eaten, result) = RecursiveForwardEvaluate(v[(i + 1)..]);
            nums.Enqueue(result);
            i += eaten;
            continue;
        }

        if (v[i] == ')')
        {
            i++;
            break;
        }
    }

    if (numBuilder.Length > 0)
    {
        nums.Enqueue(int.Parse(numBuilder.ToString()));
        numBuilder.Clear();
    }

    var r = nums.Dequeue();
    while (ops.Count > 0)
    {
        var op = ops.Dequeue();
        if (op == '+')
        {
            r += nums.Dequeue();
        }
        else
        {
            r *= nums.Dequeue();
        }
    }
    return (i, r);
}
