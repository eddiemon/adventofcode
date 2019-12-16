using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc
{
    internal class D16
    {
        static int[] basePattern = new int[] { 0, 1, 0, -1 };
        const int phases = 100;

        public object Answer()
        {
            var numbers = Enumerable.Repeat(File.ReadAllText("16.in").Select(c => int.Parse(c.ToString())), 10).SelectMany(x => x).ToArray();

            for (int p = 0; p < phases; p++)
            {
                ApplyPhase(numbers);
            }

            return string.Join("", numbers.Take(8));
        }

        private void ApplyPhase(int[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                var pattern = GeneratePattern(i + 1).GetEnumerator();
                var sum = 0;
                for (int j = 0; j < numbers.Length && pattern.MoveNext(); j++)
                {
                    if (pattern.Current == 0) continue;
                    else if (pattern.Current == 1) sum += numbers[j];
                    else if (pattern.Current == -1) sum -= numbers[j];
                    //sum += numbers[j] * pattern.Current;
                }
                var n = sum.ToString();
                numbers[i] = (int)(n[n.Length - 1] - '0');
                Console.WriteLine($"Calculated number {i} of {numbers.Length}");
            }
        }

        private IEnumerable<int> GeneratePattern(int repeats)
        {
            var basePatternIdx = 0;
            int ret = basePattern[basePatternIdx];

            for (int i = 0; i < repeats - 1; i++)
            {
                yield return ret;
            }

            basePatternIdx = (basePatternIdx + 1) % basePattern.Length;
            ret = basePattern[basePatternIdx];

            while (true)
            {
                for (int i = 0; i < repeats; i++)
                {
                    yield return ret;
                }
                basePatternIdx = (basePatternIdx + 1) % basePattern.Length;
                ret = basePattern[basePatternIdx];
            }

        }
    }
}