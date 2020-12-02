using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var r = new Regex(@"(?<min>\d+)-(?<max>\d+) (?<l>.): (?<pw>.+)");
var input = File.ReadAllLines("../../../2.in").Select(l =>
{
    var m = r.Match(l);
    return new Line(int.Parse(m.Groups["min"].Value), int.Parse(m.Groups["max"].Value), m.Groups["l"].Value, m.Groups["pw"].Value);
}).ToList();

var validPw = new List<string>();
// part 1
//foreach (var line in input)
//{
//    var charCount = 0;
//    for (int i = line.min; i < line.pw.Length; i++)
//    {
//        if (line.pw[i] == line.l[0])
//            charCount++;
//    }
//    if (charCount >= line.min && charCount <= line.max)
//        validPw.Add(line.pw);
//}

// part 2
foreach (var line in input)
{
    var charCount = 0;
    if (line.pw[line.min - 1] == line.l[0]) charCount++;
    if (line.pw[line.max - 1] == line.l[0]) charCount++;
    //for (int i = line.min; i < line.pw.Length; i++)
    //{
    //    if (line.pw[i] == line.l[0])
    //        charCount++;
    //}
    if (charCount == 1)
        validPw.Add(line.pw);
}

Console.WriteLine(validPw.Count);

record Line(int min, int max, string l, string pw);
