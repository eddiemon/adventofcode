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

RecursiveCombat(deck1, deck2);

// Return value is true if p1 won the round. otherwise p2 won.
bool RecursiveCombat(Queue<long> deck1, Queue<long> deck2)
{
    var p1DecksSeen = new HashSet<int>();
    var p2DecksSeen = new HashSet<int>();
    while (true)
    {
        var h = GetDeckHash(deck1);
        if (p1DecksSeen.Contains(h.ToHashCode()))
        {
            Console.WriteLine("Seen player 1's deck before");
            return true;
        }
        else
            p1DecksSeen.Add(h.ToHashCode());

        h = GetDeckHash(deck2);
        if (p2DecksSeen.Contains(h.ToHashCode()))
        {
            Console.WriteLine("Seen player 2's deck before");
            return true;
        }
        else
            p2DecksSeen.Add(h.ToHashCode());

        var card1 = deck1.Dequeue();
        var card2 = deck2.Dequeue();

        if (deck1.Count >= card1 && deck2.Count >= card2)
        {
            var p1Won = RecursiveCombat(new Queue<long>(deck1.Take((int)card1)), new Queue<long>(deck2.Take((int)card2)));
            if (p1Won)
            {
                deck1.Enqueue(card1);
                deck1.Enqueue(card2);
            }
            else
            {
                deck2.Enqueue(card2);
                deck2.Enqueue(card1);
            }
        }
        else
        {
            if (card1 > card2)
            {
                deck1.Enqueue(card1);
                deck1.Enqueue(card2);
            }
            else if (card2 > card1)
            {
                deck2.Enqueue(card2);
                deck2.Enqueue(card1);
            }
            else
                System.Diagnostics.Debug.Fail("");
        }

        if (deck1.Count == 0)
            return false;
        else if (deck2.Count == 0)
            return true;
    }

    throw new Exception();
}

long p1 = 0;
for (int i = deck1.Count; i > 0; i--)
{
    p1 += i * deck1.Dequeue();
}

long p2 = 0;
for (int i = deck2.Count; i > 0; i--)
{
    p2 += i * deck2.Dequeue();
}

Console.WriteLine($"P1: {p1}, P2: {p2}");

HashCode GetDeckHash(IEnumerable<long> deck)
{
    var h = new HashCode();
    foreach (var card in deck)
    {
        h.Add(card);
    }
    return h;
}
