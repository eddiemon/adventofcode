using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../14.in");

var mem = new Dictionary<ulong, ulong>();

ulong clearBits = 0UL;
ulong setBits = 0UL;
var maskR = new Regex("^mask = (?<mask>.+)$");
var memR = new Regex("^mem\\[(?<addr>\\d+)\\] = (?<num>\\d+)$");
foreach (var l in input)
{
    var maskM = maskR.Match(l);
    var memM = memR.Match(l);
    if (maskM.Success)
    {
        clearBits = ulong.MaxValue;
        setBits = 0;
        int exp = 35;
        foreach (var c in maskM.Groups["mask"].Value)
        {
            if (c == '1')
                setBits += 1UL << exp;
            if (c == '0')
                clearBits -= 1UL << exp;
            exp--;
        }
    }
    else if (memM.Success)
    {
        var addr = ulong.Parse(memM.Groups["addr"].Value);
        var num = ulong.Parse(memM.Groups["num"].Value);
        num |= setBits;
        num &= clearBits;
        mem[addr] = num;
    }
    else
    {
        System.Diagnostics.Debug.Fail("");
    }
}

Console.WriteLine(mem.Values.Aggregate((k1, k2) => k1 + k2));
