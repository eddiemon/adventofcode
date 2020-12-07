using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../7.in");
var rules = new List<Bag>();

//light red bags contain 1 bright white bag, 2 muted yellow bags.
{
    foreach (var i in input)
    {
        var index = i.IndexOf(" bags");
        var color = i[0..index];
        index = i.IndexOf("contain ");
        var a = i[(index + "contain ".Length)..];
        var b = a.Split(',', '.').Where(aa => aa != "" && aa != "no other bags");
        var subRules = new List<(string color, int Count)>();
        foreach (var bb in b)
        {
            var m = Regex.Match(bb, @"(?<count>\d) (?<color>.*) bag");
            var count = int.Parse(m.Groups["count"].Value);
            var c = m.Groups["color"].Value;
            subRules.Add((c, count));
        }
        rules.Add(new Bag(color, subRules));
    }
}

{
    var shinyGold = rules.First(r => r.color == "shiny gold");
    Console.WriteLine(BagsWithinBag(shinyGold) - 1);
}

int BagsWithinBag(Bag bag)
{
    if (bag.ContainsBags.Count == 0)
        return 1;

    var numberOfBags = 1;
    foreach (var subRule in bag.ContainsBags)
    {
        var r = rules.First(r => r.color == subRule.color);
        numberOfBags += subRule.Count * BagsWithinBag(r);
    }
    return numberOfBags;
}

record Bag(string color, List<(string color, int Count)> ContainsBags);
