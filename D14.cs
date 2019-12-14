using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc2019
{
    public class D14
    {
        public string Answer()
        {
            keyedReactions = File.ReadAllLines("D14.txt").Select(s => Reaction.FromString(s)).ToDictionary(r => r.Produces.Key);

            foreach (var r in keyedReactions.Keys) Storage[r] = 0;

            var fuelReaction = keyedReactions["FUEL"];
            var fuel = 0;
            long oresNeeded = 0;
            long newOresNeeded = 0;
            while (oresNeeded < 1_000_000_000_000)
            {
                newOresNeeded = ProduceOneReaction(fuelReaction);
                oresNeeded += newOresNeeded;

                fuel++;
            }

            fuel -= 1;

            return fuel.ToString();
        }

        Dictionary<string, Reaction> keyedReactions = null;

        Dictionary<string, int> Storage = new Dictionary<string, int>();

        private int ProduceOneReaction(Reaction reaction)
        {
            if (reaction.Input.ContainsKey("ORE"))
            {
                Storage[reaction.Produces.Key] += reaction.Produces.Value;
                return reaction.Input["ORE"];
            }

            int oreCount = 0;
            foreach (var r in reaction.Input)
            {
                var rr = keyedReactions[r.Key];
                while (Storage.GetValueOrDefault(r.Key) < r.Value)
                {
                    oreCount += ProduceOneReaction(rr);
                }
                Storage[r.Key] -= r.Value;
            }

            var alreadyProduced = Storage[reaction.Produces.Key];
            Storage[reaction.Produces.Key] = alreadyProduced + reaction.Produces.Value;

            return oreCount;
        }

        class Reaction
        {
            private static Regex r = new Regex(@"(\d+)\s(\w+)");

            public Dictionary<string, int> Input = new Dictionary<string, int>();
            public KeyValuePair<string, int> Produces;

            public static Reaction FromString(string s)
            {
                var reaction = new Reaction();

                var matches = r.Matches(s);
                for (int i = 0; i < matches.Count - 1; i++)
                {
                    var m = matches[i];
                    reaction.Input[m.Groups[2].ToString()] = int.Parse(m.Groups[1].ToString());
                }

                var lastMatch = matches[matches.Count - 1];
                reaction.Produces = new KeyValuePair<string, int>(lastMatch.Groups[2].ToString(), int.Parse(lastMatch.Groups[1].ToString()));

                return reaction;
            }

            public override string ToString()
            {
                return string.Join(", ", Input.Select(x => $"{x.Value} {x.Key}")) + " => " + $"{Produces.Value} {Produces.Key}";
            }
        }
    }
}
