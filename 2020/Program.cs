using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../16.in");

var readingMyTicket = false;
var readingNearbyTickets = false;

var rules = new Dictionary<string, Func<int, bool>>();
var ticketScanningErrorRate = 0;

foreach (var l in input)
{
    if (string.IsNullOrEmpty(l))
        continue;
    if (l == "your ticket:")
    {
        readingMyTicket = true;
        continue;
    }
    if (l == "nearby tickets:")
    {
        readingNearbyTickets = true;
        continue;
    }

    if (readingNearbyTickets)
    {
        var t = l.Split(',').Select(i => int.Parse(i));
        foreach (var n in t)
        {
            bool found = false;
            foreach (var (rule, validate) in rules)
            {
                if (validate(n))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
                ticketScanningErrorRate += n;
        }
    }
    else if (readingMyTicket)
    {
    }
    else
    {
        var m = Regex.Match(l, @"^(?<rule>.+): (?<min1>\d+)\-(?<max1>\d+) or (?<min2>\d+)\-(?<max2>\d+)$");
        var rule = m.Groups["rule"].Value;
        var min1 = int.Parse(m.Groups["min1"].Value);
        var max1 = int.Parse(m.Groups["max1"].Value);
        var min2 = int.Parse(m.Groups["min2"].Value);
        var max2 = int.Parse(m.Groups["max2"].Value);
        Func<int, bool> validation = (int n) => n >= min1 && n <= max1 || n >= min2 && n <= max2;
        rules.Add(rule, validation);
    }
}

Console.WriteLine(ticketScanningErrorRate);
