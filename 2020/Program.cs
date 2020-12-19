using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using aoc;

var input = File.ReadAllLines("../../../19.in");

var rrules = new Dictionary<int, string>();
var rules = new Dictionary<int, Rule>();
var messages = new List<string>();
var parsingMessages = false;
foreach (var l in input)
{
    if (l == string.Empty)
    {
        parsingMessages = true;
        continue;
    }

    if (!parsingMessages)
    {
        var ruleParse = Regex.Match(l, @"(?<ruleNumber>\d+): (?<rule>.*)");
        var ruleNumber = int.Parse(ruleParse.Groups["ruleNumber"].Value);
        rrules.Add(ruleNumber, ruleParse.Groups["rule"].Value);
    }
    else
    {
        messages.Add(l);
    }
}

rrules[8] = "42 | 42 42 | 42 42 42 | 42 42 42 42 | 42 42 42 42 42 | 42 42 42 42 42 42";
rrules[11] = "42 31 | 42 42 31 31 | 42 42 42 31 31 31 | 42 42 42 42 31 31 31 31 | 42 42 42 42 42 31 31 31 31 31";

var messagesMatchingRule = messages.Where(m => Regex.IsMatch(m, "^" + GetRule(0).ToString() + "$")).ToList();
Console.WriteLine(messagesMatchingRule.Count);

Rule GetRule(int ruleNumber, int stack = 0)
{
    if (!rules.ContainsKey(ruleNumber))
        rules[ruleNumber] = ToRule(rrules[ruleNumber]);

    return rules[ruleNumber];
}

Rule ToRule(string r)
{
    if (r == "\"a\"" || r == "\"b\"")
    {
        return new ConstantRule(r[1]);
    }
    else if (r.IndexOf(" | ") > 0)
    {
        var pipeSplit = r.Split(" | ");
        var andRules = pipeSplit.Select(s => new AndRule(s.Trim().Split(' ').Select(ss => GetRule(int.Parse(ss))).ToArray()));
        return new OrRule(andRules.ToArray());
    }
    else
    {
        return new AndRule(r.Trim().Split(' ').Select(s => GetRule(int.Parse(s))).ToArray());
    }
}

#region selector

internal abstract class Rule
{
}

internal class ConstantRule : Rule
{
    public ConstantRule(char theChar)
    {
        TheChar = theChar;
    }

    public char TheChar { get; }

    public override string ToString() => TheChar.ToString();
}

internal class AndRule : Rule
{
    public AndRule(params Rule[] rules)
    {
        Rules = rules.ToList();
    }

    public List<Rule> Rules { get; }

    public override string ToString() => string.Join("", Rules);
}

internal class OrRule : Rule
{
    public OrRule(params Rule[] rules)
    {
        Rules = rules.ToList();
    }

    public List<Rule> Rules { get; }

    public override string ToString() => "(" + string.Join("|", Rules.Select(r => r.ToString())) + ")";
}

#endregion selector
