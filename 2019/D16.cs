using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace aoc
{
    internal class D16
    {
        const int phases = 100;

        public object Answer()
        {
            var originalCode = File.ReadAllText("16.in").Select(c => int.Parse(c.ToString())).ToArray();

            var sb = new StringBuilder();
            foreach (var n in originalCode.AsSpan().Slice(0, 7))
                sb.Append(n.ToString());

            var pos = int.Parse(sb.ToString());
            var c = Enumerable.Repeat(originalCode, 10000).SelectMany(x => x).ToArray();
            var code = c.AsSpan().Slice(pos);

            for (int phase = 0; phase < phases; phase++)
            {
                var total = 0;
                var p = code.Length - 1;
                while (p >= 0)
                {
                    total += code[p];
                    code[p] = total % 10;
                    --p;
                }
            }

            sb.Clear();
            foreach (var n in code.Slice(0, 8))
                sb.Append(n.ToString());
            return sb.ToString();
        }
    }
}