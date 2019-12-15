using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc
{
    public class D14
    {
        public string Answer()
        {
            ReactionPerProduct = File.ReadAllLines("D14.txt").Select(s => Reaction.FromString(s)).ToDictionary(r => r.Product.ReactantName);

            foreach (var r in ReactionPerProduct.Keys) Storage[r] = 0;

            var fuel = Maths.LongBinarySearch(0, (long)1e12, numFuel => {
                OreRequired = 0;
                ProduceReactantOrder(new ReactantOrder("FUEL", numFuel));
                return OreRequired > 1e12;
            });

            return fuel.ToString();
        }

        Dictionary<string, long> Storage = new Dictionary<string, long>();
        Dictionary<string, Reaction> ReactionPerProduct = null;

        long OreRequired = 0;

        private void ProduceReactantOrder(ReactantOrder order)
        {
            checked
            {
                if (order.ReactantName == "ORE")
                {
                    OreRequired += order.Amount;
                    return;
                }

                if (Storage[order.ReactantName] >= order.Amount)
                {
                    Storage[order.ReactantName] -= order.Amount;
                    return;
                }
                else
                {
                    order.Amount -= Storage[order.ReactantName];
                    Storage[order.ReactantName] = 0;
                }

                var r = ReactionPerProduct[order.ReactantName];

                var reactions = (long)Math.Ceiling(order.Amount / (double)r.Product.Amount);
                foreach (var reactant in r.Reactants)
                {
                    ProduceReactantOrder(new ReactantOrder(reactant.ReactantName, reactant.Amount * reactions));
                }

                var leftOverReactantAfterOrder = r.Product.Amount * reactions - order.Amount;

                if (leftOverReactantAfterOrder > 0)
                {
                    Storage[order.ReactantName] = leftOverReactantAfterOrder;
                }
            }
        }

        class Reaction
        {
            private static Regex r = new Regex(@"(\d+)\s(\w+)");

            public List<ReactantOrder> Reactants = new List<ReactantOrder>();
            public ReactantOrder Product;

            public static Reaction FromString(string s)
            {
                var reaction = new Reaction();

                var matches = r.Matches(s);
                for (int i = 0; i < matches.Count - 1; i++)
                {
                    var m = matches[i];
                    reaction.Reactants.Add(new ReactantOrder(m.Groups[2].ToString(), int.Parse(m.Groups[1].ToString())));
                }

                var lastMatch = matches[matches.Count - 1];
                reaction.Product = new ReactantOrder(lastMatch.Groups[2].ToString(), int.Parse(lastMatch.Groups[1].ToString()));

                return reaction;
            }

            public override string ToString()
            {
                return string.Join(", ", Reactants.Select(x => $"{x.Amount} {x.ReactantName}")) + " => " + $"{Product.Amount} {Product.ReactantName}";
            }
        }

        struct ReactantOrder
        {
            public readonly string ReactantName;
            public long Amount;

            [System.Diagnostics.DebuggerStepThrough]
            public ReactantOrder(string reactantName, long amount)
            {
                ReactantName = reactantName;
                Amount = amount;
            }

            public override string ToString() => $"{Amount} {ReactantName}";
        }
    }
}
