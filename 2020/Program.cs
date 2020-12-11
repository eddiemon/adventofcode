using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../10.3.in").Select(n => int.Parse(n)).ToList();

input.Sort();
//input.Insert(0, 0);
//input.Add(input.Max() + 3);

var inp = input.ToArray().AsSpan();

//int arrangements = 1;
//for (int i = 1; i < inp.Length - 2; i++)
//{
//    if (inp[i + 1] - inp[i] == 1)
//        arrangements++;
//}

//Console.WriteLine(arrangements);

//var arrangements = new List<int>();
//for (int i = 0; i < inp.Length - 1; i++)
//{
//    var n = inp[i];
//    var canConnect = 0;
//    if (inp.Length > i + 1 && inp[i + 1] <= n + 3) canConnect++;
//    if (inp.Length > i + 2 && inp[i + 2] <= n + 3) canConnect++;
//    if (inp.Length > i + 3 && inp[i + 3] <= n + 3) canConnect++;
//    arrangements.Add(canConnect);
//}

//bool multiplying = false;
//int multiplier = 1;
//long sum = 0;
//for (int i = 0; i < arrangements.Count; i++)
//{
//    if (arrangements[i] > 1)
//    {
//        multiplying = true;
//        multiplier *= arrangements[i];
//    }
//    else
//    {
//        if (multiplying)
//            sum += multiplier;
//        multiplying = false;
//        multiplier = 1;
//    }
//}
//Console.WriteLine(sum);
//var numOfArrangements = CountArrangements(inp[0], inp[1..]);
//Console.WriteLine(numOfArrangements);

Console.WriteLine(CountArrangements(10, new[] { 11, 12, 13, 15 }));

int CountArrangements(int n, Span<int> numbers)
{
    if (numbers.Length <= 1)
        return 1;

    int numsInRange = 0;
    for (int i = 0; i < 3 && i < numbers.Length; i++)
    {
        if (numbers[i] - n <= 3)
            numsInRange++;
    }

    int arrangements = numsInRange == 3 ? 7 : 2;

    if (numbers.Length == 1)
        return arrangements;

    if (numbers[0] - n == 1)
        arrangements++;

    return arrangements * CountArrangements(numbers[numsInRange], numbers[numsInRange..]);
}
