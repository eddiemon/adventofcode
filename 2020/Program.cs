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
    try
    {
        var d = p.Replace('\n', ' ').Split(' ').Where(pp => pp != "").ToDictionary(
            pp => pp.Substring(0, pp.IndexOf(':')),
            pp => pp.Substring(pp.IndexOf(':') + 1)
        );
        if (int.Parse(d["byr"]) < 1920 || int.Parse(d["byr"]) > 2002)
            throw new Exception();
        if (int.Parse(d["iyr"]) < 2010 || int.Parse(d["iyr"]) > 2020)
            throw new Exception();
        if (int.Parse(d["eyr"]) < 2020 || int.Parse(d["eyr"]) > 2030)
            throw new Exception();
        var hgtMatch = Regex.Match(d["hgt"], @"^(?<num>\d+)(?<u>.{2})$");
        if (!hgtMatch.Success)
            throw new Exception();
        var length = int.Parse(hgtMatch.Groups["num"].Value);
        switch (hgtMatch.Groups["u"].Value)
        {
            case "cm":
                if (length < 150 || length > 193)
                    throw new Exception();
                break;

            case "in":
                if (length < 59 || length > 76)
                    throw new Exception();
                break;

            default:
                throw new Exception();
        }
        if (!Regex.IsMatch(d["hcl"], "^#[0-9a-z]{6}$"))
            throw new Exception();

        var validEcl = new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
        if (!validEcl.Any(ecl => ecl == d["ecl"]))
            throw new Exception();

        if (!Regex.IsMatch(d["pid"], "^[0-9]{9}$"))
            throw new Exception();

        validPassports++;
    }
    catch (Exception) { }
}

Console.WriteLine(validPassports);
