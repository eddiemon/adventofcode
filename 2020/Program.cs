using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../13.in");
var estimatedTime = long.Parse(input[0]);
var busIds = input[1].Split(',').Where(c => c != "x").Select(c => long.Parse(c)).ToArray();

var bus = busIds.Select(id => (id, time: id - estimatedTime % id)).OrderBy(n => n.time).First();
Console.WriteLine(bus.id * bus.time);
