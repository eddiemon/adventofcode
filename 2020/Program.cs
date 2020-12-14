using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../14.in");

var mem = new Dictionary<string, ulong>();

string mask = string.Empty;

var sb = new StringBuilder();

var maskR = new Regex("^mask = (?<mask>.+)$");
var memR = new Regex("^mem\\[(?<addr>\\d+)\\] = (?<num>\\d+)$");
foreach (var l in input)
{
    var maskM = maskR.Match(l);
    var memM = memR.Match(l);
    if (maskM.Success)
    {
        mask = maskM.Groups["mask"].Value;
    }
    else if (memM.Success)
    {
        var addr = long.Parse(memM.Groups["addr"].Value);
        var bitStr = Convert.ToString(addr, 2);
        int i = bitStr.Length - 1;
        for (int j = mask.Length - 1; j >= 0; j--)
        {
            if (mask[j] == 'X')
                sb.Append('X');
            else if (mask[j] == '1')
                sb.Append('1');
            else if (i >= 0)
                sb.Append(bitStr[i]);
            else
                sb.Append('0');
            i--;
        }
        var fullAddr = new string(sb.ToString().Reverse().ToArray());
        sb.Clear();

        var num = ulong.Parse(memM.Groups["num"].Value);
        WriteValueToMem(fullAddr, num);
    }
    else
    {
        System.Diagnostics.Debug.Fail("");
    }
}

void WriteValueToMem(string addr, ulong num)
{
    int xs = 0;
    foreach (var c in addr)
    {
        if (c == 'X') xs++;
    }

    var max = (long)Math.Pow(2, xs);
    for (var i = 0; i < max; i++)
    {
        var ss = Convert.ToString(i, 2);
        int j = ss.Length - 1;
        foreach (var c in addr.Reverse())
        {
            if (c == 'X')
                if (j >= 0)
                    sb.Append(ss[j--]);
                else
                    sb.Append('0');
            else
                sb.Append(c);
        }
        var newAddr = new string(sb.ToString().Reverse().ToArray());
        mem[newAddr] = num;
        sb.Clear();
    }
}

Console.WriteLine(mem.Values.Aggregate((k1, k2) => k1 + k2));
