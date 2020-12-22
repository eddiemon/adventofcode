using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using aoc;

var input = File.ReadAllLines("../../../22.in");

var deck1 = new Queue<long>();
var deck2 = new Queue<long>();
var parsedDeck = deck1;
foreach (var l in input)
{
    if (l.StartsWith("Player"))
        continue;
    if (string.IsNullOrEmpty(l))
    {
        parsedDeck = deck2;
        continue;
    }

    parsedDeck.Enqueue(long.Parse(l));
}

var round = 1;
while (true)
{
    //Console.WriteLine($"Round {round}");
    var card1 = deck1.Dequeue();
    var card2 = deck2.Dequeue();
    if (card1 > card2)
    {
        //Console.WriteLine("Player 1 wins the round");
        deck1.Enqueue(card1);
        deck1.Enqueue(card2);
    }
    else if (card2 > card1)
    {
        //Console.WriteLine("Player 2 wins the round");
        deck2.Enqueue(card2);
        deck2.Enqueue(card1);
    }
    else
        System.Diagnostics.Debug.Fail("");

    if (deck1.Count == 0 || deck2.Count == 0)
        break;
}

//Console.Write("Player 1's deck: ");
//Console.Write(string.Join(", ", deck1));
//Console.WriteLine();

//Console.Write("Player 2's deck: ");
//Console.Write(string.Join(", ", deck2));
//Console.WriteLine();

long p1 = 0;
for (int i = deck1.Count; i > 0; i--)
{
    p1 += i * deck1.Dequeue();
}

long p2 = 0;
for (int i = deck2.Count; i > 0; i--)
{
    p1 += i * deck2.Dequeue();
}

Console.WriteLine($"P1: {p1}, P2: {p2}");
