using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllText("../../../4.in");

var pIn = input.Split("\n\n");
int validPassports = 0;
foreach (var p in pIn)
{
    var d = p.Replace('\n', ' ').Split(' ').Where(pp => pp != "").ToDictionary(
        pp => pp.Substring(0, pp.IndexOf(':')),
        pp => pp.Substring(pp.IndexOf(':') + 1)
    );
    if (d.ContainsKey("byr") && d.ContainsKey("iyr") && d.ContainsKey("eyr") && d.ContainsKey("hgt") && d.ContainsKey("hcl") && d.ContainsKey("ecl") && d.ContainsKey("pid"))
        validPassports++;
}

Console.WriteLine(validPassports);
