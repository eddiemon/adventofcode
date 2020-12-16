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

int[] myTicket = new int[0];
var nearbyTickets = new List<int[]>();

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
        var t = l.Split(',').Select(i => int.Parse(i)).ToArray();
        nearbyTickets.Add(t);
    }
    else if (readingMyTicket)
    {
        var t = l.Split(',').Select(i => int.Parse(i)).ToArray();
        myTicket = t;
    }
    else
    {
        var m = Regex.Match(l, @"^(?<rule>.+): (?<min1>\d+)\-(?<max1>\d+) or (?<min2>\d+)\-(?<max2>\d+)$");
        var rule = m.Groups["rule"].Value;
        var min1 = int.Parse(m.Groups["min1"].Value);
        var max1 = int.Parse(m.Groups["max1"].Value);
        var min2 = int.Parse(m.Groups["min2"].Value);
        var max2 = int.Parse(m.Groups["max2"].Value);
        Func<int, bool> validation = (int n) => (n >= min1 && n <= max1) || (n >= min2 && n <= max2);
        rules.Add(rule, validation);
    }
}

var validNearbyTickets = nearbyTickets.Where(t => TicketIsValid(t)).ToList();

var validations = rules.Select(kvp => kvp.Value).ToList();

var columnValidators = new Dictionary<Func<int, bool>, int>();
while (validations.Count > 0)
{
    for (int i = 0; i < myTicket.Length; i++)
    {
        var fieldIsDepartureField = validations.Where(v => validNearbyTickets.All(t => v(t[i]))).ToList();
        if (fieldIsDepartureField.Count == 1)
        {
            validations.Remove(fieldIsDepartureField.First());
            columnValidators.Add(fieldIsDepartureField.First(), i);
            break;
        }
    }
}

var departureValidations = rules.Where(kvp => kvp.Key.Contains("departure")).Select(kvp => kvp.Value).ToList();
var product = 1L;
foreach (var validation in departureValidations)
{
    var column = columnValidators[validation];
    product *= myTicket[column];
}
Console.WriteLine(product);

bool TicketIsValid(int[] t)
{
    foreach (var n in t)
    {
        var isValid = rules.Values.Any(valid => valid(n));
        if (!isValid)
            return false;
    }
    return true;
}
