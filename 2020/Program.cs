using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using aoc;

//var input = File.ReadAllLines("../../../22.in");
var cups = new LinkedList<int>("389125467".Select(c => c - '0').Concat(Enumerable.Range(10, 1000000 - 9)));
var hiCup = cups.Max();
var cup = cups.First;

var valToCup = new Dictionary<int, LinkedListNode<int>>();
var cc = cups.First;
while (cc != null)
{
    valToCup.Add(cc.Value, cc);
    cc = cc.Next;
}

for (int i = 0; i < 10_000000; i++)
{
    var cupsToMove = new List<LinkedListNode<int>>();
    for (int ii = 0; ii < 3; ii++)
    {
        var next = cup.Next;
        if (next == null)
            next = cups.First;
        cupsToMove.Add(next);
        cups.Remove(next);
    }

    var dest = cup.Value - 1;
    while (true)
    {
        if (dest < 1) dest = hiCup;

        if (!cupsToMove.Select(c => c.Value).Contains(dest))
            break;

        dest = (char)(dest - 1);
    }

    var destNode = valToCup[dest];
    cupsToMove.Reverse();
    foreach (var c in cupsToMove)
    {
        cups.AddAfter(destNode, c);
    }

    cup = cup.Next;
    if (cup == null)
        cup = cups.First;
}

var onePlusOne = valToCup[1].Next ?? cups.First;
var onePlusTwo = onePlusOne.Next ?? cups.First;
Console.WriteLine((long)onePlusOne.Value * (long)onePlusTwo.Value);
