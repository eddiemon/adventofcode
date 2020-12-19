using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using aoc;

var input = File.ReadAllLines("../../../19.1.in");

var rules = new Dictionary<int, RuleSelector>();
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
        var ruleNumberSeparatorIdx = l.IndexOf(":");
        var ruleParse = Regex.Match(l, @"(?<ruleNumber>\d+): (?<char>""."")?(?<subRule1>[^\|]+)?\|?(?<subRule2>.*)?");
        var ruleNumber = int.Parse(ruleParse.Groups["ruleNumber"].Value);
        if (ruleParse.Groups["subRule2"].Value != string.Empty)
        {
            var subRule2 = ruleParse.Groups["subRule2"].Value.Trim().Split(' ').Select(s => int.Parse(s)).ToArray();
            var subRule1 = ruleParse.Groups["subRule1"].Value.Trim().Split(' ').Select(s => int.Parse(s)).ToArray();
            var ruleSelector = new CompoundChainRuleSelector(new ChainRuleSelector(subRule1), new ChainRuleSelector(subRule2));
            rules.Add(ruleNumber, ruleSelector);
        }
        else if (ruleParse.Groups["subRule1"].Value != string.Empty)
        {
            var subRule1 = ruleParse.Groups["subRule1"].Value.Trim().Split(' ').Select(s => int.Parse(s)).ToArray();
            var ruleSelector = new ChainRuleSelector(subRule1);
            rules.Add(ruleNumber, ruleSelector);
        }
        else if (ruleParse.Groups["char"].Value != string.Empty)
        {
            var theChar = ruleParse.Groups["char"].Value[1];
            var ruleSelector = new ConstantRuleSelector(theChar);
            rules.Add(ruleNumber, ruleSelector);
        }
        else
        {
            throw new Exception();
        }
    }
    else
    {
        messages.Add(l);
    }
}

rules[8] = new CompoundChainRuleSelector(new ChainRuleSelector(new[] { 42 }), new ChainRuleSelector(new[] { 42, 8 }));
rules[11] = new CompoundChainRuleSelector(new ChainRuleSelector(new[] { 42, 31 }), new ChainRuleSelector(new[] { 42, 11, 31 }));

var messagesMatchingRule = messages.Where(m => StringMatchWholeRule(m, 0)).ToList();
Console.WriteLine(messagesMatchingRule.Count);

bool StringMatchWholeRule(ReadOnlySpan<char> l, int ruleNumber)
{
    var res = StringMatchRule(l, ruleNumber);
    return res.match && res.consumedChars == l.Length;
}

(int consumedChars, bool match) StringMatchRule(ReadOnlySpan<char> l, int ruleNumber)
{
    if (l.Length == 0)
        return (0, true);

    RuleSelector ruleSelector = rules[ruleNumber];

    if (ruleSelector is ConstantRuleSelector a)
    {
        return (1, l[0] == a.TheChar);
    }
    else if (ruleSelector is ChainRuleSelector b)
    {
        return ChainRuleMatch(l, b);
    }
    else if (ruleSelector is CompoundChainRuleSelector c)
    {
        var ruleSelectors = c.RuleSelector;

        foreach (var bb in ruleSelectors)
        {
            var res = ChainRuleMatch(l, bb);
            if (res.match)
                return res;
        }
    }

    return (0, false);

    (int consumedChars, bool match) ChainRuleMatch(ReadOnlySpan<char> l, ChainRuleSelector b)
    {
        int consumedChars = 0;
        bool allMatch = true;
        foreach (var subRuleNumber in b.SubRules)
        {
            var res = StringMatchRule(l[consumedChars..], subRuleNumber);
            consumedChars += res.consumedChars;
            if (!res.match)
            {
                allMatch = false;
                break;
            }
        }
        return (consumedChars, allMatch);
    }
}

#region selector

internal class RuleSelector
{
}

internal class ConstantRuleSelector : RuleSelector
{
    public ConstantRuleSelector(char theChar)
    {
        TheChar = theChar;
    }

    public char TheChar { get; }
}

internal class ChainRuleSelector : RuleSelector
{
    public ChainRuleSelector(int[] subRules)
    {
        SubRules = subRules;
    }

    public int[] SubRules { get; }
}

internal class CompoundChainRuleSelector : RuleSelector
{
    public CompoundChainRuleSelector(params ChainRuleSelector[] ruleSelector)
    {
        RuleSelector = ruleSelector;
    }

    public IEnumerable<ChainRuleSelector> RuleSelector { get; }
}

#endregion selector
