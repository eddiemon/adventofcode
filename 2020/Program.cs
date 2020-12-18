using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using aoc;

var input = File.ReadAllLines("../../../18.in").Select(l => Maths.Evaluate(l));

Console.WriteLine(input.Sum());
