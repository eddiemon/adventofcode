using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../7.in");
var rules = new List<Rule>();

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
        rules.Add(new Rule(color, subRules));
    }
}

{
    var count = 0;
    foreach (var r in rules)
    {
        if (CanContainBag(r, "shiny gold"))
            count++;
    }
    Console.WriteLine(count);
}

bool CanContainBag(Rule rule, string bagColor)
{
    if (rule.subRules.Count == 0)
        return false;
    if (rule.subRules.Any(r => r.color == bagColor))
        return true;
    foreach (var subRule in rule.subRules)
    {
        if (CanContainBag(rules.First(r => r.color == subRule.color), bagColor))
            return true;
    }
    return false;
}

record Rule(string color, List<(string color, int Count)> subRules);
